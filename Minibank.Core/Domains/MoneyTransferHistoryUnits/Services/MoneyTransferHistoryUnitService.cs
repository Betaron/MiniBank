using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;

namespace Minibank.Core.Domains.MoneyTransferHistoryUnits.Services
{
    public class MoneyTransferHistoryUnitService : IMoneyTransferHistoryUnitService
    {
        private readonly IMoneyTransferHistoryUnitRepository _historyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoneyTransferHistoryUnitService(IMoneyTransferHistoryUnitRepository historyRepository, IUnitOfWork unitOfWork)
        {
            _historyRepository = historyRepository;
            _unitOfWork = unitOfWork;
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
            _unitOfWork.SaveChanges();
        }

        public void Update(MoneyTransferHistoryUnit unit)
        {
            _historyRepository.Update(unit);
            _unitOfWork.SaveChanges();
        }

        public void Delete(string id)
        {
            _historyRepository.Delete(id);
            _unitOfWork.SaveChanges();
        }
    }
}
