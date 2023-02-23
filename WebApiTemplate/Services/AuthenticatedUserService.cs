using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WebApiTemplate.Api.Application.Constants;
using WebApiTemplate.Api.Application.Interfaces.Services;

namespace WebApiTemplate.Api.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor) => UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.Uid);

        public string UserId { get; }
    }
}
