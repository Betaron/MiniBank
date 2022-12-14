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

                builder.HasKey(it => it.Id).HasName("pk_user_id");
            }
        }
    }
}
