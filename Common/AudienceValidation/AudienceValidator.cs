using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.AudienceValidation;

public class AudienceValidatorForSingleAudience
{
    private readonly string _audience;

    public AudienceValidatorForSingleAudience(string audience)
    {
        _audience = audience;
    }

    public bool AudienceValidation(IEnumerable<string> audiences, SecurityToken securitytoken, TokenValidationParameters validationparameters)
    {
        var audiencesAsList = audiences.ToList();

        if (audiencesAsList.Count() != 1)
        {
            // We only validate one audience
            return false;
        }

        return audiencesAsList[0] == _audience;
    }
}
