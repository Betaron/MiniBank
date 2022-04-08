using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Exceptions;

namespace Minibank.Core
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ICurrencyHttpProvider _currencyData;

        public CurrencyConverter(ICurrencyHttpProvider currencyData)
        {
            _currencyData = currencyData;
        }

        public async Task<double> ConvertCurrencyAsync(double amount, CurrencyType fromCurrency, CurrencyType toCurrency)
        {
            if (amount < 0)
                throw new ValidationException(validationMessage: "Передано отрицательное количество");

            var fromExchangeRate = await _currencyData.GetExchangeRateAsync(fromCurrency);
            var toExchangeRate = await _currencyData.GetExchangeRateAsync(toCurrency);

            return amount * fromExchangeRate / toExchangeRate;
        }
    }
}
