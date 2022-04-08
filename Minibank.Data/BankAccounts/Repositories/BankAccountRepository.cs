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

        public async Task<BankAccount> GetByIdAsync(string id)
        {
            var entity = await _context.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id.ToString() == id);

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

        public async Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId)
        {
            var data = (_context.Accounts.AsNoTracking()
                .Where(it => it.UserId.ToString() == userId)
                .Select(it => new BankAccount
                {
                    Id = it.Id.ToString(),
                    UserId = it.UserId.ToString(),
                    AccountBalance = it.AccountBalance,
                    Currency = it.Currency,
                    IsActive = it.IsActive,
                    OpeningDate = it.OpeningDate,
                    ClosingDate = it.ClosingDate
                }));

            return await data.ToListAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetAllAsync()
        { 
            var data = _context.Accounts.AsNoTracking().Select(it => new BankAccount()
            {
                Id = it.Id.ToString(),
                UserId = it.UserId.ToString(),
                AccountBalance = it.AccountBalance,
                Currency = it.Currency,
                IsActive = it.IsActive,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            });

            return await data.ToListAsync();
        }

        public async Task CreateAsync(BankAccount account)
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

            await _context.Accounts.AddAsync(entity);
        }

        public async Task UpdateAsync(BankAccount account)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => it.Id.ToString() == account.Id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {account.Id} не найден");
            }

            entity.UserId = Guid.Parse(account.UserId);
            entity.Currency = account.Currency;
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            _context.Accounts.Remove(entity);
        }

        public async Task CloseAccountAsync(string id)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            entity.IsActive = false;
            entity.ClosingDate = DateTime.UtcNow;
        }

        public async Task UpdateBalanceAsync(string id, double amount)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            entity.AccountBalance = amount;
        }
    }
}
