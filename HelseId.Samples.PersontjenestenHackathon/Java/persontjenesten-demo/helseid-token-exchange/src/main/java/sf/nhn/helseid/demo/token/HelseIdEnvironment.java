package sf.nhn.helseid.demo.token;

public enum HelseIdEnvironment {

    TEST("https://helseid-sts.test.nhn.no/connect/token");

    private final String tokenEndpoint;

    private HelseIdEnvironment(String tokenEndpoint) {
        this.tokenEndpoint = tokenEndpoint;
    }

    public String getTokenEndpoint() {
        return tokenEndpoint;
    }
}
