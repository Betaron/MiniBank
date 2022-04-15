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

        public async Task<BankAccount> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

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

        public async Task<IEnumerable<BankAccount>> GetByUserIdAsync(
            Guid userId, CancellationToken cancellationToken)
        {
            var data = (_context.Accounts.AsNoTracking()
                .Where(it => it.UserId == userId)
                .Select(it => new BankAccount
                {
                    Id = it.Id,
                    UserId = it.UserId,
                    AccountBalance = it.AccountBalance,
                    Currency = it.Currency,
                    IsActive = it.IsActive,
                    OpeningDate = it.OpeningDate,
                    ClosingDate = it.ClosingDate
                }));

            return await data.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BankAccount>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var data = _context.Accounts.AsNoTracking()
                .Select(it => new BankAccount()
            {
                Id = it.Id,
                UserId = it.UserId,
                AccountBalance = it.AccountBalance,
                Currency = it.Currency,
                IsActive = it.IsActive,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            });

            return await data.ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(BankAccount account, CancellationToken cancellationToken)
        {
            var entity = new BankAccountDbModel
            {
                Id = Guid.NewGuid(),
                UserId = account.UserId,
                AccountBalance = 0,
                Currency = account.Currency,
                IsActive = true,
                OpeningDate = DateTime.UtcNow,
                ClosingDate = null
            };

            await _context.Accounts.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(BankAccount account, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => 
                    it.Id == account.Id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {account.Id} не найден");
            }

            entity.UserId = account.UserId;
            entity.Currency = account.Currency;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => 
                    it.Id == id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            _context.Accounts.Remove(entity);
        }

        public async Task CloseAccountAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => 
                    it.Id == id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            entity.IsActive = false;
            entity.ClosingDate = DateTime.UtcNow;
        }

        public async Task UpdateBalanceAsync(
            Guid id, double amount, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => 
                    it.Id == id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Аккаунт с id {id} не найден");
            }

            entity.AccountBalance = amount;
        }

        public Task<bool> ExistsByUserIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Accounts.AnyAsync(it => it.UserId == id, cancellationToken);
        }
    }
}
