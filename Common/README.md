## Common

This folder contains common code for several of the sample code projects.


### A short description of the folder structure in this project
* **ApiConsumers**: a small class that demonstrates the use of the sample API by using a bearer (access) token
* **ApiDPoPValidation** extension methods that lets the ASP.NET Core Authorization library validate DPoP proofs
* **ClientAssertions**: this folder contains the code that builds the [client assertions](https://www.rfc-editor.org/rfc/rfc7523#section-2.2) that are used to authenticate the client against HelseID. This is a required feature of HelseID.
* **Configuration**: this folder contains a simple configuration class that is used throughout the sample code
* **Endpoints**: this folder contains the code that uses metadata on HelseID to find the endpoints on the HelseID server
* **Interfaces**: this folder contains several interfaces that have their implementations elsewhere in the code
* **JwtTokens**: this folder contains the code that creates JWT tokens that are used either for token requests or for request objects
* **Models**: this folder contains data models that are used elsewhere in the code
* **PayloadClaimsCreators**: this folder contains the code that creates specialized claims for the payload that is used for token requests or for request objects
* **TokenExpiration**: this folder contains code for calculating the expiration time for a token, based on an 'expires in' value
* **TokenRequests**: this folder contains the code that can create a *client assertion* and request an access token from HelseID by using various grants


### Requirements

The [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is required to build the project.

