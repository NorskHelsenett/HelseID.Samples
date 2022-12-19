namespace HelseId.Samples.Common.Interfaces.TokenExpiration;

public interface IExpirationTimeCalculator
{
    DateTime CalculateTokenExpirationTimeUtc(int expiresIn);

    bool ExpirationTimeHasPassed(DateTime expirationTimeUtc);
}