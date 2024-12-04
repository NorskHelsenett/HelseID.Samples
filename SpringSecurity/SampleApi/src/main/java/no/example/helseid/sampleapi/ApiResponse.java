package no.example.helseid.sampleapi;

public record ApiResponse(
        String greeting,
        String parentOrganizationNumber,
        String childOrganizationNumber,
        String supplierOrganizationNumber) {}
