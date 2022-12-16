using IdentityModel.Client;

namespace HelseID.Samples.Extensions;

public static class Extensions
{
    public static void WriteAccessTokenFromTokenResult(this TokenResponse tokenResponse) {
        Console.WriteLine("Returned access token:");
        Console.WriteLine(tokenResponse.AccessToken);
        Console.WriteLine("Copy/paste the access token at https://jwt.ms to see the contents");
    }

    public static async Task WriteErrorToConsole(this TokenResponse tokenResponse) {
        await Console.Error.WriteLineAsync("An error occured:");
        await Console.Error.WriteLineAsync(tokenResponse.Error);
    }
}
