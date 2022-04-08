using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Core
{
    public interface ICurrencyHttpProvider
    {
        public Task<double> GetExchangeRateAsync(CurrencyType currencyCode);
    }
}
