﻿namespace Blazor.WASM.Api.Access.Server
{
    public class ClientAssertionConfiguration
    {
        public string? ClientId { get; set; }
        public string? ClientSecretJwk { get; set; }
        public string? HelseIdAuthorithy { get; set; }
    }
}