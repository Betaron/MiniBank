using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.MoneyTransferHistoryUnits;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.MoneyTransferHistoryUnits.Repositories
{
    public class MoneyTransferHistoryUnitRepository : IMoneyTransferHistoryUnitRepository
    {
        private readonly MinibankContext _context;

        public MoneyTransferHistoryUnitRepository(MinibankContext context)
        {
            _context = context;
        }

        public async Task<MoneyTransferHistoryUnit> GetByIdAsync(
            Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.HistoryUnits.AsNoTracking()
                .FirstOrDefaultAsync(it => 
                    it.Id == id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Запись в истории с id {id} не найдена");
            }

            return new MoneyTransferHistoryUnit
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Currency = entity.Currency,
                FromAccountId = entity.FromAccountId,
                ToAccountId = entity.ToAccountId
            };
        }

        public async Task<IEnumerable<MoneyTransferHistoryUnit>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var data = _context.HistoryUnits.AsNoTracking()
                .Select(it => new MoneyTransferHistoryUnit
            {
                Id = it.Id,
                Amount = it.Amount,
                Currency = it.Currency,
                FromAccountId = it.FromAccountId,
                ToAccountId = it.ToAccountId
            });

            return await data.ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(
            MoneyTransferHistoryUnit unit, CancellationToken cancellationToken)
        {
            var entity = new MoneyTransferHistoryUnitDbModel()
            {
                Id = Guid.NewGuid(),
                Amount = unit.Amount,
                Currency = unit.Currency,
                FromAccountId = unit.FromAccountId,
                ToAccountId = unit.ToAccountId
            };

            await _context.HistoryUnits.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(
            MoneyTransferHistoryUnit unit, CancellationToken cancellationToken)
        {
            var entity = await _context.HistoryUnits
                .FirstOrDefaultAsync(it => 
                    it.Id == unit.Id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException(
                    $"Запись в истории с id {unit.Id} не найдена");
            }

            entity.Amount = unit.Amount;
            entity.Currency = unit.Currency;
            entity.FromAccountId = unit.FromAccountId;
            entity.ToAccountId = unit.ToAccountId;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.HistoryUnits
                .FirstOrDefaultAsync(it => 
                    it.Id == id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Запись в истории с id {id} не найдена");
            }

            _context.HistoryUnits.Remove(entity);
        }
    }
}
