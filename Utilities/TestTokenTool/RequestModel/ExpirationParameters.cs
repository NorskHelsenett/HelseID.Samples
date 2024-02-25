namespace TestTokenTool.RequestModel;

public class ExpirationParameters
{
    public bool SetExpirationTimeAsExpired { get; set; }

    public int ExpirationTimeInSeconds { get; set; } = Int32.MinValue;

    public int ExpirationTimeInDays { get; set; } = Int32.MinValue;
}
