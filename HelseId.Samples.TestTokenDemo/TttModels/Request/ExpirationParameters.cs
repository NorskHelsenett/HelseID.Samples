namespace HelseId.Samples.TestTokenDemo.TttModels.Request;

public class ExpirationParameters
{
    public bool SetExpirationTimeAsExpired { get; set; }

    public int? ExpirationTimeInSeconds { get; set; }

    public int? ExpirationTimeInDays { get; set; }
}
