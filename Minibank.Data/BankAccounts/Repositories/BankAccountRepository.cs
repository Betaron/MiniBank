using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users;
using Minibank.Core.Exceptions;
using Minibank.Data.Users.Repositories;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        internal static List<BankAccountDbModel> AccountsStorage { get;} = new();

        public BankAccount GetById(string id)
        {
            var entity = AccountsStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            return new BankAccount
            {
                Id = entity.Id,
                UserId = entity.UserId,
                AccountBalance = entity.AccountBalance,
                Currency = entity.Currency,
                IsActive = entity.IsActive,
                OpeningDate = entity.OpeningDate,
                ClosingDate = entity.ClosingDate
            };
        }

        public IEnumerable<BankAccount> GetByUserId(string userId)
        {
            return AccountsStorage.Where(it => it.UserId == userId)
                .Select(it => new BankAccount
            {
                Id = it.Id,
                UserId = it.UserId,
                AccountBalance = it.AccountBalance,
                Currency = it.Currency,
                IsActive = it.IsActive,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            });
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return AccountsStorage.Select(it => new BankAccount()
            {
                Id = it.Id,
                UserId = it.UserId,
                AccountBalance = it.AccountBalance,
                Currency = it.Currency,
                IsActive = it.IsActive,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            });
        }

        public void Create(BankAccount account)
        {
            var entity = new BankAccountDbModel
            {
                Id = Guid.NewGuid().ToString(),
                UserId = account.UserId,
                AccountBalance = 0,
                Currency = account.Currency,
                IsActive = true,
                OpeningDate = DateTime.Now,
                ClosingDate = null
            };

            AccountsStorage.Add(entity);
        }

        public void Update(BankAccount account)
        {
            var entity = AccountsStorage.FirstOrDefault(it => it.Id == account.Id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {account.Id} не найден");
            }

            entity.UserId = account.UserId;
            entity.Currency = account.Currency;
        }

        public void Delete(string id)
        {
            var entity = AccountsStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            AccountsStorage.Remove(entity);
        }

        public void CloseAccount(string id)
        {
            var entity = AccountsStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            entity.IsActive = false;
            entity.ClosingDate = DateTime.Now;
        }

        public void UpdateBalance(string id, double amount)
        {
            var entity = AccountsStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            entity.AccountBalance = amount;
        }
    }
}
