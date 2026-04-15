using Microsoft.Extensions.DependencyInjection;
using Platform.SystemContext.Abstractions;
using Platform.SystemContext.Infrastructure;

namespace Platform.SystemContext.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSystemContext(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext, UserContext>();

            return services;
        }
    }
}
