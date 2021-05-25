# HelseId.RsaJwk

## Introduction

This is a command line program to generate an RSA key pair as a JWK (JSON Web Key). It uses `System.Security.Cryptography.RSA` to generate the key pair with a size of 4096 bits. The key pair is saved to two json files.

## Requirements

.NET 5 (or later) SDK is required to build the program.

## Usage

```
dotnet run -- keyname
```

Replace `keyname` with a descriptive name of your key. It will be part of the filenames. For example, `dotnet run -- test` will create two files: 'test_jwk.json' and 'test_jwk_pub.json'. The first file contains the whole key pair (including the private key), while the second file only contains the public key.

## Build exe

```
./publish.ps1
```

This outputs a win64 exe file in the publish folder which bundles all dependencies, including the .NET runtime.