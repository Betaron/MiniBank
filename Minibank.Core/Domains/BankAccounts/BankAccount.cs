namespace Minibank.Core.Domains.BankAccounts
{
    public class BankAccount
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public double AccountBalance { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}
