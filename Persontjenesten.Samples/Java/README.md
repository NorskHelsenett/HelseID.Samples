# Persontjenesten api client sample

This directory contains two projects that together form a working sample for connecting to the Persontjenesten API,
using HelseID for authentication and authorization.

## Running the sample locally

First, install the generated api client package located in the [client](client) folder to your local maven registry:

```shell
cd client
mvn clean install
```

Once this package is installed, the api client project in the [demo](demo) folder should compile and run.

## HelseIDs test environment and private keys

This repository contains a private key which can be used to sign token requests in HelseIDs test environment,
in order for the sample to work out of the box. In the test environment for HelseID, it's a deliberate strategy
that private keys are considered harmless to keep in source control.

**Never keep private keys in source control in a production environment**.

In a real world application, the private key should be kept secret and loaded into the application by a secret provider
on demand.

