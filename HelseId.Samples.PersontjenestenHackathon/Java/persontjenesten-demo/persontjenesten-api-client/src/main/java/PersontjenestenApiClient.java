import com.google.gson.Gson;
import sf.nhn.helseid.demo.persontjenesten.ApiClient;
import sf.nhn.helseid.demo.persontjenesten.ApiException;
import sf.nhn.helseid.demo.persontjenesten.Configuration;
import sf.nhn.helseid.demo.persontjenesten.api.EventWithLegalBasisApi;
import sf.nhn.helseid.demo.persontjenesten.auth.HttpBearerAuth;
import sf.nhn.helseid.demo.persontjenesten.model.EventDocument;
import sf.nhn.helseid.demo.token.HelseIdTokenExchanger;

import java.net.http.HttpClient;

public class PersontjenestenApiClient {

    public static void main(String[] args) {
        ApiClient defaultClient = Configuration.getDefaultApiClient();
        defaultClient.setBasePath("https://et.persontjenesten.test.nhn.no");

        var httpClient = HttpClient.newHttpClient();
        var gson = new Gson();

        var exchanger = new HelseIdTokenExchanger(
                HelseIdConfiguration.CLIENT_ID,
                HelseIdConfiguration.TOKEN_ENDPOINT,
                HelseIdConfiguration.SCOPE,
                HelseIdConfiguration.JWK_PRIVATE_KEY,
                httpClient,
                gson);

        // Configure HTTP bearer authorization: HelseID
        HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
        HelseID.setBearerToken(exchanger.getAccessToken().accessToken);

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
