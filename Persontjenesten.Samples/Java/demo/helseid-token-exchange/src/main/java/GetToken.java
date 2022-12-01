import com.google.gson.Gson;
import sf.nhn.helseid.demo.token.HelseIdTokenExchanger;

import java.net.http.HttpClient;

public class GetToken {

    public static void main(String[] args) {
        var httpClient = HttpClient.newHttpClient();
        var gson = new Gson();

        var exchanger = new HelseIdTokenExchanger(
                HelseIdConfiguration.CLIENT_ID,
                HelseIdConfiguration.TOKEN_ENDPOINT,
                HelseIdConfiguration.SCOPE,
                HelseIdConfiguration.JWK_PRIVATE_KEY,
                httpClient,
                gson);


        var tokenResponse = exchanger.getAccessToken();
        System.out.println(tokenResponse.accessToken);
    }
}
