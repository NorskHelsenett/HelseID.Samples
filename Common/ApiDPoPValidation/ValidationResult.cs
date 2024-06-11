namespace HelseId.Samples.Common.ApiDPoPValidation;

public class ValidationResult
{
    public static ValidationResult Error(string description)
    {
        return new ValidationResult
        {
            IsError = true,
            ErrorDescription = description,
        };
    }
    
    public static ValidationResult Success()
    {
        return new ValidationResult
        {
            IsError = false,
        };
    }

    public bool IsError { get; private init; }

    public string? ErrorDescription { get; private init; }
}