using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Data.BankAccounts;

namespace Minibank.Data.Users
{
    public class UserDbModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        /* navigation properties */
        public List<BankAccountDbModel>? Accounts { get; set; }

        internal class Map : IEntityTypeConfiguration<UserDbModel>
        {
            public void Configure(EntityTypeBuilder<UserDbModel> builder)
            {
                builder.ToTable("users");

                builder.Property(it => it.Id)
                    .HasColumnName("id");
                builder.Property(it => it.Login)
                    .HasColumnName("login");
                builder.Property(it => it.Email)
                    .HasColumnName("email");

                builder.HasKey(it => it.Id).HasName("pk_user_id");
            }
        }
    }
}
