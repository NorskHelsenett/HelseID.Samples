using System.CommandLine;
using Microsoft.Extensions.Configuration;
using TestTokenTool.ApiCalls;
using TestTokenTool.Commands;
using TestTokenTool.Constants;
using TestTokenTool.RequestModel;
using TokenRequest = TestTokenTool.RequestModel.TokenRequest;

namespace TestTokenTool;
public class Parameters
{
    public bool PrintJwt { get; set; }

    public bool PrettyPrintJwt { get; set; }

    public bool SaveTokenToFile { get; set; }

    public bool CallApi { get; set; }
}

static class Program
{
    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: false);

        var printJwtOption = new Option<bool>(
            aliases: new [] { "--printToken", "-pt"},
            description: "Det returnerte test-tokenet blir skrevet til standard utput",
            getDefaultValue: () => true);

        var prettyPrintJwtOption = new Option<bool>(
            aliases: new [] { "--prettyPrintToken", "-ppt"},
            description: "The returned token is printed in JSON format on screen",
            getDefaultValue: () => true);
        
        var saveTokenToFileOption = new Option<bool>(
            aliases: new [] { "--saveTokenToFile", "-st"},
            description: $"The returned token is saved to a file named '{FileConstants.TokenFileName}'",
            getDefaultValue: () => false);

        var setInvalidIssuerOption = new Option<bool>(
            aliases: new [] { "--setInvalidIssuer", "-ii"},
            description: "The returned token will have an invalid 'iss' claim",
            getDefaultValue: () => false);

        var setInvalidAudienceOption = new Option<bool>(
            aliases: new [] { "--setInvalidAudience", "-ia"},
            description: "The returned token will have an invalid 'aud' claim",
            getDefaultValue: () => false);
        
        var setPidPseudonymOption = new Option<bool>(
            aliases: new [] { "--setPidPseudonym", "-pp"},
            description: "The returned token will have a claim with a pseudonymized pid value. In order to use this parameter, you must also set a 'pid' parameter.",
            getDefaultValue: () => false);

        var pidValueOption = new Option<string>(
            aliases: new [] { "--pidValue", "-p"},
            description: "The returned token will have a 'pid' claim matching the injected value",
            getDefaultValue: () => string.Empty);

        var generalClaimsParametersOption = new Option<ParametersGeneration>(
            aliases: new [] { "--generalClaimsParameters", "-gcp"},
            description: "The way to set general claims parameters",
            getDefaultValue: () => ParametersGeneration.GenerateNone);
        
        var userClaimsParametersOption = new Option<ParametersGeneration>(
            aliases: new [] { "--userClaimsParameters", "-ucp"},
            description: "The way to set user claims parameters",
            getDefaultValue: () => ParametersGeneration.GenerateNone);
        /*        
        var fileOption = new Option<FileInfo?>(
            name: "--file",
            description: "The file to read and display on the console.");

        var delayOption = new Option<int>(
            name: "--delay",
            description: "Delay between lines, specified as milliseconds per character in a line.",
            getDefaultValue: () => 42);

        var lightModeOption = new Option<bool>(
            name: "--light-mode",
            description: "Background color of text displayed on the console: default is black, light mode is white.");
*/
        var rootCommand = new RootCommand("Tool for accessing the HelseID test token service");
                
        rootCommand.AddOption(printJwtOption);
        rootCommand.AddOption(prettyPrintJwtOption);
        rootCommand.AddOption(saveTokenToFileOption);
        rootCommand.AddOption(setInvalidIssuerOption);
        rootCommand.AddOption(setInvalidAudienceOption);
        rootCommand.AddOption(setPidPseudonymOption);
        rootCommand.AddOption(pidValueOption);
        rootCommand.AddOption(generalClaimsParametersOption);
        rootCommand.AddOption(userClaimsParametersOption);
        

        var getTokenCommand = new Command("get_token", "Get a token from the test token service")
        {
            printJwtOption,
            prettyPrintJwtOption,
            saveTokenToFileOption,
            setInvalidIssuerOption,
            setInvalidAudienceOption,
            setPidPseudonymOption,
            
            pidValueOption,
            generalClaimsParametersOption,
            userClaimsParametersOption
        };
        
        rootCommand.AddCommand(getTokenCommand);

        getTokenCommand.SetHandler(
            async (
                printJwt,
                prettyPrintJwt,
                saveTokenToFile,
                setInvalidIssuer,
                setInvalidAudience,
                setPidPseudonym,
                pidValue,
                generalClaimsParameters,
                userClaimsParameters) =>
            {

                if (!File.Exists(FileConstants.JwkFileName))
                {
                    Console.WriteLine("Ooops! The private key for usage with HelseID is missing. [Read the README.md file for how to create this key and use its public equivalent in HelseID Selvbetjening.]");
                    return;
                }
                
                var tokenRequest = new TokenRequest
                {
                    SetInvalidIssuer = setInvalidIssuer,
                    SetInvalidAudience = setInvalidAudience,
                    SetPidPseudonym = setPidPseudonym,
                    GeneralParametersGeneration = generalClaimsParameters,
                    UserClaimsParametersGeneration = userClaimsParameters,
                    UserClaimsParameters = new UserClaimsParameters
                    {
                        Pid = pidValue
                    }
                };
                
                /*
                {
                    GeneralParametersGeneration =
                        ParametersGeneration.GenerateDefaultWithClaimsFromNonEmptyParameterValues,
                    UserClaimsParametersGeneration = ParametersGeneration.GenerateOnlyFromNonEmptyParameterValues,
                    SetInvalidIssuer = false,
                    SetInvalidAudience = false,
                    SignJwtWithInvalidSigningKey = false,
                    GetPersonFromPersontjenesten = true,
                    SetPidPseudonym = false,
                    GetHprNumberFromHprregisteret = true,
                    UserClaimsParameters = new UserClaimsParameters
                    {
                        // 05709992424 OVERTENKT LAMPEFOT
                        Pid = "11845799077", // "03678826129", //"43918802593",
                        Network = "foo",
                        //HprNumber = "12332123",
                    },
                    GeneralParameters = new GeneralParameters(),
                };
                */
                
                var tokenResponse = await TokenRetriever.GetToken(builder, tokenRequest);

                var parameters = new Parameters()
                {
                    PrintJwt = printJwt,
                    PrettyPrintJwt = prettyPrintJwt,
                    SaveTokenToFile = saveTokenToFile,
                };
                TokenPrinter.WriteResponse(tokenResponse, parameters);
                IApiCaller apiCaller = new ApiCaller();
                await apiCaller.CallApi(tokenResponse, parameters);
            },
            printJwtOption,
            prettyPrintJwtOption,
            saveTokenToFileOption,
            setInvalidIssuerOption,
            setInvalidAudienceOption,
            setPidPseudonymOption,
            pidValueOption,
            generalClaimsParametersOption,
            userClaimsParametersOption);


        var createJwkCommand = new Command("create_jwk", "Create a new jwk key for use with HelseID");
        
        createJwkCommand.SetHandler(async () =>
        {
            await JwkGenerator.GenerateKey();
        });

        rootCommand.AddCommand(createJwkCommand);

        await rootCommand.InvokeAsync(args);
    }
}