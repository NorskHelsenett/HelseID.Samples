package no.example.helseid.sampleapi;

import jakarta.servlet.http.HttpServletRequest;

public class MockNimbusDPoPValidator implements NimbusDPoPValidator {

    private boolean validation = true;

    public void setValidation(boolean validation) {
        this.validation = validation;
    }

    @Override
    public boolean validateTokenWithProof(HttpServletRequest request)  {
        return validation;
    }
}
