using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace HelseId.Core.BFF.Sample.Api;

public static class TokenValidation
{
    public static bool ValidateSingleAudience(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
    {
        if (audiences.Count() != 1)
        {
            Log.Warning("Rejecting JWT with not exactly one audience");
            return false;
        }

        var audience = audiences.Single();
        if (audience != validationParameters.ValidAudience)
        {
            Log.Warning("Rejecting JWT with invalid audience '{invalidAudience}'", audience);
            return false;
        }

        return true;
    }
}