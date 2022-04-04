using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly MinibankContext _context;

        public BankAccountRepository(MinibankContext context)
        {
            _context = context;
        }

        public BankAccount GetById(string id)
        {
            var entity = _context.Accounts.AsNoTracking()
                .FirstOrDefault(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            return new BankAccount
            {
                Id = entity.Id.ToString(),
                UserId = entity.UserId.ToString(),
                AccountBalance = entity.AccountBalance,
                Currency = entity.Currency,
                IsActive = entity.IsActive,
                OpeningDate = entity.OpeningDate,
                ClosingDate = entity.ClosingDate
            };
        }

        public IEnumerable<BankAccount> GetByUserId(string userId)
        {
            return _context.Accounts.AsNoTracking().Where(it => it.UserId.ToString() == userId)
                .Select(it => new BankAccount
            {
                Id = it.Id.ToString(),
                UserId = it.UserId.ToString(),
                AccountBalance = it.AccountBalance,
                Currency = it.Currency,
                IsActive = it.IsActive,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            });
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return _context.Accounts.AsNoTracking().Select(it => new BankAccount()
            {
                Id = it.Id.ToString(),
                UserId = it.UserId.ToString(),
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
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(account.UserId),
                AccountBalance = 0,
                Currency = account.Currency,
                IsActive = true,
                OpeningDate = DateTime.UtcNow,
                ClosingDate = null
            };

            _context.Accounts.Add(entity);

            _context.SaveChanges();
        }

        public void Update(BankAccount account)
        {
            var entity = _context.Accounts
                .FirstOrDefault(it => it.Id.ToString() == account.Id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {account.Id} не найден");
            }

            entity.UserId = Guid.Parse(account.UserId);
            entity.Currency = account.Currency;

            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var entity = _context.Accounts.FirstOrDefault(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            _context.Accounts.Remove(entity);

            _context.SaveChanges();
        }

        public void CloseAccount(string id)
        {
            var entity = _context.Accounts
                .FirstOrDefault(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            entity.IsActive = false;
            entity.ClosingDate = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void UpdateBalance(string id, double amount)
        {
            var entity = _context.Accounts
                .FirstOrDefault(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            entity.AccountBalance = amount;

            _context.SaveChanges();
        }
    }
}
