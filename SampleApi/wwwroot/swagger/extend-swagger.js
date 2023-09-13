async function setBearerTokenInRequest(req) {
    let response = await fetch('/test-token');
    const parsedResponse = await response.json();
    req.headers['Authorization'] = 'Bearer ' + parsedResponse;
    return req;
}

