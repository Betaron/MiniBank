using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.MoneyTransferHistoryUnits;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IMoneyTransferHistoryUnitRepository _historyRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountService(
            IBankAccountRepository bankAccountRepository, 
            IMoneyTransferHistoryUnitRepository historyRepository, 
            ICurrencyConverter currencyConverter, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _bankAccountRepository = bankAccountRepository;
            _historyRepository = historyRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currencyConverter = currencyConverter;
        }

        public Task<BankAccount> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return _bankAccountRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var existence = await _userRepository.ExistsAsync(userId, cancellationToken);

            if (!existence)
            {
                throw new ObjectNotFoundException($"Пользователь с id {userId} не найден");
            }

            return await _bankAccountRepository.GetByUserIdAsync(userId, cancellationToken);
        }

        public Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _bankAccountRepository.GetAllAsync(cancellationToken);
        }

        public async Task CreateAsync(BankAccount account, CancellationToken cancellationToken)
        {
            if (account.UserId is null)
            {
                throw new ValidationException("Неверные данные");
            }

            var existence = await _userRepository.ExistsAsync(account.UserId, cancellationToken);

            if (!existence)
            {
                throw new ObjectNotFoundException($"Пользователь с id {account.UserId} не найден");
            }

            await _bankAccountRepository.CreateAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(BankAccount account, CancellationToken cancellationToken)
        {
            if (account.UserId is null)
            {
                throw new ValidationException("Неверные данные");
            }

            await _bankAccountRepository.UpdateAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);

            if (account.AccountBalance != 0)
            {
                throw new ValidationException("Баланс не нулевой");
            }

            await _bankAccountRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CloseAccountAsync(string id, CancellationToken cancellationToken)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);

            if (account.AccountBalance != 0)
            {
                throw new ValidationException("Баланс не нулевой");
            }

            if (account.IsActive && account.ClosingDate is not null)
            {
                throw new ValidationException("Аккаунт уже закрыт");
            }

            await _bankAccountRepository.CloseAccountAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBalanceAsync(string id, double amount, CancellationToken cancellationToken)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);

            if (!account.IsActive)
            {
                throw new ValidationException("Счёт закрыт");
            }

            await _bankAccountRepository.UpdateBalanceAsync(id, amount, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<double> CalculateCommissionAsync(double amount, string fromAccountId, string toAccountId, CancellationToken cancellationToken)
        {
            if (amount < 1)
            {
                throw new ValidationException("Сумма слишком мала");
            }

            var fromUser = await _bankAccountRepository.GetByIdAsync(fromAccountId, cancellationToken);
            var toUser = await _bankAccountRepository.GetByIdAsync(toAccountId, cancellationToken);

            var commissionValue = fromUser.UserId != toUser.UserId ? 0.02 : 0.0;

            return Math.Round(amount * commissionValue, 2);
        }

        public async Task MoneyTransactionAsync(double amount, string fromAccountId, string toAccountId, CancellationToken cancellationToken)
        {
            if (amount < 1)
            {
                throw new ValidationException("Сумма слишком мала");
            }

            if (fromAccountId == toAccountId)
            {
                throw new ValidationException("Нельзя перевести средства на тот же счёт");
            }

            var fromAccount = await _bankAccountRepository.GetByIdAsync(fromAccountId, cancellationToken);
            var toAccount = await _bankAccountRepository.GetByIdAsync(toAccountId, cancellationToken);

            if (!fromAccount.IsActive || !toAccount.IsActive)
            {
                throw new ValidationException("Один из счетов неактивен");
            }

            if (fromAccount.AccountBalance < amount)
            {
                throw new ValidationException("Недостаточно средств на счёте");
                
            }

            var commissionValue = await CalculateCommissionAsync(amount, fromAccountId, toAccountId, cancellationToken);
            var creditedAmount = await _currencyConverter.ConvertCurrencyAsync(amount - commissionValue, fromAccount.Currency, toAccount.Currency, cancellationToken);

            await _bankAccountRepository.UpdateBalanceAsync(fromAccountId, fromAccount.AccountBalance - amount, cancellationToken);
            await _bankAccountRepository.UpdateBalanceAsync(toAccountId, toAccount.AccountBalance + creditedAmount, cancellationToken);

            await _historyRepository.CreateAsync(new MoneyTransferHistoryUnit
            {
                Currency = fromAccount.Currency,
                Amount = amount,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            }, cancellationToken);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
