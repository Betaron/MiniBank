using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Data.BankAccounts;

namespace Minibank.Data.MoneyTransferHistoryUnits
{
    public class MoneyTransferHistoryUnitDbModel
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }

        /* navigation properties */
        public List<BankAccountDbModel>? Accounts { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<MoneyTransferHistoryUnitDbModel>
    {
        public void Configure(EntityTypeBuilder<MoneyTransferHistoryUnitDbModel> builder)
        {
            builder.ToTable("transactions_history");

            builder.Property(it => it.Id)
                .HasColumnName("id");
            builder.Property(it => it.Amount)
                .HasColumnName("amount");
            builder.Property(it => it.Currency)
                .HasColumnName("currency");
            builder.Property(it => it.FromAccountId)
                .HasColumnName("from_account_id");
            builder.Property(it => it.ToAccountId)
                .HasColumnName("to_account_id");

            builder.HasKey(it => it.Id).HasName("pk_history_unit_id");

            builder.HasMany(it => it.Accounts)
                .WithMany(it => it.HistoryUnits)
                .UsingEntity(it => it.ToTable("history_accounts"));
        }
    }
}
