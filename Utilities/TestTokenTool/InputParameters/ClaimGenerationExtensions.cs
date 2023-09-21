using TestTokenTool.RequestModel;

namespace TestTokenTool.InputParameters;

public static class ClaimGenerationExtensions
{
    public static ParametersGeneration ToParametersGeneration(this ClaimGeneration claimGeneration)
    {
        return claimGeneration switch
        {
            ClaimGeneration.Default => ParametersGeneration.GenerateOnlyDefault,
            ClaimGeneration.ParameterValues => ParametersGeneration.GenerateOnlyFromNonEmptyParameterValues,
            ClaimGeneration.DefaultWithParameterValues => ParametersGeneration.GenerateDefaultWithClaimsFromNonEmptyParameterValues,
            _ => ParametersGeneration.GenerateNone
        };
    }
}