# HelseId.JwkGenerator

## Introduction

This is a command line program to generate a key pair as a JWK (JSON Web Key). It uses `System.Security.Cryptography` to generate a 4096 bit RSA key or a P-521 ECDSA key. The key pair is saved to two json files.

### Requirements

The [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) is required to build the program.

## Usage

```
dotnet run -- --key-type rsa|ec --prefix <file-prefix>
```

Replace `<file-prefix>` with a descriptive name of your key. It will be part of the filenames. For example, `dotnet run -- --key-type ec --prefix test` will create two files: 'test_jwk.json' and 'test_jwk_pub.json'. The first file contains the key pair (including the private key), while the second file only contains the public key.

## Build exe

```
./publish.ps1
```

This outputs a win64 exe file in the publish folder which bundles all dependencies, including the .NET runtime.
