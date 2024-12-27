package no.example.helseid.sampleapi;

import com.nimbusds.jose.JOSEException;
import com.nimbusds.jose.JWSAlgorithm;
import com.nimbusds.jose.JWSObject;
import com.nimbusds.jose.shaded.gson.internal.LinkedTreeMap;
import com.nimbusds.jose.util.Base64URL;
import com.nimbusds.jwt.JWTClaimsSet;
import com.nimbusds.jwt.SignedJWT;
import com.nimbusds.oauth2.sdk.dpop.JWKThumbprintConfirmation;
import com.nimbusds.oauth2.sdk.dpop.verifiers.*;
import com.nimbusds.oauth2.sdk.token.DPoPAccessToken;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.http.HttpHeaders;

import java.net.URI;
import java.net.URISyntaxException;
import java.text.ParseException;
import java.util.Arrays;
import java.util.HashSet;
import java.util.Set;

import static no.example.helseid.sampleapi.DPoPTokenResolver.DPOPHEADER;

public class DefaultNimbusDPoPValidator implements NimbusDPoPValidator {

    // The max accepted age of the DPoP proof JWTs
    private final long proofMaxAgeSeconds = 10;

    // DPoP single use checker, caches the DPoP proof JWT jti claims
    private final long cachePurgeIntervalSeconds = 600;

    // Supported algorithms as described in https://utviklerportal.nhn.no/informasjonstjenester/helseid/protokoller-og-sikkerhetsprofil/sikkerhetsprofil/docs/vedlegg/krav_til_kryptografi_enmd/
    private final Set<JWSAlgorithm> supportedAlgorithms = new HashSet<>(
            Arrays.asList(
                    JWSAlgorithm.RS256,
                    JWSAlgorithm.RS384,
                    JWSAlgorithm.RS512,
                    JWSAlgorithm.ES256,
                    JWSAlgorithm.ES384,
                    JWSAlgorithm.ES512,
                    JWSAlgorithm.PS256,
                    JWSAlgorithm.PS384,
                    JWSAlgorithm.PS512));

    private DPoPProtectedResourceRequestVerifier dPoPProofVerifier;

    DefaultNimbusDPoPValidator() {
        var singleUseChecker = new DefaultDPoPSingleUseChecker(
                proofMaxAgeSeconds,
                cachePurgeIntervalSeconds);

        // Create the DPoP proof and access token binding verifier,
        // the class is thread-safe
        dPoPProofVerifier = new DPoPProtectedResourceRequestVerifier(
                supportedAlgorithms,
                proofMaxAgeSeconds,
                singleUseChecker);
    }

    @Override
    public boolean validateTokenWithProof(HttpServletRequest request) throws URISyntaxException {

        try {
            var  httpMethod = request.getMethod();
            URI httpURI = new URI(request.getRequestURL().toString());

            // The DPoP proof, obtained from the HTTP DPoP header
            var dpopProof = request.getHeader(DPOPHEADER);
            var dpopToken = request.getHeader(HttpHeaders.AUTHORIZATION);

            SignedJWT dPoPProof = SignedJWT.parse(dpopProof);
            DPoPAccessToken dPoPAccessToken = DPoPAccessToken.parse(dpopToken);

            var jwsObject = JWSObject.parse(dPoPAccessToken.toString());
            var claims = JWTClaimsSet.parse(jwsObject.getPayload().toJSONObject());

            var clientID = (String)claims.getClaim("client_id");
            // The DPoP proof issuer, typically the client ID obtained from the
            // access token introspection
            DPoPIssuer dPoPIssuer = new DPoPIssuer(clientID);

            // The JWK SHA-256 thumbprint confirmation, obtained from the
            // access token introspection
            var cnfClaim = (LinkedTreeMap)claims.getClaim("cnf");
            var jktClaim = cnfClaim.get("jkt");
            Base64URL base64URL = new Base64URL(jktClaim.toString());
            JWKThumbprintConfirmation cnf = new JWKThumbprintConfirmation(base64URL);

            dPoPProofVerifier.verify(httpMethod, httpURI, dPoPIssuer, dPoPProof, dPoPAccessToken, cnf, null);

        } catch (ParseException e) {
            System.err.println("Invalid parsing of token: " + e.getMessage());
            return false;
        } catch (com.nimbusds.oauth2.sdk.ParseException e) {
            System.err.println("Invalid parsing of OAuth 2.0 token: " + e.getMessage());
            return false;
        } catch (InvalidDPoPProofException e) {
            System.err.println("Invalid DPoP proof: " + e.getMessage());
            return false;
        } catch (AccessTokenValidationException e) {
            System.err.println("Invalid access token binding: " + e.getMessage());
            return false;
        } catch (JOSEException e) {
            System.err.println("Internal error: " + e.getMessage());
            return false;
        }

        return true;
    }
}
