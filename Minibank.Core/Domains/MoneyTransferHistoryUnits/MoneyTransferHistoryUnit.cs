using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Core.Domains.MoneyTransferHistoryUnits
{
    public class MoneyTransferHistoryUnit
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }

        /* navigation properties */
        public IEnumerable<BankAccount>? BasAccounts { get; set; }
    }
}
