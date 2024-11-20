package no.example.helseid.sampleapi;

import jakarta.servlet.http.HttpServletRequest;

import java.net.URISyntaxException;

public interface NimbusDPoPValidator {

    boolean validateTokenWithProof(HttpServletRequest request) throws URISyntaxException;

}

