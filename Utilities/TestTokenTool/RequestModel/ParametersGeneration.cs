namespace TestTokenTool.RequestModel;

public enum ParametersGeneration
{
    GenerateNone = 0,
    GenerateOnlyDefault = 1,
    GenerateOnlyFromNonEmptyParameterValues = 2,
    GenerateDefaultWithClaimsFromNonEmptyParameterValues = 3,
}