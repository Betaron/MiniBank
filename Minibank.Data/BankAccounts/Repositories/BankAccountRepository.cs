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
        private static readonly double CommissionPercent = 0.02;

        public BankAccount GetById(string id)
        {
            var entity = AccountsStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new NotFoundException();
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
            if (AccountsStorage.Count == 0)
            {
                throw new NotFoundException();
            }

            return AccountsStorage.Select(it => new BankAccount
            {
                Id = it.Id,
                UserId = it.UserId,
                AccountBalance = it.AccountBalance,
                Currency = it.Currency,
                IsActive = it.IsActive,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            }).Where(it => it.UserId == userId);
        }

        public IEnumerable<BankAccount> GetAll()
        {
            if (AccountsStorage.Count == 0)
            {
                throw new NotFoundException();
            }

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
            if (!UserRepository.UsersStorage.Exists(it => it.Id == account.UserId))
            {
                throw new NotFoundException();
            }

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
                throw new NotFoundException();
            }

            entity.UserId = account.UserId;
            entity.Currency = account.Currency;
        }

        public void Delete(string id)
        {
            var entity = AccountsStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            AccountsStorage.Remove(entity);
        }

        public void CloseAccount(string id)
        {
            var entity = AccountsStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            entity.IsActive = false;
            entity.ClosingDate = DateTime.Now;
        }

        public void UpdateBalance(string id, double amount)
        {
            var entity = AccountsStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            entity.AccountBalance = amount;
        }

        public double GetCommissionPercent()
        {
            return CommissionPercent;
        }
    }
}
