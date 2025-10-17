import no.helseid.configuration.Client;
import no.helseid.endpoints.token.AccessTokenResponse;
import no.helseid.endpoints.token.ErrorResponse;
import no.helseid.endpoints.token.TokenResponse;
import no.helseid.exceptions.HelseIdException;
import no.helseid.grants.ClientCredentials;
import no.helseid.signing.JWKKeyReference;
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.api.EventWithLegalBasisApi;
import org.openapitools.client.auth.HttpBearerAuth;
import org.openapitools.client.model.EventDocument;

import java.net.URI;
import java.util.List;

public class PersontjenestenApiClient {
  public static final String CLIENT_ID = "helseid-sample-client-credentials";
  public static final String SCOPE = "nhn:hgd-persontjenesten-api/read-with-legal-basis";
  // In the production environment, the private key should not be checked into version control, but kept secret and loaded into the application
  public static final String JWK_PRIVATE_KEY = """
      {
        "d":"n_3zcpn9WdTilETFAiHk-RdQgf71FH07QmE4xwFQXP8wngZAjlS6G_i5MIOVgDkDpqRN8kZ8UltKxqBgC0G0ov1iL_oqzfLrsGxNUlzKbwox-LQaVB7C4dpcmn-bpAKKVHTsmiq-DQ7gF5NCHzEgiEKSGisDhmszcbmCuXZjqvrwjGO2Dvuu7REhA3ThE3mIovWS5p5ocAgufeQmBCwXcE-W36IxNEIqNXJLX6ZKy2289muRnLUgSkSVw6nzTNvNWRMqbLsJ3uIb9xtN2IuRTPiPAPrbMKzxuw5zDlBimU3ZyYFlwW8OWObJCdGyhNCHxCICNQkDbOFo8Afep6Ygz_uzu7eBPDI6ualbGFFRjWWuXqakcOpHEari-DWAigLXDKQ990x7MgfeyHf_tPhTXVcvOfdMmBbtBks812NduoBx3_3XoqgHIKT1_yUAZarsEJHZF0AIYtIxvjokiq1fqQvy3UaWuYeTffgHYbziMW0Xz0J18bsfQxrFjMaB5FqJLzTqkXnZ6wpxSq3AyNoXNz5IJXLmg9A0IT0HPqM3-gVf8AY0YRL4DPKvLip6BJN3AKa5PQSuKBYFi52IbJXB0Xfx9ghtOVNZDGZYdo75_gCpMkZFKLs6SlnW8fBCejmHuQurDnhkDE1B3H2WWOyz_IFr6UaN1jJzioU-HAio5-0",
        "dp":"sLpQ1TesYWE2wJ3C0yvpD0E7gzw-VNdQJCi8DE6V227TIKVQI9sqzBCGjPN8cxiObE_7NWz-TZbPOTkbQ33eVyIH8g6cOf8LlCN_mV0b7Tn5UX_LzhOpBxW1O7Z4txE-LW_pgfdT_FnrJiQSWKpFHTSrFpB4ZkkF0Hx-AKO2jqcKFSsv0g4OYlNBN2ok9hDqVl1dILC0uv6B2ppRDrO0g86wuTEUwF9eTa8onOy1uUJ6y6Tc1JwFTs8o57h5LlQ8y2-xGL1juiK8GhBLR5k_5ht6k73217u_JHQDEKsEZbZ3DpU_tbjVKht2Br5gw_4ZCyET8OtLEcidsJFIwGPfYQ",
        "dq":"CuVhvtQfQlbxoS3r3UGYN2zm3J3Bpvvg8qBH91Nd8vHsT9mQyvSqchnlaBXjHvzU_vBDgiEOmCUYwMVuwS4PbJL57vOSet1F9gOOYh5gkm9wOSDZ0oeaby8ZlrNXkouvpM4Cl8tSXku-vHhh6rx1zwQvbmvCrAnRS267VFsAD2zoVv8Poy3kGLDLXkG70MqoBQ7efcJcOYqA3JEXwJX-pKoWAWF9j548RTpqxNdJvXuhil873H9JJRA34tddsHLwE-BnL-iBeqsTpSKoCrLJqg8zOE3etVvvnVeepXABjgdbhJxz36KcMJkU_d6Uc_W3TfZu6EIjJGo1uEgcRkyNMw",
        "e":"AQAB",
        "kty":"RSA",
        "alg":"RS256",
        "n":"qcByQdQX3xXnvjdZwJfiCh87KNVSDdxD6wOuUlfAod1d_1-nGFzFj8mlWVIdUTRuZAoD9urHU-t7M9K4Ytg7u6hfxrESXVlXRpfAxDEKbDK_xt1V9AKqN1E6vCKXIXvkZMYgdstjBejjC1-3IjwApvxEb8XVzpFWfI_dZc4pyk6E7VSJZGYYNxJuSXCLntkHrGgmaWp9BAbfq1S__WwufHAc6JvebGAYgCH0dihV-ueNqbS6a2lHJUqc88baCSwRrPjEDLzmOaZIsNmWvO-TJtqWdYUmdGK7OPMwnRs8s-v4WtwtHsSODklbYC8Q6E1Wmf268NlFlMTz8x9QeKP9j8P16vI8a2Dz4Uuli1YbkUnPoEqLI_asV_fE5eo4aydaTzJK_8CNm5XB0M8XoFqTQe72AOEhhkAdROa_5SOLWdBc61Bx4U0_evv1ER-vS52e8q_6qPNFntrGKYzKWnqoNmYnO117ksjnKTn9uFj88U4Akxwcxyql0a9vYK9p_IaVbxIesZ0EzAanUgWkiOpMyjJxeo8PxAO3n1O1sQ92EIEglYJ5qLzgegWKoZZLxRD_3jUoDNKbTeelqE2Rr9725VzrGa8VIv7te2ezz3AszfsGcU2emKVaZmfo043dy4KwYq-NmJgAt84ODoqgydqIAjHh4QU3vyDcRhLVqcZEjCU",
        "p":"0KxlUgbxuakt1gfWmr7SAKNJmH_SnsMkEMrw-AFWnAcBB9yB7JHjsgyhmkfrz2QmGfgQjit7u1GtkclWZmXY_Bcq9-mhulB2wNNxtLZgIXT_Y4UK6X1W1Y-qLmXu8RCzwxvtVpEvZPGk73dLVMbMJ9JI0K0MSAgsZUJo7OUGD-3QA8m1c-yq1zK26rmnvYlWTToFKCKis4_Ql0PfbG3S8etugnsCXkHSAt3JmAuZJ72HL76nyHjZv-Q23M4xCEDYOOGBjT1O_zfRpBtUT95iJXegk9qPabBB-ZghFRjRqL-1V1uluUTBNnb15mm2ER2UJu_7XCsUIgmEbulQTF0hTw",
        "q":"0EBA0XqE7K3My1VDDjgaWwQgqfdcG5aUKGgd1WVBf242odkTrH4gTymMIWTNhbUxLpsFoO3DUZPQW9emTr_qGR1g0z5L4mrSVMZRzNeDO80iAurLK8ft_d_0fOWEW-wnwRzY8Iog0hgd4QasZIRVXnDqUenVbif3bt48s_LkL0F0-xpAhZn2yTGAkFH6qfSQN3ZMWMGo8TKStPdJ9iBX_twjy7PFQLZ0Rq1apzyXKfwxuNWvzEYAeVhj-Hp6Rl_AATr76sY-gE2EiFjH4lkq_cAKHcuET1WOKxhDRUGMbtwOVPU1-9r091wr0fqmFAVSBkBraKxDrkbJA6VCGjEWSw",
        "qi":"QAznSkYlpX6Mp7Fq9-tmT1fKzpC45IfEwCjdHaJtki17er2_ch-B4knqOV20jZOhHHhEP_Dy7_ytia68qjexWdJI8roca7LHrUmdtKZO4md5fUCnC989285RMiNBllAK-0XACu0oK9coinbAVq3nstbxBknwe7qoNG-BkcqHAE5uX4dkJkNZQwTQfS_Cgj6lD6u36t-VJc3QH4LHv_RrNiMUBfm-S1jpDm_q_VUZ7U7jUu7Ei5sS4ptkJk9uJreyytOWzXbT_WxpIHQkPOSvi1Px0zZAV4oCqXyVOA1uwZCxWKN3aO_dLT4H45vJg1Bd0DD8TvEzYpCKzRxLniXb0A",
        "kid":"BE9A843ED0A783F7881977BA671A8533"
      }
      """;
  private static final URI AUTHORITY = URI.create("https://helseid-sts.test.nhn.no");

  public static void main(String[] args) throws HelseIdException {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("https://et.persontjenesten.test.nhn.no");

    var client = new Client(CLIENT_ID, JWKKeyReference.parse(JWK_PRIVATE_KEY), List.of(SCOPE));
    var clientCredentials = new ClientCredentials.Builder(AUTHORITY)
        .withClient(client)
        .build();


    TokenResponse tokenResponse = clientCredentials.getAccessToken();
    if (tokenResponse instanceof ErrorResponse errorResponse) {
      System.err.println("Failed to get token:");
      System.err.println(errorResponse.rawResponse());
    }

    if (tokenResponse instanceof AccessTokenResponse accessTokenResponse) {
      // Configure HTTP bearer authorization: HelseID
      HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
      HelseID.setBearerToken(accessTokenResponse.accessToken());

      EventWithLegalBasisApi apiInstance = new EventWithLegalBasisApi(defaultClient);
      String apiVersion = "1.5";
      try {
        EventDocument result = apiInstance.apiLegalBasisEventLatestGet(apiVersion);
        System.out.println(result);
      } catch (ApiException e) {
        System.err.println("Exception when calling EventNoLegalBasisApi#apiNoLegalBasisEventLatestGet");
        System.err.println("Status code: " + e.getCode());
        System.err.println("Reason: " + e.getResponseBody());
        System.err.println("Response headers: " + e.getResponseHeaders());
        e.printStackTrace();
      }
    }
  }
}
