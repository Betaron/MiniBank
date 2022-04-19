using FluentValidation;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.BankAccounts.Validators;
using Minibank.Core.Domains.MoneyTransferHistoryUnits;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IMoneyTransferHistoryUnitRepository _historyRepository;
        private readonly IUserRepository _userRepository; 
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IUnitOfWork _unitOfWork;

        private readonly BankAccountValidator _bankAccountValidator;

        public BankAccountService(
            IBankAccountRepository bankAccountRepository, 
            IMoneyTransferHistoryUnitRepository historyRepository, 
            IUserRepository userRepository,
            ICurrencyConverter currencyConverter,
            IUnitOfWork unitOfWork,
            BankAccountValidator bankAccountValidator)
        {
            _bankAccountRepository = bankAccountRepository;
            _historyRepository = historyRepository;
            _unitOfWork = unitOfWork;
            _bankAccountValidator = bankAccountValidator;
            _userRepository = userRepository;
            _currencyConverter = currencyConverter;
        }

        public Task<BankAccount> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _bankAccountRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<BankAccount>> GetByUserIdAsync(
            Guid userId, CancellationToken cancellationToken)
        {
            await FindUserValidateAndThrowAsync(userId, cancellationToken);
            
            return await _bankAccountRepository.GetByUserIdAsync(userId, cancellationToken);
        }

        public Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _bankAccountRepository.GetAllAsync(cancellationToken);
        }

        public async Task CreateAsync(BankAccount account, CancellationToken cancellationToken)
        {
            await _bankAccountValidator.ValidateAndThrowAsync(account, cancellationToken);
            await FindUserValidateAndThrowAsync(account.UserId, cancellationToken);

            await _bankAccountRepository.CreateAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(BankAccount account, CancellationToken cancellationToken)
        {
            await _bankAccountValidator.ValidateAndThrowAsync(account, cancellationToken);

            await _bankAccountRepository.UpdateAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);

            HasNonZeroBalanceValidateAndThrow(account);

            await _bankAccountRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CloseAccountAsync(Guid id, CancellationToken cancellationToken)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);

            HasNonZeroBalanceValidateAndThrow(account);

            AccountInactiveValidateAndThrow(account);

            await _bankAccountRepository.CloseAccountAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBalanceAsync(
            Guid id, double amount, CancellationToken cancellationToken)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);

            AccountInactiveValidateAndThrow(account);

            await _bankAccountRepository.UpdateBalanceAsync(id, amount, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<double> CalculateCommissionAsync(
            double amount,
            Guid fromAccountId,
            Guid toAccountId, 
            CancellationToken cancellationToken)
        {
            TransactionAmountValidateAndThrow(amount);

            var fromAccount = 
                await _bankAccountRepository.GetByIdAsync(fromAccountId, cancellationToken);
            var toAccount = 
                await _bankAccountRepository.GetByIdAsync(toAccountId, cancellationToken);

            var commission = CalculateCommission(amount, fromAccount, toAccount);

            return commission;
        }

        public async Task MoneyTransactAsync(
            double amount,
            Guid fromAccountId,
            Guid toAccountId, 
            CancellationToken cancellationToken)
        {
            TransactionAmountValidateAndThrow(amount);

            var fromAccount = 
                await _bankAccountRepository.GetByIdAsync(fromAccountId, cancellationToken);
            var toAccount = 
                await _bankAccountRepository.GetByIdAsync(toAccountId, cancellationToken);

            TransactionValidateAndThrow(amount, fromAccount, toAccount);

            var commissionValue = CalculateCommission(amount, fromAccount, toAccount);
            var creditedAmount = 
                await _currencyConverter.ConvertCurrencyAsync(
                    amount - commissionValue, 
                    fromAccount.Currency, 
                    toAccount.Currency, 
                    cancellationToken);

            await _bankAccountRepository.UpdateBalanceAsync(
                fromAccountId,
                fromAccount.AccountBalance - amount, 
                cancellationToken);
            await _bankAccountRepository.UpdateBalanceAsync(
                toAccountId, 
                toAccount.AccountBalance + creditedAmount, 
                cancellationToken);

            await _historyRepository.CreateAsync(new MoneyTransferHistoryUnit
            {
                Currency = fromAccount.Currency,
                Amount = amount,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            }, cancellationToken);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Calculates the value of the commission for a transaction
        /// </summary>
        /// <param name="amount">Any number, including negative ones</param>
        private double CalculateCommission(
            double amount,
            BankAccount fromAccount,
            BankAccount toAccount)
        {
            var commissionValue = fromAccount.UserId != toAccount.UserId ? 0.02 : 0.0;

            return Math.Round(amount * commissionValue, 2);
        }

        private void AccountInactiveValidateAndThrow(BankAccount account)
        {
            if (!account.IsActive)
            {
                throw new ValidationException($"{account.Id} - аккаунт неактивен");
            }
        }

        private void HasNonZeroBalanceValidateAndThrow(BankAccount account)
        {
            if (account.AccountBalance != 0)
            {
                throw new ValidationException($"Баланс не нулевой - {account.Id}");
            }
        }

        private void TransactionAmountValidateAndThrow(double amount)
        {
            if (amount <= 0)
            {
                throw new ValidationException("Сумма перевода должна превышать 0");
            }
        }

        private void TransactionValidateAndThrow(
            double amount, BankAccount fromAccount, BankAccount toAccount)
        {
            AccountInactiveValidateAndThrow(fromAccount);
            AccountInactiveValidateAndThrow(toAccount);

            if (fromAccount.Id == toAccount.Id)
            {
                throw new ValidationException(
                    $"Нельзя перевести средства на тот же счёт - {fromAccount.Id}");
            }

            if (fromAccount.AccountBalance < amount)
            {
                throw new ValidationException(
                    $"Недостаточно средств на счёте - {fromAccount.Id}");
            }
        }

        private async Task FindUserValidateAndThrowAsync(Guid userId, CancellationToken cancellationToken)
        {
            var isUserExists = await _userRepository.UserExistsByIdAsync(userId, cancellationToken);

            if (!isUserExists)
            {
                throw new ValidationException($"Пользователь с id: {userId} не найден");
            }
        }
    }
}
