using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Domains.MoneyTransferHistoryUnits;
using Minibank.Core.Domains.Users;

namespace Minibank.Core.Domains.BankAccounts
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public double AccountBalance { get; set; }
        public CurrencyType Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }

        /* navigation properties */
        public User User { get; set; }
        public IEnumerable<MoneyTransferHistoryUnit>? HistoryUnits { get; set; }
    }
}
