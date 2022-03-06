namespace Minibank.Core
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ICurrencyData _currencyData;

        public CurrencyConverter(ICurrencyData currencyData)
        {
            _currencyData = currencyData;
        }

        public int ConvertCurrency(int amount, string code)
        {
            return amount * _currencyData.GetExchangeRate(code);
        }
    }
}
