async function setDPoPTokenInRequest(req, testTokenProxyAddress) {
    try {

        // Get the access token from the test token service proxy:
        let response = await fetch(testTokenProxyAddress,{
            headers: {
                'Content-Type': 'application/json'
            },
            method: 'POST',
            body: JSON.stringify({ uri : req.url })
        });

        if (!response.ok) {
            console.log('Could not get an access token from the test token service proxy. Error message: ' + response.statusText);
            return req;
        }

        const parsedResponse = await response.json();

        // Set headers in the request:
        req.headers['Authorization'] = 'DPoP ' + parsedResponse.accessToken;
        req.headers['DPoP'] = parsedResponse.dPoPProof;

    } catch (e) {
        console.log('Could not get an access token from the test token service proxy. Error message: ' + e);
    }

    return req;
}

