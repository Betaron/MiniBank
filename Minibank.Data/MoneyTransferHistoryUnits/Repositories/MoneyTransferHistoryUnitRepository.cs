using Minibank.Core.Domains.MoneyTransferHistoryUnits;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.MoneyTransferHistoryUnits.Repositories
{
    public class MoneyTransferHistoryUnitRepository : IMoneyTransferHistoryUnitRepository
    {
        internal static List<MoneyTransferHistoryUnitDbModel> HistoryUnitStorage { get; } = new();

        public MoneyTransferHistoryUnit GetById(string id)
        {
            var entity = HistoryUnitStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new NotFoundException();
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

        public IEnumerable<MoneyTransferHistoryUnit> GetAll()
        {
            if (HistoryUnitStorage.Count == 0)
            {
                throw new NotFoundException();
            }

            return HistoryUnitStorage.Select(it => new MoneyTransferHistoryUnit
            {
                Id = it.Id,
                Amount = it.Amount,
                Currency = it.Currency,
                FromAccountId = it.FromAccountId,
                ToAccountId = it.ToAccountId
            });
        }

        public void Create(MoneyTransferHistoryUnit unit)
        {
            var entity = new MoneyTransferHistoryUnitDbModel()
            {
                Id = Guid.NewGuid().ToString(),
                Amount = unit.Amount,
                Currency = unit.Currency,
                FromAccountId = unit.FromAccountId,
                ToAccountId = unit.ToAccountId
            };

            HistoryUnitStorage.Add(entity);
        }

        public void Update(MoneyTransferHistoryUnit unit)
        {
            var entity = HistoryUnitStorage.FirstOrDefault(it => it.Id == unit.Id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            entity.Amount = unit.Amount;
            entity.Currency = unit.Currency;
            entity.FromAccountId = unit.FromAccountId;
            entity.ToAccountId = unit.ToAccountId;
        }

        public void Delete(string id)
        {
            var entity = HistoryUnitStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            HistoryUnitStorage.Remove(entity);
        }
    }
}
