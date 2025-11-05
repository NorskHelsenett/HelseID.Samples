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
This sample is created as a command-line application. It demonstrates how to generate an Access Token by using the [HelseID library (Client Credentials)](https://github.com/NorskHelsenett/HelseID.Library). 

When HelseID approves the request, an access token is returned. The application then uses the access token to access
the [Sample API](../../SampleAPI/README.md).

### A short description of the folder structure in this project
* The main folder contains
  * The `Program` class that configures the machine-to-machine client
* **Client**: this folder contains the application proper; the `Machine2MachineClient`, which orchestrates the actual work
* **Configuration**: this folder contains code that creates and configures `Machine2MachineClient`


### Requirements

The [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) is required to build the program.

First, you will need to start the [Sample API project](../SampleAPI/README.md) in a separate terminal. Then, you can run this application, either with or without a request for a child organization number. 

#### To run the default sample, enter the following on the command line inside this folder
```
dotnet run
```
