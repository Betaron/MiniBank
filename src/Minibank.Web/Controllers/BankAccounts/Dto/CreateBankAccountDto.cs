using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Web.Controllers.BankAccounts.Dto
{
    public class CreateBankAccountDto
    {
        public Guid UserId { get; set; }
        public CurrencyType Currency { get; set; }
    }
}
