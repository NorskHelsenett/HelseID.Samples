using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HelseId.Samples.ClientCredentialsWithUnderenhet
{   
    class Program
    {
        // Corresponding public key: {"e":"AQAB","kty":"RSA","n":"oMV8mrf9wS2z6sBrF9_aNculRp5RHC8tGanalfmnGeZ47icC8R6Z93zFJ44EKrvFqYKv_ndxPrE25HAWg3O3tRFf8YXnVmRoCzBC6h8KInJa0-1tBWgETTy2UdlM2HH5WZBvo8I1uFPLAJ95Ha9jw7STZhS_J11yM8Gko3KqQ3ERbgQPF1EURhimKJg_iFvd-vrWCVEFilpaeeg2ayqmmMU1_oTtSgI1qu4R-ztJliFsTEFlcUsWTKF9gsYv2dHaO2fv8cCdeC9hdT8SlE7pt0s7xu_3jG4KzUnSPZG_Z3D2-KEeaWS21aJnq0ic-7BwFkJGVk3tWcfi0w8jTMqHrTBFkZhja7HBw5ZsFoNhJ4c1Kt0vUdF6Y9_sUOQY8I7Tj6Yim6lq7VlL9zpTO7isl5bErafbtq0JxH9gXKeroGz1TMw1JqiP8RCZYsZNoHWx1-GS-21ktOD72jFrK2i3M2LVYXdaSc0OqDFoYCB31NriC309iZmfvqC5z84FFKki0r_oUrg11GQf9esbGPJeryIg7x_taKtl72e7WpimuGsdlC5Y6_cxYtJ2iTuwtSxOgNpbEy6-k3jBls_UBHDxFTFehUpKvfoQ9qGer905TeLnvGKTSuFOqmyXDbjlh-d57jFUkbZ3VCo6Im-npqpAYWLoALow_NanLQRZAoFvCuk","kid":"EF5F04B8B1FCACD24F7A907A6E2FACC1"}
        private const string PrivateKeyJwk = "{'d':'JrIh4aNzYjG74j7aB-8LOHOBnlOwthXsxwaI5BwoyiUc6kVdgeeZq2XOlkyqWBbF5pzb-2WITkydJ7KK2KzbhKtf6mQOYE7edALzuCIscEoR7nxxByIYFvcL8vtYcahNGe-Xz1SJVYpljArWPfzfCFXIPIO11qW5PRfYpraMoJrwVQiJEVWkiZKZO6AZepqR_Tyuk6N6r-g7aqgnoPetDYw6CL651lxdynxuu4dzmL0GsfvzjKdEAMHF0dod3lGSM0Y7DHcN78WHsY6yzBuKSWHES4eqj5RU6UBhaAxSg3U_HGy7QybIkvb5-NILZtD6yYC-KTusazWfNKmqF2uP6A_M_JA5yOusaJFPf4cUPDMfL_u7TUIkwlro11pw049woUrb1jk5652-ieNYZj4eoCm-EEh18_WvJCdkSgY4nIldkPWV7d_QpCkKb8daLa7jLIfJyjBY0rABeUy5NgKlUU2LOBNzJttUBSMtdBxqbdEkgzIyvGUXYWzeZSWq7RRCdFI_Y6oLY9AEr_3yJM-8-dhjiq3nybXSLhx3LBkNyil6q_Ef_JO1Ki4FE4m0Q3ngCFv7vWKRg9HCt13_Z4LZO_peOaFG08g4j6R--m_P9fG09OJcRczT9NKeWnERoqFMlFc_AOyq6TE3GuwlHoJVduc6k-3eI84oOs2DTD-Ogdk','dp':'YzLd4BTcTdT4ed26rGZXTp1GhJNI2eryEuyCN9mAYaNFCYgs5jgfXvuB6NuBbblVgg46vBK882BmInQZmQcLSZDV4uLtwLmF8WBQe7RQ2kAPb3vvu3F-jVfqR1kf7E9iDKPgbyUBG6OHzdXMXk-e4T23xdfr3jjtAS5V_YFqTMk1aiLYT34OPVcsZm9M5EC03tTauz14Pd--eyAVOzOsulIZJdBZdlpF2ChDzMc1TQXtcdbymmtcbvPZvMopc6x9Aw7bCpUDa7dLSHYsJREDrss3OQ8-GOlLQeA3yaTq5A_PCBHEwS5FdBgOwlOcSEooiy4xrJw3YTJZnpL1m91tvQ','dq':'k6UF0D_N2peglEfC7bmpo__IX7LeVKKBzFsnh-Lmo6MrHQQyVWY7-7dq2tNZKvd4QxiFDaeHYF7vbgnrqqtdaGUUdS8nVhvWbpvngWLAZh_o3EZ3N_PY8x_ZAIMn33_rXpYHuxg4mjnca22khqDD-xTCpVYQTxDVFOSsNrpU_hpoRgbfwha5U2HZdQ-GdRRAKHq4GvDWuvswnqlDbH31l8hzOcFU-PWwGwo0xTUN4OfK9K1CJ3OvXCS0chbllqyZYI9E2jI-Nde8faUpM23VYF5V2Z9HKbtaFUakqQlaR9uXtIxT2EVO-OEMRqlbPDNnYyu9v-QbthBLWQgX2MDclQ','e':'AQAB','kty':'RSA','n':'oMV8mrf9wS2z6sBrF9_aNculRp5RHC8tGanalfmnGeZ47icC8R6Z93zFJ44EKrvFqYKv_ndxPrE25HAWg3O3tRFf8YXnVmRoCzBC6h8KInJa0-1tBWgETTy2UdlM2HH5WZBvo8I1uFPLAJ95Ha9jw7STZhS_J11yM8Gko3KqQ3ERbgQPF1EURhimKJg_iFvd-vrWCVEFilpaeeg2ayqmmMU1_oTtSgI1qu4R-ztJliFsTEFlcUsWTKF9gsYv2dHaO2fv8cCdeC9hdT8SlE7pt0s7xu_3jG4KzUnSPZG_Z3D2-KEeaWS21aJnq0ic-7BwFkJGVk3tWcfi0w8jTMqHrTBFkZhja7HBw5ZsFoNhJ4c1Kt0vUdF6Y9_sUOQY8I7Tj6Yim6lq7VlL9zpTO7isl5bErafbtq0JxH9gXKeroGz1TMw1JqiP8RCZYsZNoHWx1-GS-21ktOD72jFrK2i3M2LVYXdaSc0OqDFoYCB31NriC309iZmfvqC5z84FFKki0r_oUrg11GQf9esbGPJeryIg7x_taKtl72e7WpimuGsdlC5Y6_cxYtJ2iTuwtSxOgNpbEy6-k3jBls_UBHDxFTFehUpKvfoQ9qGer905TeLnvGKTSuFOqmyXDbjlh-d57jFUkbZ3VCo6Im-npqpAYWLoALow_NanLQRZAoFvCuk','p':'xlcDWaJ4FAD8ifGiM5X_se9DkmwqZxeKJPJHWyRRn-bPUWgQu3tlikb0PaYDmi166S1CC3INIzPy9rkxomCZFUKVs7-WT5BWDxNWNU1vEHSKRElospSAY6UmV4f364A2s8XOseT6nbT0v5w8M4P0Eqe87FzsKdZRtdR5viv0LVN54Srtih3xH_RUZJ-KME9sCqKea8_6txmqtGcIqeSKATWOwKvxHpEVDzcHnX22Vje4lR0inAe9GdqMTlGuGhHX-jgvzyPtMu1zcn33XybideyVjN3s-ctEdP3Pf4NyzP1iZBHIQkDgMzcFaFiWTA7wf-N77-0JdhY75StA_JJXxw','q':'z4KHJkY9b4K2iC0wh25mppNEPPqkUyVEyKee8_fd3a-vXQ1b8dxdx8dhBWc-1GE2kvB8anze8EGdn8GZRhoy5vfYtC46mLTBs50U0HUwXziCPxZzkUAB-IQXPFu1IW7hocbfXMrMyp4VWjDF7Y-DPmdakGRBxtgWmz7jjCFmWPUFoplV1h0dqy9-TC9dhTxi2LI7QesgByAojGxc1cRvBJcDpYFkmdK-_bfFq7SEoLw4J5LDlKWpGldtKwb4dx-n4WQih4LJIhTw2Bi4gxi4dH8u5UUO0VDNHGMut-evuHE5U-B3qfd0D2qGG9q43mePxqhJDyx0wHr7_-Ma9e9nzw','qi':'k4j15FTmUivfDPRdS4uQ-02enadgBzUHPXDrlM-SC5SVfhrXSeN7a426yKKc_Mdt-GSFT1QKYOZYfCfX9eXvidJIP1recl2nvegyRfynXcN07O2fCpqRXhNyffNt1sf7Q9LHpVTpY6AzgSAqXFTsx3PyGwJNGoznB5crFMHjt1wK2F8hhyEM_8u8UP0KeYuD5H8eIVq5V00yMK3xh-i1iL2FWFgBypJ4EsQZq6D15EwJX9LZCA5UfbQ_sifspudumbyTz6Y7DfKeVMdCr5c7YGD1Svxgm7ZfUwnG3HFutvl9ZTGVcVLMsEw0pclxxQhdKkPLFk4HxyCUnNnkJJgazw','kid':'EF5F04B8B1FCACD24F7A907A6E2FACC1'}";
        private const string ClientId = "helseid-sample-client-credentials-with-underenhet";
        private const string Scope = "nhn:helseid-public-samplecode/client-credentials";
        private const string TokenEndpoint = "https://helseid-sts.test.nhn.no/connect/token";

        // The child organization number is provided by the EPJ
        const string ChildOrgNo = "999977775";
        static async Task Main(string[] args)
        {

            var clientAssertion = BuildClientAssertion(ClientId, TokenEndpoint, ChildOrgNo);

            var request = new ClientCredentialsTokenRequest
            {
                Address = TokenEndpoint,
                ClientId = ClientId,
                Scope = Scope,
                ClientAssertion = new ClientAssertion { Value = clientAssertion, Type = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" },
                ClientCredentialStyle = ClientCredentialStyle.PostBody
            };

            var result = await new HttpClient().RequestClientCredentialsTokenAsync(request);

            if (result.IsError)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(result.Error);
            }
            else
            {
                Console.WriteLine("Access token:");
                Console.WriteLine(result.AccessToken);
                Console.WriteLine("Copy/paste the access token at https://jwt.ms to see the contents");
            }
        }


        private static string BuildClientAssertion(string clientId, string audience, string childOrgNo)
        {
            // Builds a client assertion containing the authorization details claim with
            // the following structure:
            //        "authorization_details":{
            //            "type":"helseid_authorization",
            //            "practitioner_role":{
            //                "organization":{
            //                    "identifier": {
            //                        "system":"urn:oid:2.16.578.1.12.4.1.2.101",
            //                        "type":"ENH",
            //                        "value":"[orgnummer]"
            //                    }
            //                }
            //            }
            //        }

            var orgNumberDetails = new Dictionary<string, string>
            {
                { "system", "urn:oid:2.16.578.1.12.4.1.2.101" },
                { "type", "ENH" },
                { "value", childOrgNo }
            };

            var identifier = new Dictionary<string, object>
            {
                { "identifier", orgNumberDetails }
            };

            var organization = new Dictionary<string, object>
            {
                { "organization", identifier }
            };

            var authorizationDetails = new Dictionary<string, object>
            {
                { "type", "helseid_authorization" },
                { "practitioner_role", organization }
            };

            var serialized = JsonConvert.SerializeObject(authorizationDetails);

            var claims = new List<Claim>
            {
                new Claim("authorization_details", serialized, "json"),
                
                // Client assertions should have a "sub" set to ClientId
                new Claim(JwtClaimTypes.Subject, ClientId),

                // Setting a unique value for each client assertion is recommended
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N"))
            };

            var securityKey = new JsonWebKey(PrivateKeyJwk);

            var assertion = Jwt.Generate(clientId,
                audience,
                securityKey,
                claims);

            return assertion;
        }
    }
}
