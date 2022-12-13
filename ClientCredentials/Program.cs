using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace HelseId.Samples.ClientCredentials
{
    class Program
    {
        // The corresponding public key in HelseID Test: {'e':'AQAB','kty':'RSA','n':'qcByQdQX3xXnvjdZwJfiCh87KNVSDdxD6wOuUlfAod1d_1-nGFzFj8mlWVIdUTRuZAoD9urHU-t7M9K4Ytg7u6hfxrESXVlXRpfAxDEKbDK_xt1V9AKqN1E6vCKXIXvkZMYgdstjBejjC1-3IjwApvxEb8XVzpFWfI_dZc4pyk6E7VSJZGYYNxJuSXCLntkHrGgmaWp9BAbfq1S__WwufHAc6JvebGAYgCH0dihV-ueNqbS6a2lHJUqc88baCSwRrPjEDLzmOaZIsNmWvO-TJtqWdYUmdGK7OPMwnRs8s-v4WtwtHsSODklbYC8Q6E1Wmf268NlFlMTz8x9QeKP9j8P16vI8a2Dz4Uuli1YbkUnPoEqLI_asV_fE5eo4aydaTzJK_8CNm5XB0M8XoFqTQe72AOEhhkAdROa_5SOLWdBc61Bx4U0_evv1ER-vS52e8q_6qPNFntrGKYzKWnqoNmYnO117ksjnKTn9uFj88U4Akxwcxyql0a9vYK9p_IaVbxIesZ0EzAanUgWkiOpMyjJxeo8PxAO3n1O1sQ92EIEglYJ5qLzgegWKoZZLxRD_3jUoDNKbTeelqE2Rr9725VzrGa8VIv7te2ezz3AszfsGcU2emKVaZmfo043dy4KwYq-NmJgAt84ODoqgydqIAjHh4QU3vyDcRhLVqcZEjCU','kid':'BE9A843ED0A783F7881977BA671A8533'}
        private const string RsaPrivateKeyJwk = "{'d':'n_3zcpn9WdTilETFAiHk-RdQgf71FH07QmE4xwFQXP8wngZAjlS6G_i5MIOVgDkDpqRN8kZ8UltKxqBgC0G0ov1iL_oqzfLrsGxNUlzKbwox-LQaVB7C4dpcmn-bpAKKVHTsmiq-DQ7gF5NCHzEgiEKSGisDhmszcbmCuXZjqvrwjGO2Dvuu7REhA3ThE3mIovWS5p5ocAgufeQmBCwXcE-W36IxNEIqNXJLX6ZKy2289muRnLUgSkSVw6nzTNvNWRMqbLsJ3uIb9xtN2IuRTPiPAPrbMKzxuw5zDlBimU3ZyYFlwW8OWObJCdGyhNCHxCICNQkDbOFo8Afep6Ygz_uzu7eBPDI6ualbGFFRjWWuXqakcOpHEari-DWAigLXDKQ990x7MgfeyHf_tPhTXVcvOfdMmBbtBks812NduoBx3_3XoqgHIKT1_yUAZarsEJHZF0AIYtIxvjokiq1fqQvy3UaWuYeTffgHYbziMW0Xz0J18bsfQxrFjMaB5FqJLzTqkXnZ6wpxSq3AyNoXNz5IJXLmg9A0IT0HPqM3-gVf8AY0YRL4DPKvLip6BJN3AKa5PQSuKBYFi52IbJXB0Xfx9ghtOVNZDGZYdo75_gCpMkZFKLs6SlnW8fBCejmHuQurDnhkDE1B3H2WWOyz_IFr6UaN1jJzioU-HAio5-0','dp':'sLpQ1TesYWE2wJ3C0yvpD0E7gzw-VNdQJCi8DE6V227TIKVQI9sqzBCGjPN8cxiObE_7NWz-TZbPOTkbQ33eVyIH8g6cOf8LlCN_mV0b7Tn5UX_LzhOpBxW1O7Z4txE-LW_pgfdT_FnrJiQSWKpFHTSrFpB4ZkkF0Hx-AKO2jqcKFSsv0g4OYlNBN2ok9hDqVl1dILC0uv6B2ppRDrO0g86wuTEUwF9eTa8onOy1uUJ6y6Tc1JwFTs8o57h5LlQ8y2-xGL1juiK8GhBLR5k_5ht6k73217u_JHQDEKsEZbZ3DpU_tbjVKht2Br5gw_4ZCyET8OtLEcidsJFIwGPfYQ','dq':'CuVhvtQfQlbxoS3r3UGYN2zm3J3Bpvvg8qBH91Nd8vHsT9mQyvSqchnlaBXjHvzU_vBDgiEOmCUYwMVuwS4PbJL57vOSet1F9gOOYh5gkm9wOSDZ0oeaby8ZlrNXkouvpM4Cl8tSXku-vHhh6rx1zwQvbmvCrAnRS267VFsAD2zoVv8Poy3kGLDLXkG70MqoBQ7efcJcOYqA3JEXwJX-pKoWAWF9j548RTpqxNdJvXuhil873H9JJRA34tddsHLwE-BnL-iBeqsTpSKoCrLJqg8zOE3etVvvnVeepXABjgdbhJxz36KcMJkU_d6Uc_W3TfZu6EIjJGo1uEgcRkyNMw','e':'AQAB','kty':'RSA','n':'qcByQdQX3xXnvjdZwJfiCh87KNVSDdxD6wOuUlfAod1d_1-nGFzFj8mlWVIdUTRuZAoD9urHU-t7M9K4Ytg7u6hfxrESXVlXRpfAxDEKbDK_xt1V9AKqN1E6vCKXIXvkZMYgdstjBejjC1-3IjwApvxEb8XVzpFWfI_dZc4pyk6E7VSJZGYYNxJuSXCLntkHrGgmaWp9BAbfq1S__WwufHAc6JvebGAYgCH0dihV-ueNqbS6a2lHJUqc88baCSwRrPjEDLzmOaZIsNmWvO-TJtqWdYUmdGK7OPMwnRs8s-v4WtwtHsSODklbYC8Q6E1Wmf268NlFlMTz8x9QeKP9j8P16vI8a2Dz4Uuli1YbkUnPoEqLI_asV_fE5eo4aydaTzJK_8CNm5XB0M8XoFqTQe72AOEhhkAdROa_5SOLWdBc61Bx4U0_evv1ER-vS52e8q_6qPNFntrGKYzKWnqoNmYnO117ksjnKTn9uFj88U4Akxwcxyql0a9vYK9p_IaVbxIesZ0EzAanUgWkiOpMyjJxeo8PxAO3n1O1sQ92EIEglYJ5qLzgegWKoZZLxRD_3jUoDNKbTeelqE2Rr9725VzrGa8VIv7te2ezz3AszfsGcU2emKVaZmfo043dy4KwYq-NmJgAt84ODoqgydqIAjHh4QU3vyDcRhLVqcZEjCU','p':'0KxlUgbxuakt1gfWmr7SAKNJmH_SnsMkEMrw-AFWnAcBB9yB7JHjsgyhmkfrz2QmGfgQjit7u1GtkclWZmXY_Bcq9-mhulB2wNNxtLZgIXT_Y4UK6X1W1Y-qLmXu8RCzwxvtVpEvZPGk73dLVMbMJ9JI0K0MSAgsZUJo7OUGD-3QA8m1c-yq1zK26rmnvYlWTToFKCKis4_Ql0PfbG3S8etugnsCXkHSAt3JmAuZJ72HL76nyHjZv-Q23M4xCEDYOOGBjT1O_zfRpBtUT95iJXegk9qPabBB-ZghFRjRqL-1V1uluUTBNnb15mm2ER2UJu_7XCsUIgmEbulQTF0hTw','q':'0EBA0XqE7K3My1VDDjgaWwQgqfdcG5aUKGgd1WVBf242odkTrH4gTymMIWTNhbUxLpsFoO3DUZPQW9emTr_qGR1g0z5L4mrSVMZRzNeDO80iAurLK8ft_d_0fOWEW-wnwRzY8Iog0hgd4QasZIRVXnDqUenVbif3bt48s_LkL0F0-xpAhZn2yTGAkFH6qfSQN3ZMWMGo8TKStPdJ9iBX_twjy7PFQLZ0Rq1apzyXKfwxuNWvzEYAeVhj-Hp6Rl_AATr76sY-gE2EiFjH4lkq_cAKHcuET1WOKxhDRUGMbtwOVPU1-9r091wr0fqmFAVSBkBraKxDrkbJA6VCGjEWSw','qi':'QAznSkYlpX6Mp7Fq9-tmT1fKzpC45IfEwCjdHaJtki17er2_ch-B4knqOV20jZOhHHhEP_Dy7_ytia68qjexWdJI8roca7LHrUmdtKZO4md5fUCnC989285RMiNBllAK-0XACu0oK9coinbAVq3nstbxBknwe7qoNG-BkcqHAE5uX4dkJkNZQwTQfS_Cgj6lD6u36t-VJc3QH4LHv_RrNiMUBfm-S1jpDm_q_VUZ7U7jUu7Ei5sS4ptkJk9uJreyytOWzXbT_WxpIHQkPOSvi1Px0zZAV4oCqXyVOA1uwZCxWKN3aO_dLT4H45vJg1Bd0DD8TvEzYpCKzRxLniXb0A','kid':'BE9A843ED0A783F7881977BA671A8533'}";        
        private const string RsaPrivateKeyJwkForClientWithUnderenhet = "{'d':'JrIh4aNzYjG74j7aB-8LOHOBnlOwthXsxwaI5BwoyiUc6kVdgeeZq2XOlkyqWBbF5pzb-2WITkydJ7KK2KzbhKtf6mQOYE7edALzuCIscEoR7nxxByIYFvcL8vtYcahNGe-Xz1SJVYpljArWPfzfCFXIPIO11qW5PRfYpraMoJrwVQiJEVWkiZKZO6AZepqR_Tyuk6N6r-g7aqgnoPetDYw6CL651lxdynxuu4dzmL0GsfvzjKdEAMHF0dod3lGSM0Y7DHcN78WHsY6yzBuKSWHES4eqj5RU6UBhaAxSg3U_HGy7QybIkvb5-NILZtD6yYC-KTusazWfNKmqF2uP6A_M_JA5yOusaJFPf4cUPDMfL_u7TUIkwlro11pw049woUrb1jk5652-ieNYZj4eoCm-EEh18_WvJCdkSgY4nIldkPWV7d_QpCkKb8daLa7jLIfJyjBY0rABeUy5NgKlUU2LOBNzJttUBSMtdBxqbdEkgzIyvGUXYWzeZSWq7RRCdFI_Y6oLY9AEr_3yJM-8-dhjiq3nybXSLhx3LBkNyil6q_Ef_JO1Ki4FE4m0Q3ngCFv7vWKRg9HCt13_Z4LZO_peOaFG08g4j6R--m_P9fG09OJcRczT9NKeWnERoqFMlFc_AOyq6TE3GuwlHoJVduc6k-3eI84oOs2DTD-Ogdk','dp':'YzLd4BTcTdT4ed26rGZXTp1GhJNI2eryEuyCN9mAYaNFCYgs5jgfXvuB6NuBbblVgg46vBK882BmInQZmQcLSZDV4uLtwLmF8WBQe7RQ2kAPb3vvu3F-jVfqR1kf7E9iDKPgbyUBG6OHzdXMXk-e4T23xdfr3jjtAS5V_YFqTMk1aiLYT34OPVcsZm9M5EC03tTauz14Pd--eyAVOzOsulIZJdBZdlpF2ChDzMc1TQXtcdbymmtcbvPZvMopc6x9Aw7bCpUDa7dLSHYsJREDrss3OQ8-GOlLQeA3yaTq5A_PCBHEwS5FdBgOwlOcSEooiy4xrJw3YTJZnpL1m91tvQ','dq':'k6UF0D_N2peglEfC7bmpo__IX7LeVKKBzFsnh-Lmo6MrHQQyVWY7-7dq2tNZKvd4QxiFDaeHYF7vbgnrqqtdaGUUdS8nVhvWbpvngWLAZh_o3EZ3N_PY8x_ZAIMn33_rXpYHuxg4mjnca22khqDD-xTCpVYQTxDVFOSsNrpU_hpoRgbfwha5U2HZdQ-GdRRAKHq4GvDWuvswnqlDbH31l8hzOcFU-PWwGwo0xTUN4OfK9K1CJ3OvXCS0chbllqyZYI9E2jI-Nde8faUpM23VYF5V2Z9HKbtaFUakqQlaR9uXtIxT2EVO-OEMRqlbPDNnYyu9v-QbthBLWQgX2MDclQ','e':'AQAB','kty':'RSA','n':'oMV8mrf9wS2z6sBrF9_aNculRp5RHC8tGanalfmnGeZ47icC8R6Z93zFJ44EKrvFqYKv_ndxPrE25HAWg3O3tRFf8YXnVmRoCzBC6h8KInJa0-1tBWgETTy2UdlM2HH5WZBvo8I1uFPLAJ95Ha9jw7STZhS_J11yM8Gko3KqQ3ERbgQPF1EURhimKJg_iFvd-vrWCVEFilpaeeg2ayqmmMU1_oTtSgI1qu4R-ztJliFsTEFlcUsWTKF9gsYv2dHaO2fv8cCdeC9hdT8SlE7pt0s7xu_3jG4KzUnSPZG_Z3D2-KEeaWS21aJnq0ic-7BwFkJGVk3tWcfi0w8jTMqHrTBFkZhja7HBw5ZsFoNhJ4c1Kt0vUdF6Y9_sUOQY8I7Tj6Yim6lq7VlL9zpTO7isl5bErafbtq0JxH9gXKeroGz1TMw1JqiP8RCZYsZNoHWx1-GS-21ktOD72jFrK2i3M2LVYXdaSc0OqDFoYCB31NriC309iZmfvqC5z84FFKki0r_oUrg11GQf9esbGPJeryIg7x_taKtl72e7WpimuGsdlC5Y6_cxYtJ2iTuwtSxOgNpbEy6-k3jBls_UBHDxFTFehUpKvfoQ9qGer905TeLnvGKTSuFOqmyXDbjlh-d57jFUkbZ3VCo6Im-npqpAYWLoALow_NanLQRZAoFvCuk','p':'xlcDWaJ4FAD8ifGiM5X_se9DkmwqZxeKJPJHWyRRn-bPUWgQu3tlikb0PaYDmi166S1CC3INIzPy9rkxomCZFUKVs7-WT5BWDxNWNU1vEHSKRElospSAY6UmV4f364A2s8XOseT6nbT0v5w8M4P0Eqe87FzsKdZRtdR5viv0LVN54Srtih3xH_RUZJ-KME9sCqKea8_6txmqtGcIqeSKATWOwKvxHpEVDzcHnX22Vje4lR0inAe9GdqMTlGuGhHX-jgvzyPtMu1zcn33XybideyVjN3s-ctEdP3Pf4NyzP1iZBHIQkDgMzcFaFiWTA7wf-N77-0JdhY75StA_JJXxw','q':'z4KHJkY9b4K2iC0wh25mppNEPPqkUyVEyKee8_fd3a-vXQ1b8dxdx8dhBWc-1GE2kvB8anze8EGdn8GZRhoy5vfYtC46mLTBs50U0HUwXziCPxZzkUAB-IQXPFu1IW7hocbfXMrMyp4VWjDF7Y-DPmdakGRBxtgWmz7jjCFmWPUFoplV1h0dqy9-TC9dhTxi2LI7QesgByAojGxc1cRvBJcDpYFkmdK-_bfFq7SEoLw4J5LDlKWpGldtKwb4dx-n4WQih4LJIhTw2Bi4gxi4dH8u5UUO0VDNHGMut-evuHE5U-B3qfd0D2qGG9q43mePxqhJDyx0wHr7_-Ma9e9nzw','qi':'k4j15FTmUivfDPRdS4uQ-02enadgBzUHPXDrlM-SC5SVfhrXSeN7a426yKKc_Mdt-GSFT1QKYOZYfCfX9eXvidJIP1recl2nvegyRfynXcN07O2fCpqRXhNyffNt1sf7Q9LHpVTpY6AzgSAqXFTsx3PyGwJNGoznB5crFMHjt1wK2F8hhyEM_8u8UP0KeYuD5H8eIVq5V00yMK3xh-i1iL2FWFgBypJ4EsQZq6D15EwJX9LZCA5UfbQ_sifspudumbyTz6Y7DfKeVMdCr5c7YGD1Svxgm7ZfUwnG3HFutvl9ZTGVcVLMsEw0pclxxQhdKkPLFk4HxyCUnNnkJJgazw','kid':'EF5F04B8B1FCACD24F7A907A6E2FACC1'}";

        private const string ClientId = "helseid-sample-client-credentials";
        private const string ClientIdForClientWithUnderenhet = "helseid-sample-client-credentials-with-underenhet";
        private const string Scope = "nhn:helseid-public-samplecode/client-credentials";
        private const string StsUrl = "https://helseid-sts.test.nhn.no";
        // The child organization number is provided by the EPJ
        private const string ChildOrgNo = "999977775";

        static async Task Main(string[] args)
        {
            
            try {
                var client = new HttpClient();
                var tokenEndpoint = await GetTokenEndpointFromSts(client);

                var request = CreateTokenRequest(tokenEndpoint);
                //Console.WriteLine("Request token:");
                //Console.WriteLine(request.ClientAssertion.Value);

                var result = await client.RequestClientCredentialsTokenAsync(request);

                if (result.IsError)
                {
                    await Console.Error.WriteLineAsync("Error:");
                    await Console.Error.WriteLineAsync(result.Error);
                }
                else
                {
                    Console.WriteLine("Access token:");
                    Console.WriteLine(result.AccessToken);
                    Console.WriteLine("Copy/paste the access token at https://jwt.ms to see the contents");
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(e.ToString());
            }            
        }

        /// <summary>
        /// Gets the discovery endpoint from the STS and extracts the URI for the token endpoint
        /// </summary>
        private static async Task<string> GetTokenEndpointFromSts(HttpClient client)
        {
            var disco = await client.GetDiscoveryDocumentAsync(StsUrl);
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }
            return disco.TokenEndpoint;
        }

        /// <summary>
        /// Builds a request for a token, using the client credential grant
        /// </summary>
        private static ClientCredentialsTokenRequest CreateTokenRequest(string tokenEndpoint)
        {
            var clientAssertion = BuildClientAssertion(tokenEndpoint);

            return new ClientCredentialsTokenRequest
            {
                Address = tokenEndpoint,
                ClientId = ClientId,
                Scope = Scope,
                GrantType = GrantTypes.ClientCredentials,
                ClientAssertion = clientAssertion,
                ClientCredentialStyle = ClientCredentialStyle.PostBody
            };
        }

        /// <summary>
        /// This method builds a client assertion, which includes a JWT token that establishes the identity of this
        /// client application for the STS. See https://www.rfc-editor.org/rfc/rfc7523#section-2.2 for more information
        /// about this mechanism.
        /// </summary>
        private static ClientAssertion BuildClientAssertion(string stsTokenEndpoint)
        {
            var token = CreateSigningToken(stsTokenEndpoint);

            return new ClientAssertion
            {
                Value = token,
                Type = ClientAssertionTypes.JwtBearer
            };
        }

        /// <summary>
        /// Creates the token that is used in the client assertions
        /// </summary>
        /// <param name="stsTokenEndpoint"></param>
        /// <returns></returns>
        private static string CreateSigningToken(string stsTokenEndpoint)
        {
            var claims = GetClaimsForClientAssertionToken();

            var signingCredentials = GetClientAssertionSigningCredentials();

            var jwtSecurityToken = new JwtSecurityToken(
                ClientId,
                stsTokenEndpoint,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddSeconds(60),
                signingCredentials);

           
            
            //  "authorization_details":{
            //      "type":"helseid_authorization",
            //      "practitioner_role":{
            //          "organization":{
            //              "identifier": {
            //                  "system":"urn:oid:2.16.578.1.12.4.1.2.101",
            //                  "type":"ENH",
            //                  "value":"[orgnummer]"
            //              }
            //          }
            //      }
            //  }
/*
            var authorizationDetails = new
            {
                type = "helseid_authorization",
                practitioner_role = new
                {
                    organization = new
                    {
                        identifier = new 
                        {
                          system = "urn:oid:2.16.578.1.12.4.1.2.101",
                          type = "ENH",
                          value = $"{ChildOrgNo}"
                        }
                    }
                } 
            };

            jwtSecurityToken.Payload["authorization_details"] = authorizationDetails;
*/
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        /// <summary>
        /// These claims are used in the token that is contained in the client assertion 
        /// </summary>
        private static List<Claim> GetClaimsForClientAssertionToken()
        {
            return new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, ClientId),
                new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
            };
        }

        /// <summary>
        /// Creates a SigningCredentials object that contains our security key. To be used in the client assertion.
        /// </summary>
        private static SigningCredentials GetClientAssertionSigningCredentials()
        {
            var securityKey = new JsonWebKey(RsaPrivateKeyJwk);
            return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);
        }
    }
}
