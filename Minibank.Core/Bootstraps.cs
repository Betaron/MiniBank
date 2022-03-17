using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.Users.Services;

namespace Minibank.Core
{
    public static class Bootstraps
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverter, CurrencyConverter>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
