using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using TestTokenTool.Configuration;
using TestTokenTool.Constants;
using Microsoft.IdentityModel.JsonWebTokens;
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

            var dPoPProof = result.SuccessResponse.DPoPProof;

            if (!string.IsNullOrEmpty(dPoPProof))
            {
                if (parameters.PrintJwt)
                {
                    PrintDPoPPRoof(dPoPProof);
                }

                if (parameters.PrettyPrintJwt)
                {
                    PrettyPrintToken(dPoPProof, isDPoPProof:true);
                }
            }
        }
    }

    private static void PrintToken(string jwtInput)
    {
        Console.WriteLine(jwtInput);
    }

    private static void PrintDPoPPRoof(string jwtInput)
    {
        Console.WriteLine("DPoP proof:");
        Console.WriteLine(jwtInput);
    }
    
    private static void PrettyPrintToken(string jwtInput, bool isDPoPProof = false)
    {
        var previousColour = Console.ForegroundColor;
        var jsonWebTokenHandler = new JsonWebTokenHandler();
        var readableToken = jsonWebTokenHandler.CanReadToken(jwtInput);

        if (!readableToken)
        {
            Console.WriteLine("The token is not in a valid format.");
            return;
        }

        if (isDPoPProof)
        {
            Console.WriteLine("DPoP proof response in JSON format:");
        }
        else
        {
            Console.WriteLine("Token response in JSON format:");
        }

        var token = jsonWebTokenHandler.ReadToken(jwtInput) as JsonWebToken;
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(JsonPrettify(DecodeBase64DataFromJwt(token!.EncodedHeader)));
       
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(".");
        Console.Write(JsonPrettify(DecodeBase64DataFromJwt(token.EncodedPayload)));

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(".[Signature]");
        
        Console.ForegroundColor = previousColour;
        Console.WriteLine();
        Console.WriteLine();
    }

    private static string DecodeBase64DataFromJwt(string encodedString)
    {
        encodedString = encodedString.PadRight(encodedString.Length + (encodedString.Length * 3) % 4, '=');  // add padding
        var data = Convert.FromBase64String(encodedString);
        return Encoding.UTF8.GetString(data);
    }

    private static string JsonPrettify(string json)
    {
        using var jDoc = JsonDocument.Parse(json, new JsonDocumentOptions{ AllowTrailingCommas = true });
        var jsonSerializerOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
                WriteIndented = true,
            };
        return JsonSerializer.Serialize(jDoc, jsonSerializerOptions);
    }
    
    private static void SaveTokenToFile(string jwtInput)
    {
        File.WriteAllText(FileConstants.TokenFileName, jwtInput);
        Console.WriteLine($"Saved the token as '{FileConstants.TokenFileName}'.");
    } 
}