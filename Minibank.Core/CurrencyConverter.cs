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

        public double ConvertCurrency(double amount, CurrencyType fromCurrency, CurrencyType toCurrency)
        {
            if (amount < 0)
                throw new ValidationException(validationMessage: "Передано отрицательное количество");

            return amount * _currencyData.GetExchangeRate(fromCurrency) /
                   _currencyData.GetExchangeRate(toCurrency);
        }
    }
}
