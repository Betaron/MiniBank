using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Core
{
    public interface ICurrencyHttpProvider
    {
        public Task<double> GetExchangeRateAsync(
            CurrencyType currencyCode, CancellationToken cancellationToken);
    }
}
