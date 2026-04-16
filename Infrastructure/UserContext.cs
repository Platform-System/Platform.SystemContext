using Microsoft.AspNetCore.Http;
using Platform.BuildingBlocks.Abstractions;
using Platform.SystemContext.Abstractions;
using System.Security.Claims;

namespace Platform.SystemContext.Infrastructure
{
    public class UserContext : IUserContext, ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
        public Guid? UserId
        {
            get
            {
                var rawUserId = User?.FindFirst("sub")?.Value
                          ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return Guid.TryParse(rawUserId, out var id) ? id : null;
            }
        }
        public string? Email =>
            User?.FindFirst("email")?.Value
            ?? User?.FindFirst(ClaimTypes.Email)?.Value;
        public string? UserName =>
            User?.FindFirst("preferred_username")?.Value
            ?? User?.Identity?.Name;
        public string? CurrentUserId => UserId?.ToString();
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;
        
    }
}
