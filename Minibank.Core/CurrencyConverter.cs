using Minibank.Core.Utility;

namespace Minibank.Core
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ICurrencyData _currencyData;

        public CurrencyConverter(ICurrencyData currencyData)
        {
            _currencyData = currencyData;
        }

        public double ConvertCurrency(int amount, string code)
        {
            if (amount < 0)
                throw new UserFriendlyException(userFriendlyMessage: "Сумма в целевой валюте отрицательна");

            return amount / _currencyData.GetExchangeRate(code);
        }
    }
}
