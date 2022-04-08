using Minibank.Core.Domains.BankAccounts.Enums;

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
        Task<double> ConvertCurrencyAsync(double amount, CurrencyType fromCurrency, CurrencyType toCurrency);
    }
}
