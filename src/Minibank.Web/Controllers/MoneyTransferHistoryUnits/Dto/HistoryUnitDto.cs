using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Web.Controllers.MoneyTransferHistoryUnits.Dto
{
    public class HistoryUnitDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
    }
}
