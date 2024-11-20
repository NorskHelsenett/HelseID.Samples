package no.example.helseid.sampleapi;

import java.net.URISyntaxException;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import jakarta.servlet.http.HttpServletRequest;

import org.springframework.http.HttpHeaders;
import org.springframework.security.oauth2.core.OAuth2AuthenticationException;
import org.springframework.security.oauth2.server.resource.BearerTokenError;
import org.springframework.security.oauth2.server.resource.BearerTokenErrors;
import org.springframework.security.oauth2.server.resource.web.BearerTokenResolver;
import org.springframework.util.StringUtils;

public final class DPoPTokenResolver implements BearerTokenResolver {

    public static final String DPOPHEADER = "DPoP";

    private static final Pattern authorizationPattern = Pattern.compile("^DPoP (?<token>[a-zA-Z0-9-._~+/]+=*)$",
            Pattern.CASE_INSENSITIVE);
    private final NimbusDPoPValidator nimbusDPoPValidator;

    DPoPTokenResolver(NimbusDPoPValidator nimbusDPoPValidator) {
        this.nimbusDPoPValidator = nimbusDPoPValidator;
    }

    @Override
    public String resolve(final HttpServletRequest request) {
        final String authorizationHeaderToken = resolveFromAuthorizationHeader(request);
        if (authorizationHeaderToken == null) {
            return null;
        }

        var dpopHeader = request.getHeader(DPOPHEADER);
        if (dpopHeader == null) {
            // TODO: debug
            return null;
        }

        boolean dpopValidation = false;
        try {
            dpopValidation = this.nimbusDPoPValidator.validateTokenWithProof(request);
        } catch (URISyntaxException e) {
            // TODO: debug
            return null;
        }
        if (dpopValidation == false) {
            return null;
        }
        return authorizationHeaderToken;
    }

    private String resolveFromAuthorizationHeader(HttpServletRequest request) {
        String authorizationHeader = request.getHeader(HttpHeaders.AUTHORIZATION);
        if (!StringUtils.startsWithIgnoreCase(authorizationHeader, "dpop")) {
            return null;
        }
        Matcher matcher = authorizationPattern.matcher(authorizationHeader);
        if (!matcher.matches()) {
            BearerTokenError error = BearerTokenErrors.invalidToken("The DPoP token is malformed");
            throw new OAuth2AuthenticationException(error);
        }
        return matcher.group("token");
    }
}
