using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;

namespace Minibank.Core.Domains.MoneyTransferHistoryUnits.Services
{
    public class MoneyTransferHistoryUnitService : IMoneyTransferHistoryUnitService
    {
        private readonly IMoneyTransferHistoryUnitRepository _historyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoneyTransferHistoryUnitService(
            IMoneyTransferHistoryUnitRepository historyRepository, IUnitOfWork unitOfWork)
        {
            _historyRepository = historyRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<MoneyTransferHistoryUnit> GetByIdAsync(
            string id, CancellationToken cancellationToken)
        {
            return _historyRepository.GetByIdAsync(id, cancellationToken);
        }

        public Task<IEnumerable<MoneyTransferHistoryUnit>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return _historyRepository.GetAllAsync(cancellationToken);
        }

        public async Task CreateAsync(
            MoneyTransferHistoryUnit unit, CancellationToken cancellationToken)
        {
            await _historyRepository.CreateAsync(unit, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(
            MoneyTransferHistoryUnit unit, CancellationToken cancellationToken)
        {
            await _historyRepository.UpdateAsync(unit, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(
            string id, CancellationToken cancellationToken)
        {
            await _historyRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
