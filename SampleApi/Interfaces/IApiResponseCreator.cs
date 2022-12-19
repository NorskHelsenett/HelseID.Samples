using System.Security.Claims;
using HelseId.Samples.Common.Models;

namespace HelseId.SampleApi.Interfaces;

public interface IApiResponseCreator
{
    ApiResponse CreateApiResponse(List<Claim> claims, string apiName);
}