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
            var total = amount * _currencyData.GetExchangeRate(code);

            if (total < 0)
                throw new Exception();

            return total;
        }
    }
}
