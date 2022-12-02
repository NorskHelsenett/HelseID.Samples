package sf.nhn.helseid.demo.token;

import com.google.gson.Gson;
import com.nimbusds.jose.JOSEException;
import com.nimbusds.jose.JWSAlgorithm;
import com.nimbusds.jose.JWSHeader;
import com.nimbusds.jose.crypto.RSASSASigner;
import com.nimbusds.jose.jwk.JWK;
import com.nimbusds.jwt.JWTClaimsSet;
import com.nimbusds.jwt.SignedJWT;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.URLEncoder;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.text.ParseException;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

public class HelseIdTokenExchanger implements TokenExchanger {

    private final String clientId;
    private final HttpClient httpClient;
    private final Gson gson;
    private final String tokenEndpoint;
    private final String scope;
    private final String jwkPrivateKey;

    public HelseIdTokenExchanger(
            String clientId,
            String tokenEndpoint,
            String scope,
            String jwkPrivateKey,
            HttpClient httpClient,
            Gson gson) {
        this.tokenEndpoint = tokenEndpoint;
        this.scope = scope;
        this.jwkPrivateKey = jwkPrivateKey;
        this.clientId = clientId;
        this.httpClient = httpClient;
        this.gson = gson;
    }

    @Override
    public TokenResponse getAccessToken() {
        try {
            // POST parameters for the token request
            var values = new HashMap<String, String>();
            values.put("grant_type", "client_credentials");
            values.put("scope", scope);
            values.put("client_assertion", createClientAssertionToken());
            values.put("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer");
            var body = HttpRequest.BodyPublishers.ofString(getParamsString(values));

            HttpRequest request = HttpRequest.newBuilder()
                    .uri(new URI(tokenEndpoint))
                    .header("Content-Type", "application/x-www-form-urlencoded")
                    .POST(body)
                    .build();

            var response = httpClient.send(request, HttpResponse.BodyHandlers.ofString()).body();
            return gson.fromJson(response, TokenResponse.class);

        } catch (URISyntaxException | IOException | InterruptedException | JOSEException
                 | ParseException e) {
            throw new RuntimeException("Failed to retrieve access token", e);
        }
    }

    private String createClientAssertionToken() throws JOSEException, ParseException, IOException {
        var now = new Date();
        var expires = new Date(now.getTime() + 60 * 1000);
        var jti = UUID.randomUUID().toString();

        var claims = new JWTClaimsSet.Builder()
                .issuer(clientId)
                .subject(clientId)
                .notBeforeTime(now)
                .issueTime(now)
                .expirationTime(expires)
                .jwtID(jti)
                .audience(tokenEndpoint)
                .build();

        var jwk = JWK.parse(jwkPrivateKey);
        var signer = new RSASSASigner(jwk.toRSAKey());

        var jwt = new SignedJWT(
                new JWSHeader.Builder(JWSAlgorithm.PS256).keyID(jwk.getKeyID()).build(),
                claims
        );

        jwt.sign(signer);

        return jwt.serialize();
    }

    private static String getParamsString(Map<String, String> params) throws UnsupportedEncodingException {
        var result = new StringBuilder();
        for (var entry : params.entrySet()) {
            result.append(URLEncoder.encode(entry.getKey(), StandardCharsets.UTF_8));
            result.append("=");
            result.append(URLEncoder.encode(entry.getValue(), StandardCharsets.UTF_8));
            result.append("&");
        }

        String resultString = result.toString();
        return resultString.length() > 0
                ? resultString.substring(0, resultString.length() - 1)
                : resultString;
    }
}
