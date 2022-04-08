﻿using Microsoft.AspNetCore.Mvc;
using Minibank.Core;
using Minibank.Core.Domains.BankAccounts.Enums;

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
        public Task<double> Convert(double amount, CurrencyType fromCurrency, CurrencyType toCurrency)
        {
            return _currencyConverter.ConvertCurrencyAsync(amount, fromCurrency, toCurrency);
        }
    }
}
