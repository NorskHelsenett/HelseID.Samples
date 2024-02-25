## Login client for Node.js

This is a small example for Node.js that uses [Express](https://expressjs.com/) as a web host, [Passport](https://www.passportjs.org/) as authentication middleware, and [openid-client](https://www.npmjs.com/package/openid-client?activeTab=readme) as an authentication strategy within Passport. The sample is built upon [this blogpost](https://medium.com/@nitesh_17214/how-to-create-oidc-client-in-nodejs-b8ea779e0c64).

The purpose of this example is to demonstrate how user login can be acheived with HelseID. The Authorization Code Flow is applied for this purpose. We advise you to read the documentation on [openid-client](https://www.npmjs.com/package/openid-client?activeTab=readme) (under 'Quick start') on this subject.

### User login
When you start the sample with npm start on the command line, you'll need to connect a web browser to the address that shows up in the console:

```
Now listening on: https://localhost:5040
```

the default is https://localhost:5040, but you can change the port number in the `index.js` file. 

Click "Login", and use the "Test IDP". Then log in as a well known test person. You should be redirected back to the application at https://localhost:5040.

### Requirements
Node.js is required to build the program.

To run the sample, enter the following on the command line inside this folder:
```
npm install
npm start
```
