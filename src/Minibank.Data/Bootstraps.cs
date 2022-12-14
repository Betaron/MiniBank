using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Data.BankAccounts.Repositories;
using Minibank.Data.MoneyTransferHistoryUnits.Repositories;
using Minibank.Data.Users.Repositories;

namespace Minibank.Data
{
    public static class Bootstraps
    {
        public static IServiceCollection AddData(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ICurrencyHttpProvider, CurrencyHttpProvider>(options =>
            {
                options.BaseAddress =
                    new Uri(configuration["ExternalServices:ExchangeRateService"]);
            });

            services.AddDbContext<MinibankContext>(options =>
            {
                options.UseNpgsql(configuration["Databases:PostgreSql"]);
                options.UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IMoneyTransferHistoryUnitRepository, MoneyTransferHistoryUnitRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
