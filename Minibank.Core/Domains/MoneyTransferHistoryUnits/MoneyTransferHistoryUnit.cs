using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Core.Domains.MoneyTransferHistoryUnits
{
    public class MoneyTransferHistoryUnit
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
