namespace Minibank.Core
{
    public interface ICurrencyConverter
    {
        /// <summary>
        /// Convert a currency amount to another currency. Currency format, like USD, EUR.
        /// </summary>
        /// <param name="amount">Currency amount in rubles</param>
        /// <param name="fromCurrency">Currency code to be transferred</param>
        /// <param name="toCurrency">Currency code to which you want to transfer</param>
        double ConvertCurrency(double amount, string fromCurrency, string toCurrency);
    }
}
