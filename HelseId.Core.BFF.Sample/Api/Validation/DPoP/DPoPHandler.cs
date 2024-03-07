using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace HelseId.Core.BFF.Sample.Api.DPoP;

public static class DPoPHandler
{
    public static void Handle(JwtBearerOptions options)
    {
        options.Events ??= new JwtBearerEvents();

        options.Events.OnMessageReceived = context =>
        {
            // This ensures that the received access token is a DPoP token:
            if (context.Request.GetDPoPAccessToken(out var dPopToken))
            {
                context.Token = dPopToken;
            }
            else
            {
                // Do not accept a bearer token:
                context.Fail("Expected a valid DPoP token");
            }

            return Task.CompletedTask;
        };

        options.Events.OnTokenValidated = async tokenValidatedContext =>
        {
            var request = tokenValidatedContext.HttpContext.Request;

            if (!request.GetDPoPProof(out var dPopProof))
            {
                tokenValidatedContext.Fail("Missing DPoP proof");

                return;
            }

            if (!request.GetDPoPAccessToken(out var accessToken))
            {
                tokenValidatedContext.Fail("Missing access token");

                return;
            }

            var cnfClaimValue = tokenValidatedContext.Principal!.FindFirstValue(JwtClaimTypes.Confirmation);

            var data = new DPoPProofValidationState(request, dPopProof, accessToken, cnfClaimValue);

            var dPopProofValidator = tokenValidatedContext.HttpContext.RequestServices.GetRequiredService<DPoPProofValidator>();
            var validationResult = await dPopProofValidator.Validate(data);

            if (!validationResult.IsSuccess)
            {
                tokenValidatedContext.Fail(validationResult.ErrorMessage);
            }
        };
    }
}