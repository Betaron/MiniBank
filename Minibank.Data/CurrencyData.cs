using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core;

namespace Minibank.Data
{
    public class CurrencyData : ICurrencyData
    {
        private readonly Random _randomValue = new();
        public int GetExchangeRate(string currencyCode)
        {
            return _randomValue.Next(0, 1000);
        }
    }
}
