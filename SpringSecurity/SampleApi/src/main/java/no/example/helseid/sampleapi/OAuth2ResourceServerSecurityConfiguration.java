package no.example.helseid.sampleapi;

import com.nimbusds.jose.JOSEObjectType;
import com.nimbusds.jose.JWSAlgorithm;
import com.nimbusds.jose.jwk.source.JWKSource;
import com.nimbusds.jose.jwk.source.JWKSourceBuilder;
import com.nimbusds.jose.proc.*;
import com.nimbusds.jwt.JWTClaimNames;
import com.nimbusds.jwt.JWTClaimsSet;
import com.nimbusds.jwt.proc.DefaultJWTClaimsVerifier;
import com.nimbusds.jwt.proc.DefaultJWTProcessor;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.Customizer;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.oauth2.jwt.JwtDecoder;
import org.springframework.security.oauth2.jwt.NimbusJwtDecoder;
import org.springframework.security.oauth2.server.resource.web.BearerTokenResolver;
import org.springframework.security.web.SecurityFilterChain;

import java.net.MalformedURLException;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.URL;
import java.util.Arrays;
import java.util.HashSet;

import static org.springframework.security.oauth2.core.authorization.OAuth2AuthorizationManagers.hasScope;


@EnableWebSecurity
@Configuration
public class OAuth2ResourceServerSecurityConfiguration  {

    @Bean
    public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        http
                .authorizeHttpRequests(authorize -> authorize
                        .requestMatchers("/user-login-clients/greetings/**").access(hasScope("nhn:test-api/message:read:dpop"))
                        .requestMatchers("/machine-clients/greetings/**").access(hasScope("nhn:test-api/message:read:dpop"))
                        .anyRequest().authenticated()
                )
                .oauth2ResourceServer((oauth2) -> oauth2.jwt(Customizer.withDefaults()));
        return http.build();
    }

    @Bean
    JwtDecoder jwtDecoder() {
        try {
            return new NimbusJwtDecoder(defaultJWTProcessor());
        } catch (URISyntaxException | MalformedURLException e) {
            System.out.println("A configuration error occurred: " + e.getMessage());
            return null;
        }
    }

    @Bean
    BearerTokenResolver defaultBearerTokenResolver() {
        return new DPoPTokenResolver(new DefaultNimbusDPoPValidator());
    }

    @Bean
    public DefaultJWTProcessor<SecurityContext> defaultJWTProcessor() throws URISyntaxException, MalformedURLException {
        DefaultJWTProcessor<SecurityContext> defaultJWTProcessor = new DefaultJWTProcessor<>();

        defaultJWTProcessor.setJWSTypeVerifier(getDefaultJOSEObjectTypeVerifier());
        defaultJWTProcessor.setJWETypeVerifier(getDefaultJOSEObjectTypeVerifier());
        defaultJWTProcessor.setJWSKeySelector(getDefaultJWSKeySelector());
        defaultJWTProcessor.setJWTClaimsSetVerifier(getDefaultJWTClaimsVerifier());

        return defaultJWTProcessor;
    }

    @Bean
    public JWSKeySelector<SecurityContext> getDefaultJWSKeySelector() throws URISyntaxException, MalformedURLException {
        URL uri = new URI("https://helseid-sts.test.nhn.no/.well-known/openid-configuration/jwks").toURL();

        JWKSource<SecurityContext> keySource = JWKSourceBuilder
                .create(uri)
                .retrying(true)
                .build();

        JWSAlgorithm expectedJWSAlg = JWSAlgorithm.RS256;

        return new JWSVerificationKeySelector<>(expectedJWSAlg, keySource);
    }

    @Bean
    public DefaultJWTClaimsVerifier<SecurityContext> getDefaultJWTClaimsVerifier() {
        return new DefaultJWTClaimsVerifier<>(
                "nhn:test-api",
                new JWTClaimsSet.Builder().issuer("https://helseid-sts.test.nhn.no").build(),
                new HashSet<>(Arrays.asList(
                        JWTClaimNames.ISSUED_AT,
                        JWTClaimNames.EXPIRATION_TIME,
                        JWTClaimNames.JWT_ID)
                )
        );
    }

    @Bean
    public JOSEObjectTypeVerifier<SecurityContext> getDefaultJOSEObjectTypeVerifier() {
        return new DefaultJOSEObjectTypeVerifier<SecurityContext>(new JOSEObjectType("at+jwt"));
    }
}
