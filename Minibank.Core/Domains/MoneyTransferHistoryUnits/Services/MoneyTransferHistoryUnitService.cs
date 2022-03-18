using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;

namespace Minibank.Core.Domains.MoneyTransferHistoryUnits.Services
{
    public class MoneyTransferHistoryUnitService : IMoneyTransferHistoryUnitService
    {
        private readonly IMoneyTransferHistoryUnitRepository _historyRepository;

        public MoneyTransferHistoryUnitService(IMoneyTransferHistoryUnitRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public MoneyTransferHistoryUnit GetById(string id)
        {
            return _historyRepository.GetById(id);
        }

        public IEnumerable<MoneyTransferHistoryUnit> GetAll()
        {
            return _historyRepository.GetAll();
        }

        public void Create(MoneyTransferHistoryUnit unit)
        {
            _historyRepository.Create(unit);
        }

        public void Update(MoneyTransferHistoryUnit unit)
        {
            _historyRepository.Update(unit);
        }

        public void Delete(string id)
        {
            _historyRepository.Delete(id);
        }
    }
}
