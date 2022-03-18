using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Data.BankAccounts
{
    internal class BankAccountDbModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public double AccountBalance { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
