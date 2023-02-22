# Short example for client credentials
from authlib.integrations.requests_client import OAuth2Session
from authlib.oauth2.rfc7523 import PrivateKeyJWT

#client_id = 'helseid-sample-client-credentials'
#client_secret = 'Your GitHub client secret'
#scope = 'nhn:helseid-public-samplecode/client-credentials'

#client = OAuth2Session(client_id, client_secret, scope=scope)

token_endpoint = 'https://helseid-sts.test.nhn.no/connect/token'

private_key = "{ 'p': '1JDtA9RzEs-dgZ7YDOZPX4VJLofseq9cs7rTvxF9I6QXo3_0-FIkymdiitIcLrfXtRcmJ8bIDzhHYYUkt_5cYSdCjg90EsbLRyjz7xl4wXgCJsEga-shhjxMqiZ7JS5lMd2FkuHw9fk6iDlokmn6zIMDFgSwCy-avN4Wl8k0tS8', 'kty': 'RSA', 'q': 'nQd92kOJqtjgWy6DEeOCAsW18qLYEiXXDYFpWnxNBK9Bx5ao01UmfSGyjeCyrmYrxGdUsyXQRirUlUq5foXhbC6noiVLv2763p2vwdbQdy6UPj5rzjPhmaKBV3MXi-VKB7Hdf6vR6AXk5dBIBTPlmjJor-blngu3QKoIpzisLDM', 'd': 'NnL8DUu6Ci0BCJXN88RyQ-Og_j4tF3LkzIFg45ehkJRcxSIFU53hrewKqdVkkjFCl898CT_5AqItzjJeW6BGW5nVOVOACu_FNcCFM9_ZevXM5VtrwS5zFmQep19JZ5uFOPcBGjLOU5vd0FoBckZ2YtwvP5p8dHjL3tA0R2nvkSsOBQYCcrPvFwYLlMSxEfbvKZWUAIX3wtKOmfOw3daa5fYkpUxBXz2XcMYhXZE87vUA0qG_1d6K0r51DEayuej9D11lcIxCWXWi1LAT7FFjikt8nKxeRIZEieViPEjxw8gSWKLGMMy2FZQgLLvDuHmTzuEDPgZVBbCElkl7l8dMYQ', 'e': 'AQAB', 'use': 'sig', 'qi': '09EacJNthPVKNoea8Nj5PC37iilrc2mlVQtrXobCAkHcByVR7xPZT1cZZ3NSiMpSQu2y4tr_LA62xmnSU3zVgcEi9CfI8h2nGN82wv8SVxtUK9-RcqAohJj0y4UtnhA9atcTfZvlI5RLMP7mdLnkgzZe9Oq7Jr_OMD4IKZhG8-E', 'dp': 'iBXkd5A6v69FUifEf7Wu2SN2r6B7iCveuH4CdA-ZQwkZzSXtSlEklqRLlT5gppQyOBCC7_I2QHAyWr-nu1fQAq7k0Bgaoq68k2knikqPYaUYE4GO5ShahRrzpfcO3cXvKVZ93oRiBMezbmT6isnos6eogR8tKWwnr4SriC9bXCc', 'alg': 'RS512', 'dq': 'm2g6qbSFnswc3qDdnuqmVNAPDh8T8IH6n6cf-Sljn-tDEqCMXPq8qMKcz8U9kVQUpMAPF22o_oiM82OMySb-ve4-gT6gBMl1BrTQqOpMTmeO1zs3vk-iSkaF82I4P3-hEJR7Pktx5ktPChJj9KIz7bNN4CiHvy6hIiIlhjmUS_k', 'n': 'gmMZ0dOeUyEbEQ0Bz4gGqcYgMfreJ1yIMD_h0HyRjNB0-e1jzb4yjBBkx1pOpPvt02trdyt_nOCmR7DLrHmLrDCmAbpYIytUPNEuz6GJkT31oL3vDmNqrkawQwe3B04FIWohiTQWgnPaZiTViRdajxpJJW6juPVzr75TlWwyimT2bDq-5TxwiLxyRJDUUWnTvjUPkFVHGMwNFQxV9SCrjHSueCF8vQ959Peh-Yxvtvz2T29HgHKf7oh37DCEy-PrWLTgykeEvNhyWclDM4jm5NaqOCE-sbmmRR4wnCWksvGod7aS7IaKzg2_nNihrxAIzIOqEuLXXaUmYPeHKDgsXQ' }"

session = OAuth2Session(
    'helseid-sample-client-credentials',
    private_key,
    token_endpoint_auth_method='private_key_jwt'
)

session.register_client_auth_method(PrivateKeyJWT(token_endpoint))
session.fetch_token(token_endpoint)