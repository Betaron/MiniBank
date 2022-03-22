using Microsoft.AspNetCore.Mvc;
using Minibank.Core;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyConverter _currencyConverter;

        public CurrencyController(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        [HttpGet]
        [Route("сonvert")]
        public IActionResult Convert(double amount, string fromCurrency, string toCurrency)
        {
            return Ok(_currencyConverter.ConvertCurrency(amount, fromCurrency, toCurrency));
        }
    }
}
