using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace HelseId.Core.BFF.Sample.Api.Authorization
{
    public interface IAccessTokenProvider
    {
        /// <summary>
        /// Provides a raw access token, i.e. a Base64 encoded JWT. 
        /// </summary>
        /// <returns></returns>
        /// <remarks>Note that an access token may not be available, depending on the context.</remarks>
        Task<string?> GetAccessToken();
        /// <summary>
        /// Provides an access token as a decoded <see cref="JwtSecurityToken"/>. N
        /// </summary>
        /// <returns></returns>
        /// <remarks>Note that an access token may not be available, depending on the context.</remarks>
        Task<JwtSecurityToken?> GetAccessTokenAsJwtSecurityToken();
    }
}
