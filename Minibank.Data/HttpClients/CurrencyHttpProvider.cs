using System.Net.Http.Json;
using Minibank.Core;
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

        public double GetExchangeRate(string currencyCode)
        {
            if (currencyCode.Equals("RUB"))
            {
                return 1.0;
            }

            var response = 
                _httpClient.GetFromJsonAsync<CourseResponse>("daily_json.js").
                    GetAwaiter().GetResult();

            if (!response.Valute.ContainsKey(currencyCode))
            {
                throw new ValidationException(validationMessage: $"Курс [{currencyCode}] недоступен");
            }

            return response.Valute[currencyCode].Value;
        }
    }
}
