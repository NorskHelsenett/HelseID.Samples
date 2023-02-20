using HelseId.Samples.Common.Interfaces.TokenExpiration;

namespace HelseId.Samples.Common.TokenExpiration;

public class ExpirationTimeCalculator : IExpirationTimeCalculator
{
    // This sets the skew for the token (i.e. seconds before the token has expired)
    private const int TokenTimeSkew = 30;

    private IDateTimeService _dateTimeService;

    public ExpirationTimeCalculator(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public DateTime CalculateTokenExpirationTimeUtc(int expiresIn)
    {
        // The response tells us the number of seconds until the token expires ("expires in").
        // We subtract the skew from this number, and convert this to an 'expires at' value, which serves as a time stamp
        return _dateTimeService.UtcNow.AddSeconds(expiresIn - TokenTimeSkew);
    }

    public bool ExpirationTimeHasPassed(DateTime expirationTimeUtc)
    {
        return (_dateTimeService.UtcNow > expirationTimeUtc);
    }
}