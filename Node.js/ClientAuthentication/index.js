const express             = require('express');
const session             = require('express-session');
const cookieParser        = require('cookie-parser');
const helmet              = require('helmet');
const passport            = require('passport');
const http                = require('http');
const { Issuer,Strategy, TokenSet } = require('openid-client');
const https               = require("https");
const fs                  = require('fs');

// ------------------------------------------------------------
// Configuration constants
// ------------------------------------------------------------
const LOGIN_PATH = '/login';
const LOGOUT_PATH = '/logout';
const OIDC_REDIRECT_PATH = '/signin-oidc'; // '/login/callback';
const OIDC_POST_LOGOUT_REDIRECT_PATH = '/signout-callback-oidc'; ///logout/callback';
const LOGGED_IN_USER_PATH = '/user';
const LOGIN_FAILURE_PATH = '/login-failure';
const LOGIN_IN_AT_WRONG_SECURITY_LEVEL = '/login-at-wrong-security-level';

const PASSPORT_OIDC_STRATEGY = 'oidc';
const PORT_NUMBER = 5040;
const STS_URL = 'https://helseid-sts.test.nhn.no';
const CLIENT_ID = 'helseid-sample-client-authentication';
const CLIENT_SCOPE = 'openid profile helseid://scopes/identity/pid helseid://scopes/identity/security_level';
const OIDC_REDIRECT_URI = `https://localhost:${PORT_NUMBER}${OIDC_REDIRECT_PATH}`
const OIDC_POST_LOGOUT_REDIRECT_URI = `https://localhost:${PORT_NUMBER}${OIDC_POST_LOGOUT_REDIRECT_PATH}`
const PRIVATE_KEY_RSA = {
  'd': 'dJFf2efy5al1PZPEtgbifR0E7IC1yjTZn8OcgjdoVsaJG7F7rrAUH4awxyXJaOrHfz0ZwNtp57TzKtO50Qy08PPcg8uHO4WIpvmHgS_mtNSovWVxsWVnGdq0NlgAT8EEhVT1-3acu6OvfOROM8HfWgzded8dkWPm9QMPJtd9q2u0-ZGdEDu71ttHfFHHb8BZK5C6AX-BBaEpJQ8fsmXyO8MLmXGos4Q95XX40qgsMqMrxLIYNCjcmXkdOq9gfHd1d5ub-X84u2MzVc3qXDr3F5YgCmx0i4sKddFrQ4J28rIjTWnyYPJDOzpSVCBuazVLkSY7I15CwCZnMU_1BdkEWowzULWNQVwB2vLQTgbS2uMGci2R0QbmxCqMt2teQ9W4sroHcantTJjEcPx0K4wpy0BGhHGJXcecFw78QD5qUzUyM5TyakyEIfbVicZHDPcbQyx4xxQlhNugrljD4UelRE2_In53Re1kNPBUwiC_Ql0fMlilxyU6R1lLPUoz-BnATZn9ofJB1UQqUKYUxPB763I16WLvQwIP42unKww4HPEKTPojWzwy7igFQklJ0IavdXpQtebTQeXLuyhsikX1wYe1tX6zSFRcCDUukfz3YQb-BTGpWLFcIjbqvYND08sYDGwXcOfQeHwJIy-4MHti4HpYgy5QvibrQHfGH7soxxU',
  'dp': '1NS8GseB2YzZfkOYe9aFcvqrFTbdvShYDhbd2we4ukViZaLUQ1FnfzgTzh8WDi_7Py1rwsZZHfpZjSyI81_VGP919_kcDtf74IK502nLpztoaLRgELGZbdDCadAMb4nXGMRfoFG5esl3vRQU2JQuOnglFNccBuq61d7VPe5F8J2eF-NmtxrcxZeSvSHBGEbDFCE9xz-yCwS0sbM-ymcSVP937frfXdYztaASgUL1Mvn7EoyHcXXV787uOlpJ306lp9bTOVzeRpRy_2apQoLucq5pCSKyame7oeinpVrvbfkvdTXHZkZnS1uNv3D8cKiyt8GXKG0nUw-7SYX_Rg-6aQ',
  'dq': 'V6qxnB6ZjByVVf0hDCkyliqzDSgeyDHI2QGcvx6SnK6VmyNozqtADfxm5pRz4cHrVx2_hH9xlx0FXQb3guSe4UNanC0UfDWFXbNPzcxXnR1rTz2wouI1pYWad5cgruRmbyh0pqAx1f2nCc71UhDcuKZGvPzWCZYBHlvLDIk3dRn313rfVp9a4FzpULuzrZMnPn5Hw33UFrrIiXjGUUa3wTpjn_hl0JBBPWD7DcYFu1QSIzme37WcbGJzaSqes_9sTt1uh2qKLDup92CsPFb-uAvhfdm5L7j0GogqxQXx6LKgGx7jFyb9_3-zOnGOpZQhIMUdBDUtcx7Z_WFvhEJtlQ',
  'e': 'AQAB',
  'kty': 'RSA',
  'n': 's434f6cfoN0BWRX-UGUifqb7BWtDvMucK_RmrtZ-L-AiSRi4NrQ46tHJRP-4dq2QtkRMKU6ODZpwviy7uy6f7S8dbS0sxA74lZGOn0-uS5SGI_tlyuO94dF0L6eqB4dykcYT0nfF6iMfC2aWmk8qKOduJLHIg_JnaVl9buahQfu6-2mkX8T2fanEqL6vcwLjO5i2NFHeZYSP_DLubxHCdSgAmskiC2bRoEt3IcRFBLgt0t9jT2AA-gwusJMq9Bqz1rsP6MUgUf9g63vk_C_wOiFeCoIpsmp5gowjuMVFo-mIcSYahxtV8rAfkTW3RyLDn5KExyrAKEvCssfDJObYPQes74y_3D3kSx_Qws1sLiBNqDJ0itCI8JEnJrgSggjAk0RP8iQZO495thEcNAQ4Rnj9qIGAdhgcpQlrBqeSlOUlHqij_EC-r1FUNf9tL-9fntvdU_wMRtEEE4BcJkB4p0jXox1ORxhkU1L9wTl62OYzugzib65DkzaYvxgxum8RAFX6PYyMbeIDbWdvA07sM9CKKDHrCVEcM9blmHtfK_lom7rSycDEzBBNzM-CAlcSPZrSQlL0CXwZUouo3VQSOb_LpxC6myYIjSruLpVex5sEhFFZQuqHeUWRubPAYOvnLQFAPm3um4UVNhIJZOKbaOPdXsDO9AWXmehUJBA6DR0',
  'p': '3rd9QmGqCIeXRXYgmcGUOTou_at625y9sTgEQkZrsVBIpC423WBddktSpP0OwNzV_fmrAsWYdBpSiRMr5pgxSKn5yNRggCOfXtf3lpYxJ2Jx6hwgQ5EvVRSm0YaKjXUlVsDwC3Ns9kAz0z921padtieIoQEZWgEm1b2hL73UNXKxjm94211tJDvxPQWql9zs7kcAcYcTrkZndaRGjygHcJSldO8ZXM_ao2wowD0tZZ12vVZa7u9vOREcZ_VsESuPy-zypHQvbjreSN1bIE80sB1p1cP5taWKeq3YU1Uwe6UlYxbT61LvrEwIzw7Hxv2LfPOvAf78OVIv8eDfM3vTCw',
  'q': 'zmM4B6cubaL8ZoDrgTBEj1wslMjg1AeWTscXrWnAk7yPCHOJ8RDtarvUG2X4XoPrHbaaVKGJ8jFxQlfrWCeYneYHdn9j02-ZeaIj8su5YzwqH8EGNBNtTUuSuZdZ8AusPPpjfjI2-cwR8h0pcmaUgw2WDfler8FrBvE1EHahEBHGhQAB7TD0igM3NbuVx2DXkiOzlCBdxKgY58C6qYLn_25hBZKxkQEQn4RV184Bcm8aF4opnX8JH1klDn5HdYbfTSvEhxTjwb28KTj-X1Zz5fsMW2bSTxGsWQ6iOsBC71LA6aTxSWKlxdKri3AcKzXSZVMOj3ElLFifm-eey3i5dw',
  'qi': 'I69KjDCsOXoCKjCLsev01d-FEmxefqcOu3OTOkraafqYM4GyQydhMoXDeWUf5oI49R4Bng4lLexxSMgLGw6kaiCy1X6xdVjt7XoYG697V6DBR5RpsOIjywG4UfhviNqIU8c5vOiXaqJLYPfTH4H9hUWt2SwTt1JTFE92foHzuh8ZiZ7Aqr2lavx1Gx6B8mk28eU38pzNGdkvmjR1MWWaCH9q0Ur0YMFCwVMs3xhpagGWhkZIxNBlPTcys4fFqSbVvd_vxIWO67unP1rWgjMCv8G0o1hg0Z3giTfVzjB3fnNoDW0Ohpn5ovGt7_y4DTuIRICQTTVzebxwTR4_fTM7kQ',
  'kid': '97728F506F653AC9E7EF5A4CF12D5CB7'
};
const SIGNING_ALGORITHM_RSA = 'RS384';

// ------------------------------------------------------------
// Set up the application
// ------------------------------------------------------------
// https://codeburst.io/how-to-implement-openid-authentication-with-openid-client-and-passport-in-node-js-43d020121e87
// https://www.codespeedy.com/how-to-enable-https-in-express-js/
// To create new certificates:
// openssl req -x509 -nodes -days 8000 -newkey rsa:2048 -keyout ./sslcert/server.key -out ./sslcert/server.crt
// https://stackoverflow.com/questions/11744975/enabling-https-on-express-js
// https://github.com/goranlisak/openid-connect-sample
const app = express();

app.use(cookieParser());
app.use(express.urlencoded({
  extended: true,
}));
app.use(express.json({ limit: '15mb' }));
app.use(session({
  secret: 'secret', 
  resave: false, 
  saveUninitialized: true
}));
app.use(helmet());
app.use(passport.initialize());
app.use(passport.session());

// Gets a serialized user object into the session
passport.serializeUser((user, done) => {
  // Uncomment to debug result:
  /*  
    console.log('-----------------------------');
    console.log('serialize user');
    console.log(user);
    console.log('-----------------------------');
    */
    done(null, user);
});

// Deserializes user objects out of the session
passport.deserializeUser((user, done) => {
  // Uncomment to debug result:
  /*
    console.log('-----------------------------');
    console.log('deserialize user');
    console.log(user);
    console.log('-----------------------------');
    */
    done(null, user);
});

// ------------------------------------------------------------
// Set up OpenID Connect
// ------------------------------------------------------------

Issuer.discover(STS_URL).then(oidcIssuer => {

  var client = new oidcIssuer.Client({
    client_id: CLIENT_ID,
    redirect_uris: [ OIDC_REDIRECT_URI ],
    response_types: [ 'code' ],    
    token_endpoint_auth_signing_alg: SIGNING_ALGORITHM_RSA,
    token_endpoint_auth_method: 'private_key_jwt',
    post_logout_redirect_uris: [ OIDC_POST_LOGOUT_REDIRECT_URI ],
  },
  { 
    // This is needed to sign the client assertion:
    keys: [PRIVATE_KEY_RSA]
  });

  passport.use(
    PASSPORT_OIDC_STRATEGY,
    new Strategy({ client, passReqToCallback: true }, (request, tokenSet, userinfo, done) => {
      // Uncomment to debug result:
    /*  
      console.log('-----------------------------');
      console.log('tokenSet', tokenSet);
      console.log('userinfo', userinfo);
      console.log('-----------------------------');
      */
      request.session.tokenSet = tokenSet;
      request.session.userinfo = userinfo;
      const user = tokenSet.claims();
      user['id_token'] = tokenSet['id_token'];
      return done(null, user);
    }));
    /*
    passport.use('oidc', new Strategy({ client, params }, (tokenset, userinfo, done) => {
      return done(null, userinfo); // return user information
  }));
*/
  // ------------------------------------------------------------
  // Application paths
  // ------------------------------------------------------------

  // ------------------------------------------------------------
  // Index page
  // ------------------------------------------------------------
  app.get('/', (request, response) => {
    response.send(`
        <h1>Welcome</h1>
        <p> This sample demonstrates how a client can access an API that requires authentication.<br> 
        <a href='${LOGIN_PATH}'>Log in</a> and use <strong>Test IDP</strong> to authenticate the user. The logged in user can then call the API and access data. </p>
        `);
  });

  // ------------------------------------------------------------
  // The user was redirected after logout
  // ------------------------------------------------------------
  app.get(OIDC_POST_LOGOUT_REDIRECT_PATH, (request, response) => {
    return response.redirect('/');
  });

  // ------------------------------------------------------------
  // The user was logged on at a wrong security level
  // ------------------------------------------------------------
  app.get(LOGIN_IN_AT_WRONG_SECURITY_LEVEL, (request, response) => {
    response.send(`
        <h3>Access denied</h3>
        <p>You were logged on, but the security level was not accepted.</p>
        <p>Please <a href='${LOGOUT_PATH}'>log out</a> and try to log in log in with another identity provider!</p>
        `);
  });

  // ------------------------------------------------------------
  // Starting the passport authenticate function
  // ------------------------------------------------------------
  app.get(LOGIN_PATH, (request, response, next)  => {
    next();
  }, passport.authenticate(PASSPORT_OIDC_STRATEGY, {scope: CLIENT_SCOPE}));

  // ------------------------------------------------------------
  // The user has been redirected from HelseID:
  // ------------------------------------------------------------
  app.get(OIDC_REDIRECT_PATH, (request, response, next) => {
    passport.authenticate(PASSPORT_OIDC_STRATEGY, { successRedirect: LOGGED_IN_USER_PATH, failureRedirect: LOGIN_FAILURE_PATH })
    (request, response, next)
  });

  // ------------------------------------------------------------
  // The user has been authenticated:
  // ------------------------------------------------------------
  app.get(LOGGED_IN_USER_PATH, (request, response) => {

    if (request.session.passport == undefined || request.session.passport.user == undefined) {
      return response.redirect('/');
    }

    const user = request.session.passport.user;
    // the user info is stored as a TokenSet:

    if (user['helseid://claims/identity/security_level'] !== '4') {
      // The security level is not at the required value
      return response.redirect(LOGIN_IN_AT_WRONG_SECURITY_LEVEL);
    }

    const userInfo = JSON.stringify({
      user
    }, null, 1);

    response.send(`
        <h1>Success</h1>
        <p>The user was logged in</p>
        <p>${userInfo}</p> 
        <p><a href='${LOGOUT_PATH}'>Log out</a> here!</p>        
        `);  
  });

  // ------------------------------------------------------------
  // Logs out the user from HelseID:
  // ------------------------------------------------------------
  app.get(LOGOUT_PATH, (request, response) => {
    //const ession = request.sessionStore.get(request.session.id);
    // We need the ID token in order to indicate to HelseID what our session is
    const user = request.session.passport.user;
    response.redirect(client.endSessionUrl({id_token_hint: user['id_token'] }));
  });

  // ------------------------------------------------------------
  // This gets redirected from HelseID post logout:
  // ------------------------------------------------------------
  app.get(OIDC_POST_LOGOUT_REDIRECT_URI, (request, response) => {
    // clears the persisted user from the local storage
    request.logout();
    // redirects the user to a public route
    response.redirect('/');
  });

  // ------------------------------------------------------------
  // Login error:
  // ------------------------------------------------------------
  app.get(LOGIN_FAILURE_PATH, (request, response) => {
    response.send(`
        <h1>Error</h1>
        <p>The login failed</p>
        `);
  });
});



// ------------------------------------------------------------
// Start the application
// ------------------------------------------------------------
// Create a NodeJS HTTPS listener on port 5040 that points to the Express app
// Use a callback function to tell when the server is created.
const privateKey = fs.readFileSync('sslcert/server.key').toString();
const certificate = fs.readFileSync('sslcert/server.crt').toString();
const credentials = {key: privateKey, cert: certificate};

https
  .createServer(credentials, app)
  .listen(PORT_NUMBER, ()=> {
    console.log(`Now listening on: https://localhost:${PORT_NUMBER}`)
  });
