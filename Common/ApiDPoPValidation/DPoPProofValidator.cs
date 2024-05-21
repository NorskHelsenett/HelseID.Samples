using System.Linq;
using IdentityModel;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.ApiDPoPValidation;

public class DPoPProofValidator
{
    // This is the number of seconds that we allow for the DPoP proof to expire
    private TimeSpan ProofTokenValidityDuration { get;} = TimeSpan.FromSeconds(1);
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

    private readonly IReplayCache _replayCache;

    public DPoPProofValidator(IReplayCache replayCache)
    {
        _replayCache = replayCache;
    }

    public async Task<ValidationResult> Validate(DPoPProofValidationData data)
    {
        var validationResult = ValidateCnfClaimFromAccessToken(data);
        if (validationResult.IsError)
        {
            return validationResult;
        }

        validationResult = ValidateHeader(data);
        if (validationResult.IsError)
        {
            return validationResult;
        }

        validationResult = await ValidateSignature(data);
        if (validationResult.IsError)
        {
            return validationResult;
        }

        validationResult = ValidatePayload(data);
        if (validationResult.IsError)
        {
            return validationResult;
        }

        // Check for a replayed DPoP proof
        validationResult = await ValidateReplayAsync(data);

        return validationResult;
    }

    private static ValidationResult ValidateCnfClaimFromAccessToken(DPoPProofValidationData data)
    {
        if (string.IsNullOrEmpty(data.CnfClaimValueFromAccessToken))
        {
            return ValidationResult.Error("Missing 'cnf' claim in access token");
        }

        string? jktContent;
        try
        {
            var jktValue = JsonSerializer.Deserialize<Dictionary<string, string>>(data.CnfClaimValueFromAccessToken);
            if (jktValue == null || !jktValue.TryGetValue(JwtClaimTypes.ConfirmationMethods.JwkThumbprint, out jktContent))
            {
                return ValidationResult.Error("Missing 'jkt' value in 'cnf' claim in access token");
            }
        }
        catch (JsonException)
        {
            return ValidationResult.Error("Invalid 'jkt' value in 'cnf' claim in access token");
        }

        data.JktClaimValueFromAccessToken = jktContent;
        return ValidationResult.Success();
    }

    private static ValidationResult ValidateHeader(DPoPProofValidationData data)
    {
        JsonWebToken token;
        try
        {
            var handler = new JsonWebTokenHandler();
            token = handler.ReadJsonWebToken(data.ProofToken);
        }
        catch (Exception)
        {
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

        if (!token.TryGetHeaderValue<JsonElement>(JwtClaimTypes.JsonWebKey, out var jwkValues))
        {
            return ValidationResult.Error("Invalid 'jwk' value.");
        }

        var jwkJson = JsonSerializer.Serialize(jwkValues);

        JsonWebKey jwk;
        try
        {
            jwk = new JsonWebKey(jwkJson);
        }
        catch (Exception)
        {
            return ValidationResult.Error("Invalid 'jwk' value.");
        }

        if (jwk.HasPrivateKey)
        {
            return ValidationResult.Error("'jwk' value contains a private key.");
        }

        if (data.JktClaimValueFromAccessToken != jwk.CreateThumbprint())
        {
            return ValidationResult.Error("Failed to validate the 'cnf' claim value against the DPoP proof's public key");
        }

        // Used for signature validation
        data.JsonWebKey = jwk;
        return ValidationResult.Success();
    }

    private static async Task<ValidationResult> ValidateSignature(DPoPProofValidationData data)
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
            tokenValidationResult = await handler.ValidateTokenAsync(data.ProofToken, tvp);
        }
        catch (Exception)
        {
            return ValidationResult.Error("Invalid signature on DPoP token.");
        }

        if (tokenValidationResult.Exception != null)
        {
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

        if (data.AccessTokenHash != accessTokenHashFromProof)
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

        // This validates the 'iat' (issued at time) value for the DPoP proof.
        // Another way of validating the time origin of the proof is to use a nonce,
        // as described in https://www.ietf.org/archive/id/draft-ietf-oauth-dpop-16.html#name-resource-server-provided-no
        if (IssuedAtTimeIsInvalid(issuedAtTime.Value))
        {
            return ValidationResult.Error("Invalid 'iat' value.");
        }

        return ValidationResult.Success();
    }

    private bool IssuedAtTimeIsInvalid(long issuedAtTime)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var start = now + (int)ClientClockSkew.TotalSeconds;
        // The 'issued at' time is in the future
        if (start < issuedAtTime)
        {
            return true;
        }

        var expiration = issuedAtTime + (int)ProofTokenValidityDuration.TotalSeconds;
        var end = now - (int)ClientClockSkew.TotalSeconds;
        // The DPoP proof has expired
        if (expiration < end)
        {
            return true;
        }

        return false;
    }

    private async Task<ValidationResult> ValidateReplayAsync(DPoPProofValidationData data)
    {
        if (await _replayCache.ExistsAsync(ReplayCachePurpose, data.TokenId!))
        {
            return ValidationResult.Error("Detected a DPoP proof token replay.");
        }

        // The client clock skew is doubled because the clock may be either before or after the correct time
        // Cache duration is then set longer than the likelihood of proof token expiration:
        var skew = ClientClockSkew;
        skew *= 2;
        var cacheDuration = ProofTokenValidityDuration + skew;
        await _replayCache.AddAsync(ReplayCachePurpose, data.TokenId!, DateTimeOffset.UtcNow.Add(cacheDuration));

        return ValidationResult.Success();
    }
}
