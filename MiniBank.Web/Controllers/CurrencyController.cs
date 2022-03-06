﻿using Microsoft.AspNetCore.Mvc;
using Minibank.Core;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyConverter _currencyConverter;

        public CurrencyController(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        [HttpGet]
        [Route("convert")]
        public IActionResult Convert(int amount, string code)
        {
            return Ok(_currencyConverter.ConvertCurrency(amount, code));
        }
    }
}
