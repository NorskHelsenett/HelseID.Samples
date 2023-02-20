using HelseId.Samples.Common.Interfaces.TokenExpiration;

namespace HelseId.Samples.Common.TokenExpiration;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}