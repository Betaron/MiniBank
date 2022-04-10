using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Services;
using Minibank.Core.Domains.Users.Services;

namespace Minibank.Core
{
    public static class Bootstraps
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverter, CurrencyConverter>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IMoneyTransferHistoryUnitService, MoneyTransferHistoryUnitService>();

            services.AddFluentValidation().AddValidatorsFromAssembly(typeof(Bootstraps).Assembly);

            return services;
        }
    }
}
