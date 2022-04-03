using Minibank.Core.Domains.BankAccounts;

namespace Minibank.Core.Domains.Users
{
    public class User
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        /* navigation properties */
        public IEnumerable<BankAccount>? Accounts { get; set; }
    }
}
