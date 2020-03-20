using System.Linq;
using HelseId.Core.BFF.Sample.WebCommon.Identity;
using Microsoft.AspNetCore.Http;

namespace HelseId.Core.BFF.Sample.Api.Services
{
    public interface ICurrentUser
    {
        string? Id { get; }
    }

    public class CurrentHttpUser: ICurrentUser
    {
        private readonly HttpContext _httpContext;

        public CurrentHttpUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public string? Id => _httpContext.User.Claims.FirstOrDefault(x => x.Type == IdentityClaims.Pid)?.Value;
    }
}
