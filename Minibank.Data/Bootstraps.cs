using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core;

namespace Minibank.Data
{
    public static class Bootstraps
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddHttpClient<ICurrencyHttpProvider, CurrencyHttpProvider>(options =>
                {
                    options.BaseAddress = new Uri(configuration["CbrDaily"]);
                });

                return services;
            }
    }
}
