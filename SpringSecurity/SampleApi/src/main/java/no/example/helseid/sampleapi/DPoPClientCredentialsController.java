package no.example.helseid.sampleapi;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.concurrent.atomic.AtomicLong;

@RestController
public class DPoPClientCredentialsController {
    private static final String template = "Hello, %s!";
    private final AtomicLong counter = new AtomicLong();

    @GetMapping("/machine-clients/greetings")
    public ApiResponse greeting(@RequestParam(value = "name", defaultValue = "World") String name) {
        return CreateResult("Greetings from endpoint with DPoP");
    }

    private ApiResponse CreateResult(String greeting) {
        return new ApiResponse(greeting, "888", "7777", "");
    }
}
