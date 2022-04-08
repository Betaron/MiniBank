using System.Net.Http.Json;
using Minibank.Core;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Exceptions;
using Minibank.Data.HttpClients.Models;

namespace Minibank.Data
{
    public class CurrencyHttpProvider : ICurrencyHttpProvider
    {
        private readonly HttpClient _httpClient;

        public CurrencyHttpProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double> GetExchangeRateAsync(CurrencyType currencyCode)
        {
            if (currencyCode.ToString().Equals("RUB"))
            {
                return 1.0;
            }

            var response = await _httpClient.GetFromJsonAsync<CourseResponse>("daily_json.js");

            var currencyValidity = response?.Valute.ContainsKey(currencyCode.ToString()) ?? false;

            if (!currencyValidity)
            {
                throw new ValidationException(validationMessage: $"Курс [{currencyCode}] недоступен");
            }

            return response.Valute[currencyCode.ToString()].Value;
        }
    }
}
