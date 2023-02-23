package no.helse.kj.tokenexchange;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.nimbusds.oauth2.sdk.pkce.CodeChallenge;
import com.nimbusds.oauth2.sdk.pkce.CodeVerifier;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.URI;
import java.net.URL;
import java.net.http.HttpClient;
import java.net.http.HttpRequest.BodyPublishers;
import java.net.http.HttpResponse;
import java.time.Duration;
import java.time.Instant;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.atomic.AtomicReference;
import java.util.function.Supplier;

import static com.nimbusds.oauth2.sdk.pkce.CodeChallengeMethod.S256;
import static java.net.http.HttpRequest.BodyPublishers.ofString;
import static java.net.http.HttpRequest.newBuilder;
import static java.net.http.HttpResponse.BodyHandlers.ofInputStream;
import static java.time.Instant.now;
import static java.time.temporal.ChronoUnit.SECONDS;

/**
 *  This code is meant as a minimal example to show how to achieve SSO with kjernejournal (KJ).
 *  The user logs in with HelseID in their EPJ-system, and tokens from HelseID will be used as
 *  the basis for all further authorization/authenitcation done by KJ on behalf of the EPJ user.
 *  To achieve this, the EPJ posts Access tokens to KJ for as long as the user session is active.
 *  Since tokens will expire regularly, the EPJ will have to keep KJ updated with the latest valid token.
 *
 */
public class SSOIntegration {

  public record Tokens(String accessToken, Instant tokenExpiry) { }

  //These personal identifiers correspond to test-patients in the Kjernejournal test environment
  private static final String PATIENT_TRINE_FAKTOR = "12050050295";
  private static final String PATIENT_FOLKE_DANSER = "14019800513";

  //Here is an example user that can be used to log in to the Kjernejournal test environments.
  private static final String LEGE_GARD_TANDBERG = "05085600143";
  private static final HttpClient client = HttpClient.newBuilder().build();

  //URL for the service that keeps track of EPJ access tokens
  public static final String HELSEID_SSO_URL = "https://dev.portal.kj.nhn.no/hpp-webapp/helseid-sso";

  // st3 (systest 3) is one of the less stable test-environments, but it is where this new login functionality is available
  private static final String TICKET_URL = "https://api.st3.kjernejournal-test.no:8000/v1/helseindikator";
  private static final String KJ_WEB_URL = "https://st3.kjernejournal-test.no";

  private static final ObjectMapper mapper = new ObjectMapper();
  private static final AtomicReference<Tokens> epjAccessToken = new AtomicReference<>();

  private static final Supplier<String> systemAccessTokenFetcher = () -> {
    throw new RuntimeException("Add the HelseID system token in use for calling the ticket endpoint.");
  };

  private static final Supplier<Tokens> userAccessTokenFetcher = () -> {
    throw new RuntimeException("""
        Implement a function that fetches the current access token, along with its expiry.
        Examples of how to use the HelseID APIs for this are found elsewhere in this repository.
        """
    );
  };

  //Used in the call to the ticket endpoint
  private static final String epjName = "SSO-Example";

  //Http session in use between this app and the HelseID-SSO service - it is added programatically in this example code.
  private static String sessionId;

  public static void main(String[] args) throws Exception {

    //Start by getting the latest token
    epjAccessToken.set(userAccessTokenFetcher.get());

    //A background process that ensures KJ has a valid access token at all times
    var keepTokensUpdated = new Thread(() -> {
      try {
        //hent nye access tokens når det nåværende tokenet går ut
        while (!Thread.currentThread().isInterrupted()) {
          var currentTokens = epjAccessToken.get();

          var timeToGetNewToken = currentTokens.tokenExpiry().minus(5, SECONDS);
          var sleepDuration = Duration.between(now(), timeToGetNewToken).toMillis();

          Thread.sleep(sleepDuration);

          System.out.println("Fetching new access token");
          var newTokens = userAccessTokenFetcher.get();
          epjAccessToken.set(newTokens);
          postUpdatedTokenToKJ(newTokens.accessToken());
        }
      } catch (Exception e) {
        System.out.println("Refresh task failed! Shutting down");
        e.printStackTrace();
      }
    });
    keepTokensUpdated.setDaemon(true);
    keepTokensUpdated.start();

    final BufferedReader inputReader = new BufferedReader(new InputStreamReader(System.in));
    System.out.println("Enter patient identifier: ");
    String command = inputReader.readLine();
    while (!"q".equalsIgnoreCase(command)) {
      System.out.println("KJ URL: " + linkToKJ(command));
      command = inputReader.readLine();
    }
    end();

  }

  /**
   * Implementation calls the helseindikator-endpoint with the personal identifier supplied
   * and gets a "ticket".
   * This ticket get posted along with a hashed "nonce" value, with a valid Access token to the helseid-SSO service.
   * The SSO-service returns a one-time-code that, along with the nonce, can be used to gain access to the access token.
   * The URL to KJ returned from this function therefore includes the nonce and one-time-code.
   */
  public static URL linkToKJ(String fnr) throws Exception {
    var ticket = getTicket(fnr);

    var nonce = new CodeVerifier();
    var nonceSha = CodeChallenge.compute(S256, nonce);

    var createSessionRequest = mapper.writeValueAsString(new HashMap<>() {{
      put("nonce", nonceSha.getValue());
      put("ticket", ticket);
    }});

    var requestBuilder = newBuilder()
        .uri(URI.create(HELSEID_SSO_URL + "/api/Session/create"))
        .header("Authorization", "Bearer " + epjAccessToken.get().accessToken())
        .POST(BodyPublishers.ofString(createSessionRequest));

    //If we already have an ongoing session with the SSO-service - we use the same session.
    //This ensures that the previous value on the session gets deleted immediately (as opposed to when the token expires)
    if (sessionId != null) {
      requestBuilder.header("Cookie", sessionId);
    }

    var response = client.send(requestBuilder.build(), ofInputStream());
    var createdSSOSession = mapper.readValue(response.body(), new TypeReference<HashMap<String, String>>() {});

    //If this is the first call to the SSO-service, we keep the session id for further calls.
    if (sessionId == null) sessionId = response.headers().firstValue("Set-Cookie").orElse(null);

    return new URL(KJ_WEB_URL + "/hpp-webapp/" +
        "hentpasient.html?" +
        "otc=" + createdSSOSession.get("code") +
        "&nonce=" + nonce.getValue());
  }

  private static void postUpdatedTokenToKJ(String token) throws Exception {
    //Can't call refresh, unless we have an ongoing session
    if (sessionId == null) return;

    client.send(newBuilder()
        .header("Cookie", sessionId)
        .uri(URI.create(HELSEID_SSO_URL + "/api/Session/refresh"))
        .header("Authorization", "Bearer " + token)
        .POST(BodyPublishers.noBody())
        .build(), ofInputStream());
  }

  private static void end() throws Exception {
    //If there is no session, there is nothing to end
    if (sessionId == null) return;

    client.send(newBuilder()
        .header("Cookie", sessionId)
        .uri(URI.create(HELSEID_SSO_URL + "/api/Session/end"))
        .header("Authorization", "Bearer " + epjAccessToken.get())
        .POST(BodyPublishers.noBody())
        .build(), ofInputStream());
  }

  public static String getTicket(String fnr) throws Exception {
    var ticketRequest = mapper.writeValueAsString(new HashMap<>() {{
      put("fnr", fnr);
      put("samtykke", "HPAKUTT");
    }});

    var response = client.send(newBuilder()
            .header("Authorization", "Bearer " + systemAccessTokenFetcher.get())
            .header("X-EPJ-System", epjName)
            .header("Content-Type", "application/json;charset=utf-8")
            .uri(URI.create(TICKET_URL))
            .POST(ofString(ticketRequest))
            .build(), HttpResponse.BodyHandlers.ofString())
        .body();
    return (String) mapper.readValue(response, new TypeReference<Map<String, Object>>() {}).get("ticket");
  }

}
