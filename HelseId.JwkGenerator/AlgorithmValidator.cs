using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace HelseId.JwkGenerator;

internal static class AlgorithmValidator
{
    private static HashSet<string> _helseIdApprovedRsaAlgorithms = new HashSet<string>(
    [
        SecurityAlgorithms.RsaSha256,
        SecurityAlgorithms.RsaSha384,
        SecurityAlgorithms.RsaSha512,
        SecurityAlgorithms.RsaSsaPssSha256,
        SecurityAlgorithms.RsaSsaPssSha384,
        SecurityAlgorithms.RsaSsaPssSha512,
    ]);

    private static HashSet<string> _helseIdApprovedEcAlgorithms = new HashSet<string>(
    [
        SecurityAlgorithms.EcdsaSha256,
        SecurityAlgorithms.EcdsaSha384,
        SecurityAlgorithms.EcdsaSha512,
    ]);

    public static bool IsValidAlgorithm(string algorithm, KeyType keyType) =>
        keyType == KeyType.Rsa
        ? _helseIdApprovedRsaAlgorithms.Contains(algorithm)
        : _helseIdApprovedEcAlgorithms.Contains(algorithm);
}