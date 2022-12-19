namespace HelseId.Samples.ApiAccess.Exceptions;

public class TokenResponseErrorException : Exception
{
    public TokenResponseErrorException(string message) : base(message) { }
}