namespace TestTokenTool.Options;

internal static class StringExtensions
{
    public static string GetEmptyStringIfNotSet(this string? aString)
    {
        return aString ?? string.Empty;
    }

    public static List<string> GetListWithParameter(this string? aString)
    {
        return !string.IsNullOrEmpty(aString) ? new List<string>() { aString } : new List<string>();
    }
    
    public static List<string> GetListWithMultipleParameters(this string? aString)
    {
        return string.IsNullOrEmpty(aString) ?
            new List<string>() :
            aString.Split(' ').ToList();
    }
}