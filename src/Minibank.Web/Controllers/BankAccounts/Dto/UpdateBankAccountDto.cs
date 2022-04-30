using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Web.Controllers.BankAccounts.Dto
{
    public class UpdateBankAccountDto
    {
        public Guid UserId { get; set; }
        public CurrencyType Currency { get; set; }
    }
}
