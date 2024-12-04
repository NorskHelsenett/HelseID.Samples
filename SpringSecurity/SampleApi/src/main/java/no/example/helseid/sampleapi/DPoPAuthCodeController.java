package no.example.helseid.sampleapi;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.concurrent.atomic.AtomicLong;

@RestController
public class DPoPAuthCodeController {
    // brew install mkcert
    // mkcert -install
    // mkcert -pkcs12 localhost

    private static final String template = "Hello, %s!";
    private final AtomicLong counter = new AtomicLong();

    @GetMapping("/user-login-clients/greetings")
    public ApiResponse greeting(@RequestParam(value = "name", defaultValue = "World") String name) {
        return CreateResult();
    }

    private ApiResponse CreateResult() {
        return new ApiResponse("Greetings", "888", "7777", "");
    }
}
