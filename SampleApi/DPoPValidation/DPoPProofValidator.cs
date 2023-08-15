using System.Security.Cryptography;
using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.SampleAPI.DPoPValidation;

public class DPoPProofValidator
{
    // This is the number of seconds that we allow for the DPoP proof to expire
    private TimeSpan ProofTokenValidityDuration { get; set; } = TimeSpan.FromSeconds(1);
    // This is the maximum difference between the client's clock and this application's clock
    private TimeSpan ClientClockSkew { get; set; } = TimeSpan.FromSeconds(5);
    private const string ReplayCachePurpose = "DPoPJwtBearerEvents-DPoPReplay-jti-";

    private static readonly IEnumerable<string> SupportedDPoPSigningAlgorithms = new[]
    {
        SecurityAlgorithms.RsaSha256,
        SecurityAlgorithms.RsaSha384,
        SecurityAlgorithms.RsaSha512,

        SecurityAlgorithms.RsaSsaPssSha256,
        SecurityAlgorithms.RsaSsaPssSha384,
        SecurityAlgorithms.RsaSsaPssSha512,

        SecurityAlgorithms.EcdsaSha256,
        SecurityAlgorithms.EcdsaSha384,
        SecurityAlgorithms.EcdsaSha512
    };

    private readonly ILogger<DPoPProofValidator> _logger;
    private readonly IReplayCache _replayCache;
    
    public DPoPProofValidator(IReplayCache replayCache, ILogger<DPoPProofValidator> logger)
    {
        _replayCache = replayCache;
        _logger = logger;
    }

    public async Task<ValidationResult> Validate(DPoPProofValidationData data)
    {
        var validationResult = ValidateHeader(data);
        if (validationResult.IsError)
        {
            _logger.LogDebug("Failed to validate the DPoP header");
            return validationResult;
        }

        validationResult = ValidateSignature(data);
        if (validationResult.IsError)
        {
            _logger.LogDebug("Failed to validate the DPoP signature");
            return validationResult;
        }

        validationResult = ValidatePayload(data);
        if (validationResult.IsError)
        {
            _logger.LogDebug("Failed to validate the DPoP payload");
            return validationResult;
        }
        
        // Check for a replayed DPoP proof
        validationResult = await ValidateReplayAsync(data);
        if (validationResult.IsError)
        {
            _logger.LogDebug("Detected a replay of the DPoP token");
            return validationResult;
        }

        _logger.LogDebug("Successfully validated the DPoP proof token");

        return ValidationResult.Success();
    }

    private ValidationResult ValidateHeader(DPoPProofValidationData data)
    {
        JsonWebToken token;
        try
        {
            var handler = new JsonWebTokenHandler();
            token = handler.ReadJsonWebToken(data.ProofToken);
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Error parsing DPoP token: {error}", ex.Message);
            return ValidationResult.Error("Malformed DPoP token.");
        }

        if (!token.TryGetHeaderValue<string>("typ", out var typ) || typ != JwtClaimTypes.JwtTypes.DPoPProofToken)
        {
            return ValidationResult.Error("Invalid 'typ' value.");
        }

        if (!token.TryGetHeaderValue<string>("alg", out var alg) || !SupportedDPoPSigningAlgorithms.Contains(alg))
        {
            return ValidationResult.Error("Invalid 'alg' value.");
        }

        if (!token.TryGetHeaderValue<string>(JwtClaimTypes.JsonWebKey, out var jwkJson))
        {
            return ValidationResult.Error("Missing 'jwk' value.");
        }
        
        JsonWebKey jwk;
        try
        {
            jwk = new JsonWebKey(jwkJson);
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Error parsing DPoP jwk value: {error}", ex.Message);
            return ValidationResult.Error("Invalid 'jwk' value.");
        }

        if (jwk.HasPrivateKey)
        {
            return ValidationResult.Error("'jwk' value contains a private key.");
        }

        data.JsonWebKey = jwk;
        return ValidationResult.Success();
    }

    private ValidationResult ValidateSignature(DPoPProofValidationData data)
    {
        TokenValidationResult tokenValidationResult;
        try
        {
            var tvp = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                IssuerSigningKey = data.JsonWebKey,
            };

            var handler = new JsonWebTokenHandler();
            tokenValidationResult = handler.ValidateToken(data.ProofToken, tvp);
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Error parsing DPoP token: {error}", ex.Message);
            return ValidationResult.Error("Invalid signature on DPoP token.");
        }

        if (tokenValidationResult.Exception != null)
        {
            _logger.LogDebug("Error parsing DPoP token: {error}", tokenValidationResult.Exception.Message);
            return ValidationResult.Error("Invalid signature on DPoP token.");
        }

        data.Payload = tokenValidationResult.Claims;
        return ValidationResult.Success();
    }

    private ValidationResult ValidatePayload(DPoPProofValidationData data)
    {
        string? accessTokenHashFromProof = null;
        if (data.Payload.TryGetValue(JwtClaimTypes.DPoPAccessTokenHash, out var ath))
        {
            accessTokenHashFromProof = ath as string;
        }

        if (string.IsNullOrEmpty(accessTokenHashFromProof))
        {
            return ValidationResult.Error("Invalid 'ath' value.");
        }

        var accessTokenHash = HashAccessToken(data.AccessToken);
        if (accessTokenHash != accessTokenHashFromProof)
        {
            return ValidationResult.Error("Invalid 'ath' value.");
        }

        if (data.Payload.TryGetValue(JwtClaimTypes.JwtId, out var jti))
        {
            data.TokenId = jti as string;
        }

        if (string.IsNullOrEmpty(data.TokenId))
        {
            return ValidationResult.Error("Invalid or missing 'jti' value.");
        }

        if (!data.Payload.TryGetValue(JwtClaimTypes.DPoPHttpMethod, out var htm) || !data.HttpMethod.Equals(htm))
        {
            return ValidationResult.Error("Invalid 'htm' value.");
        }

        if (!data.Payload.TryGetValue(JwtClaimTypes.DPoPHttpUrl, out var htu) || !data.Url.Equals(htu))
        {
            return ValidationResult.Error("Invalid 'htu' value.");
        }

        long? issuedAtTime = null;
        if (data.Payload.TryGetValue(JwtClaimTypes.IssuedAt, out var iat))
        {
            issuedAtTime = iat switch
            {
                int intValue => intValue,
                long longValue => longValue,
                _ => issuedAtTime
            };
        }

        if (!issuedAtTime.HasValue)
        {
            return ValidationResult.Error("Missing 'iat' value.");
        }

        // This validates the 'iat' (issued at) value for the DPoP proof.
        // Another way of validating the time origin of the proof is to use a nonce,
        // as described in https://www.ietf.org/archive/id/draft-ietf-oauth-dpop-16.html#name-resource-server-provided-no
        if (IsExpired(issuedAtTime.Value))
        {
            return ValidationResult.Error("Invalid 'iat' value.");
        }
        
        return ValidationResult.Success();
    }

    private static string HashAccessToken(string accessToken)
    {
        using var sha = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(accessToken);
        var hash = sha.ComputeHash(bytes);

        return Base64Url.Encode(hash);
    }

    private async Task<ValidationResult> ValidateReplayAsync(DPoPProofValidationData data)
    {
        if (await _replayCache.ExistsAsync(ReplayCachePurpose, data.TokenId!))
        {
            return ValidationResult.Error("Detected DPoP proof token replay.");
        }

        // The client clock skew is doubled because the clock may be either before or after the correct time 
        // Cache duration is then set longer than the likelihood of proof token expiration:
        var skew = ClientClockSkew *= 2;
        var cacheDuration = ProofTokenValidityDuration + skew;
        await _replayCache.AddAsync(ReplayCachePurpose, data.TokenId!, DateTimeOffset.UtcNow.Add(cacheDuration));

        return ValidationResult.Success();
    }

    private bool IsExpired(long issuedAtTime)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var start = now + (int)ClientClockSkew.TotalSeconds;
        if (start < issuedAtTime)
        {
            var diff = issuedAtTime - now;
            _logger.LogDebug("Expiration check failed. Creation time was too far in the future. The time being checked was {iat}, and the clock is now {now}. The time difference is {diff} seconds", issuedAtTime, now, diff);
            return true;
        }

        var expiration = issuedAtTime + (int)ProofTokenValidityDuration.TotalSeconds;
        var end = now - (int)ClientClockSkew.TotalSeconds;
        if (expiration < end)
        {
            var diff = now - expiration;
            _logger.LogDebug("Expiration check failed. Expiration has already happened. The expiration was at {exp}, and the clock is now {now}. The time difference is {diff} seconds", expiration, now, diff);
            return true;
        }

        return false;
    }
}