namespace Minibank.Core
{
    public interface ICurrencyConverter
    {
        /// <summary>
        /// Convert a currency amount to another currency
        /// </summary>
        /// <param name="amount">Currency amount in rubles</param>
        /// <param name="code">Currency code, like USD, EUR (Now is not available)</param>
        double ConvertCurrency(int amount, string code);
    }
}
