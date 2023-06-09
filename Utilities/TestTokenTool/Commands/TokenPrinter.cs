using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using TestTokenTool.Constants;
using TokenResponse = TestTokenTool.ResponseModel.TokenResponse;

namespace TestTokenTool.Commands;

internal static class TokenPrinter
{
    public static void WriteResponse(TokenResponse? result, Parameters parameters)
    {
        if (result == null)
        {
            return;
        }

        if (result.IsError)
        {
            Console.WriteLine("Error message: " + result.ErrorResponse!.ErrorMessage);
        }
        else
        {
            var jwtInput = result.SuccessResponse.AccessTokenJwt;

            if (parameters.PrintJwt)
            {
                PrintToken(jwtInput);
            }

            if (parameters.PrettyPrintJwt)
            {
                PrettyPrintToken(jwtInput);
            }

            if (parameters.SaveTokenToFile)
            {
                SaveTokenToFile(jwtInput);
            }
        }
    }

    private static void PrintToken(string jwtInput)
    {
        Console.WriteLine("Token response:");
        Console.WriteLine(jwtInput);
        Console.WriteLine();
    }

    private static void PrettyPrintToken(string jwtInput)
    {
        var previousColour = Console.ForegroundColor;
        var jwtHandler = new JwtSecurityTokenHandler();
        var readableToken = jwtHandler.CanReadToken(jwtInput);

        if(!readableToken)
        {
            Console.WriteLine("The token is not in a valid format.");
            return;
        }

        Console.WriteLine("Token response in JSON format:");

        var token = jwtHandler.ReadJwtToken(jwtInput);

        //Extract the headers of the JWT
        var headers = token.Header;
        var jwtHeader = "{";
        foreach (var h in headers)
        {
            jwtHeader += '"' + h.Key + "\":\"" + h.Value + "\",";
        }
        jwtHeader += "}";
      
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(JsonPrettify(jwtHeader));

        //Extract the payload of the JWT
        var claims = token.Claims;
        var jwtPayload = "{";
        foreach (Claim c in claims)
        {
            jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
        }
        jwtPayload += "}";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(".");
        Console.Write(JsonPrettify(jwtPayload));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(".[Signature]");
        
        Console.ForegroundColor = previousColour;
        Console.WriteLine();
        Console.WriteLine();
    }
    
    private static string JsonPrettify(string json)
    {
        using var jDoc = JsonDocument.Parse(json, new JsonDocumentOptions{ AllowTrailingCommas = true});
        return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
    }
    
    private static void SaveTokenToFile(string jwtInput)
    {
        File.WriteAllText(FileConstants.TokenFileName, jwtInput);
        Console.WriteLine($"Saved the token as '{FileConstants.TokenFileName}'.");
    } 
}