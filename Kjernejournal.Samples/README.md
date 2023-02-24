## Integration with Kjernejournal (KJ)

This folder contains example code showing how one can use HelseID to integrate with Kjernejournal (KJ)


### Single-sign-on (SSO)
*This is still work in progress and the APIs might change substantially in the future.*

SSOIntegration.java shows a simple example of the code needed to get started with KJ integration using the EPJ's own access tokens.

With this mechanism in place, once the user is logged into the EPJ-system with HelseID, 
they will no longer need to log in to KJ, and all access to services within KJ will be based on the EPJ's own authorization token.
 

### Requirements
The example project is set up with maven and java 19
