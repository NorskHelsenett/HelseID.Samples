package no.example.helseid.sampleapi;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.security.oauth2.core.OAuth2AuthenticationException;

import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

public class DPoPTokenResolverTests {

    private final MockNimbusDPoPValidator nimbusDPoPValidator = new MockNimbusDPoPValidator();
    private final DPoPTokenResolver resolver = new DPoPTokenResolver(nimbusDPoPValidator);

    // This sets the characters for the regex:
    private String dPoPToken = "eyJhbabxyzABXYZ0189-._~+/";
    private String dPoPProof = "eyJhbabxyzABXYZ0189-._~+/";
    private final MockHttpServletRequest servletRequest = new MockHttpServletRequest();

    @BeforeEach
    void setUp() {
        servletRequest.addHeader("Authorization", "DPoP " + dPoPToken);
        servletRequest.addHeader("DPoP", "DPoP " + dPoPProof);
    }

    @Test
    void resolve_returns_an_authorization_header_token() {
        var authorizationHeaderToken = resolver.resolve(servletRequest);

        assert authorizationHeaderToken.equals(dPoPToken);
    }

    @Test
    void resolve_returns_null_when_the_header_does_not_contain_a_DPoP_token() {
        servletRequest.removeHeader("Authorization");
        servletRequest.addHeader("Authorization", "Bearer " + dPoPToken);

        var authorizationHeaderToken = resolver.resolve(servletRequest);

        assert authorizationHeaderToken == null;
    }

    @Test
    void resolve_returns_null_when_the_header_does_not_contain_a_DPoP_proof() {
        servletRequest.removeHeader("DPoP");

        var authorizationHeaderToken = resolver.resolve(servletRequest);

        assert authorizationHeaderToken == null;
    }

    @Test
    void resolve_throws_when_the_token_is_malformed() {
        servletRequest.removeHeader("Authorization");
        servletRequest.addHeader("Authorization", "DPoP $%#");

        var exception = assertThrows(OAuth2AuthenticationException.class, () -> {
            resolver.resolve(servletRequest);
        });

        assert exception.getMessage().contains("The DPoP token is malformed");
    }

    @Test
    void resolve_returns_null_when_the_dpop_validator_sets_false() {
        nimbusDPoPValidator.setValidation(false);

        var authorizationHeaderToken = resolver.resolve(servletRequest);

        assert authorizationHeaderToken == null;
    }
}
