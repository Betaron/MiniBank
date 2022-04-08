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

        public Task<MoneyTransferHistoryUnit> GetByIdAsync(string id)
        {
            return _historyRepository.GetByIdAsync(id);
        }

        public Task<IEnumerable<MoneyTransferHistoryUnit>> GetAllAsync()
        {
            return _historyRepository.GetAllAsync();
        }

        public async Task CreateAsync(MoneyTransferHistoryUnit unit)
        {
            await _historyRepository.CreateAsync(unit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(MoneyTransferHistoryUnit unit)
        {
            await _historyRepository.UpdateAsync(unit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            await _historyRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
