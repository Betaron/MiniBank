using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Web.Controllers.BankAccounts.Dto
{
    public class BankAccountDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public double AccountBalance { get; set; }
        public CurrencyType Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
