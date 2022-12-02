package sf.nhn.helseid.demo.token;

import com.google.gson.annotations.SerializedName;

public class TokenResponse {

    @SerializedName("access_token")
    public String accessToken;

    @SerializedName("expires_in")
    public int expiresIn;

    @SerializedName("token_type")
    public String tokenType;

    public String scope;
}
