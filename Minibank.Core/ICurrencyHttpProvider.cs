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
        public double GetExchangeRate(CurrencyType currencyCode);
    }
}
