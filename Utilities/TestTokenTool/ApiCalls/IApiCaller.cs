using TestTokenTool.Configuration;
using TestTokenTool.ResponseModel;

namespace TestTokenTool.ApiCalls;

public interface IApiCaller
{
    Task CallApi(TokenResponse? tokenResponse, Parameters parameters);
}