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

        public async Task<MoneyTransferHistoryUnit> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.HistoryUnits.AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id.ToString() == id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Запись в истории с id {id} не найдена");
            }

            return new MoneyTransferHistoryUnit
            {
                Id = entity.Id.ToString(),
                Amount = entity.Amount,
                Currency = entity.Currency,
                FromAccountId = entity.FromAccountId.ToString(),
                ToAccountId = entity.ToAccountId.ToString()
            };
        }

        public async Task<IEnumerable<MoneyTransferHistoryUnit>> GetAllAsync(CancellationToken cancellationToken)
        {
            var data = _context.HistoryUnits.AsNoTracking()
                .Select(it => new MoneyTransferHistoryUnit
            {
                Id = it.Id.ToString(),
                Amount = it.Amount,
                Currency = it.Currency,
                FromAccountId = it.FromAccountId.ToString(),
                ToAccountId = it.ToAccountId.ToString()
            });

            return await data.ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(MoneyTransferHistoryUnit unit, CancellationToken cancellationToken)
        {
            var entity = new MoneyTransferHistoryUnitDbModel()
            {
                Id = Guid.NewGuid(),
                Amount = unit.Amount,
                Currency = unit.Currency,
                FromAccountId = Guid.Parse(unit.FromAccountId),
                ToAccountId = Guid.Parse(unit.ToAccountId)
            };

            await _context.HistoryUnits.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(MoneyTransferHistoryUnit unit, CancellationToken cancellationToken)
        {
            var entity = await _context.HistoryUnits
                .FirstOrDefaultAsync(it => it.Id.ToString() == unit.Id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Запись в истории с id {unit.Id} не найдена");
            }

            entity.Amount = unit.Amount;
            entity.Currency = unit.Currency;
            entity.FromAccountId = Guid.Parse(unit.FromAccountId);
            entity.ToAccountId = Guid.Parse(unit.ToAccountId);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.HistoryUnits
                .FirstOrDefaultAsync(it => it.Id.ToString() == id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Запись в истории с id {id} не найдена");
            }

            _context.HistoryUnits.Remove(entity);
        }
    }
}
