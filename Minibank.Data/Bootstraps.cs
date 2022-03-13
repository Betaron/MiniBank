using Microsoft.Extensions.DependencyInjection;
using Minibank.Core;

namespace Minibank.Data
{
    public static class Bootstraps
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyData, CurrencyData>();

            return services;
        }
    }
}
