using Microsoft.AspNetCore.Http;
using Platform.BuildingBlocks.Abstractions;
using Platform.SystemContext.Abstractions;
using System.Security.Claims;
using System.Linq;

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
        public IReadOnlyCollection<string> Roles =>
            User?.FindAll(ClaimTypes.Role).Select(x => x.Value)
                .Concat(User?.FindAll("role").Select(x => x.Value) ?? [])
                .Concat(User?.FindAll("roles").Select(x => x.Value) ?? [])
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray()
            ?? [];
        public bool IsInRole(string role) => Roles.Any(x => string.Equals(x, role, StringComparison.OrdinalIgnoreCase));
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;
        
    }
}
