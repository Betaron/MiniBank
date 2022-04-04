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

        public MoneyTransferHistoryUnit GetById(string id)
        {
            var entity = _context.HistoryUnits.AsNoTracking()
                .FirstOrDefault(it => it.Id.ToString() == id);

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

        public IEnumerable<MoneyTransferHistoryUnit> GetAll()
        {
            return _context.HistoryUnits.AsNoTracking()
                .Select(it => new MoneyTransferHistoryUnit
            {
                Id = it.Id.ToString(),
                Amount = it.Amount,
                Currency = it.Currency,
                FromAccountId = it.FromAccountId.ToString(),
                ToAccountId = it.ToAccountId.ToString()
            });
        }

        public void Create(MoneyTransferHistoryUnit unit)
        {
            var entity = new MoneyTransferHistoryUnitDbModel()
            {
                Id = Guid.NewGuid(),
                Amount = unit.Amount,
                Currency = unit.Currency,
                FromAccountId = Guid.Parse(unit.FromAccountId),
                ToAccountId = Guid.Parse(unit.ToAccountId)
            };

            _context.HistoryUnits.Add(entity);
        }

        public void Update(MoneyTransferHistoryUnit unit)
        {
            var entity = _context.HistoryUnits
                .FirstOrDefault(it => it.Id.ToString() == unit.Id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Запись в истории с id {unit.Id} не найдена");
            }

            entity.Amount = unit.Amount;
            entity.Currency = unit.Currency;
            entity.FromAccountId = Guid.Parse(unit.FromAccountId);
            entity.ToAccountId = Guid.Parse(unit.ToAccountId);
        }

        public void Delete(string id)
        {
            var entity = _context.HistoryUnits
                .FirstOrDefault(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Запись в истории с id {id} не найдена");
            }

            _context.HistoryUnits.Remove(entity);
        }
    }
}
