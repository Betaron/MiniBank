namespace Minibank.Web.Controllers.BankAccounts.Dto
{
    public class NewBankAccountDto
    {
        public string UserId { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
