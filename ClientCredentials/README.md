## Client Credentials Grant

The [client credentials grant](https://www.rfc-editor.org/rfc/rfc6749#section-4.4) type is commonly used for server-to-server interactions that 
runs in the background, without immediate interaction with a user. 

The client credentials grant type MUST only be used by confidential clients (e.g., client implemented on a secure server with
restricted access to the client credentials).
```
     +------------+                                  +---------------+
     |            |                                  |               |
     |   Client   |>--(A)- Client Authentication --->| Authorization |
     |  (Example  |                                  |     Server    |
     |    code)   |<--(B)---- Access Token ---------<|   (HelseID)   |
     |            |                                  |               |
     +------------+                                  +---------------+
```
This sample is created as a command-line application. It demonstrates how to generate a signed JSON Web Token (JWT) which then is used to request access to a HelseID. The code for this functionality can be found in the `TokenRequests` folder.

When HelseID approves the request, an access token is returned. The application then uses the access token to access
the [Sample API](../../SampleAPI/README.md).

By using options on the command line, you can instruct the application to
  * Make use of a child organization number (underenhet) to get a claim in the access token
  * Use the multi-tenant pattern to send an organization number (for the tenant) to HelseID
  
### A short description of the folder structure in this project
* The main folder contains
  * The `Program` class that configures the machine-to-machine client
* **Client**: this folder contains the application proper; the `Machine2MachineClient`, which orchestrates the actual work
* **Configuration**: this folder contains code that creates and configures `Machine2MachineClient`


### Requirements

The [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is required to build the program.

First, you will need to start the [Sample API project](../SampleAPI/README.md) in a separate terminal. Then, you can run this application, either with or without a request for a child organization number. 

#### To run the default sample, enter the following on the command line inside this folder
```
dotnet run
```

#### To run the sample with the use of a child organization number
This makes use of a child organization number. The organization must be whitelisted in HelseID.
```
dotnet run --use-child-org-number
```

#### To run the sample as an app that uses multi-tenancy

This option makes use of both parent and child units in the call to the token endpoint 
````
dotnet run --use-multi-tenant-pattern
````

#### To list all options
```
dotnet run -- --help
```

