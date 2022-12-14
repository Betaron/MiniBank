using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Data.MoneyTransferHistoryUnits;
using Minibank.Data.Users;

namespace Minibank.Data.BankAccounts
{
    public class BankAccountDbModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public double AccountBalance { get; set; }
        public CurrencyType Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }

        /* navigation properties */
        public UserDbModel User { get; set; }
        public List<MoneyTransferHistoryUnitDbModel> TransactionsFrom { get; set; }
        public List<MoneyTransferHistoryUnitDbModel> TransactionsTo { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<BankAccountDbModel>
    {
        public void Configure(EntityTypeBuilder<BankAccountDbModel> builder)
        {
            builder.ToTable("bank_accounts");

            builder.HasKey(it => it.Id).HasName("pk_bank_account_id");

            builder.HasOne(it => it.User)
                .WithMany(it => it.Accounts)
                .HasForeignKey(it => it.UserId);
        }
    }
}
