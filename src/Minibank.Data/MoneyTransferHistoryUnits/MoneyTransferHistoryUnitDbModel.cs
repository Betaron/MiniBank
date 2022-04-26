using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Data.BankAccounts;

namespace Minibank.Data.MoneyTransferHistoryUnits
{
    public class MoneyTransferHistoryUnitDbModel
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }

        /* navigation properties */
        public BankAccountDbModel FromAccount { get; set; }
        public BankAccountDbModel ToAccount { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<MoneyTransferHistoryUnitDbModel>
    {
        public void Configure(EntityTypeBuilder<MoneyTransferHistoryUnitDbModel> builder)
        {
            builder.ToTable("transactions_history");

            builder.HasKey(it => it.Id).HasName("pk_history_unit_id");

            builder.HasOne(it => it.FromAccount)
                .WithMany(it => it.TransactionsFrom)
                .HasForeignKey(it => it.FromAccountId);

            builder.HasOne(it => it.ToAccount)
                .WithMany(it => it.TransactionsTo)
                .HasForeignKey(it => it.ToAccountId);
        }
    }
}
