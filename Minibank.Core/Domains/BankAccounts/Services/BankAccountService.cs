using FluentValidation;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.BankAccounts.Validators;
using Minibank.Core.Domains.MoneyTransferHistoryUnits;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;
using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IMoneyTransferHistoryUnitRepository _historyRepository;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IUnitOfWork _unitOfWork;

        private readonly FindUserValidator _findUserValidator;
        private readonly EmptyFieldsValidator _emptyFieldsValidator;

        public BankAccountService(
            IBankAccountRepository bankAccountRepository, 
            IMoneyTransferHistoryUnitRepository historyRepository, 
            ICurrencyConverter currencyConverter,
            IUnitOfWork unitOfWork, 
            FindUserValidator findUserValidator, 
            EmptyFieldsValidator emptyFieldsValidator)
        {
            _bankAccountRepository = bankAccountRepository;
            _historyRepository = historyRepository;
            _unitOfWork = unitOfWork;
            _findUserValidator = findUserValidator;
            _emptyFieldsValidator = emptyFieldsValidator;
            _currencyConverter = currencyConverter;
        }

        public Task<BankAccount> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return _bankAccountRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<BankAccount>> GetByUserIdAsync(
            string userId, CancellationToken cancellationToken)
        {
            await _findUserValidator.ValidateAndThrowAsync(
                new BankAccount() {UserId = userId}, 
                cancellationToken);
            
            return await _bankAccountRepository.GetByUserIdAsync(userId, cancellationToken);
        }

        public Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _bankAccountRepository.GetAllAsync(cancellationToken);
        }

        public async Task CreateAsync(BankAccount account, CancellationToken cancellationToken)
        {
            await _emptyFieldsValidator.ValidateAndThrowAsync(account, cancellationToken);
            await _findUserValidator.ValidateAndThrowAsync(account, cancellationToken);

            await _bankAccountRepository.CreateAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(BankAccount account, CancellationToken cancellationToken)
        {
            await _emptyFieldsValidator.ValidateAsync(account, cancellationToken);

            await _bankAccountRepository.UpdateAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);

            HasNonZeroBalanceValidateAndThrow(account);

            await _bankAccountRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CloseAccountAsync(string id, CancellationToken cancellationToken)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);

            HasNonZeroBalanceValidateAndThrow(account);

            AccountInactiveValidateAndThrow(account);

            await _bankAccountRepository.CloseAccountAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBalanceAsync(
            string id, double amount, CancellationToken cancellationToken)
        {
            var account = await _bankAccountRepository.GetByIdAsync(id, cancellationToken);

            AccountInactiveValidateAndThrow(account);

            await _bankAccountRepository.UpdateBalanceAsync(id, amount, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<double> CalculateCommissionAsync(
            double amount, 
            string fromAccountId, 
            string toAccountId, 
            CancellationToken cancellationToken)
        {
            TransactionValueValidateAndThrow(amount);

            var fromUser = 
                await _bankAccountRepository.GetByIdAsync(fromAccountId, cancellationToken);
            var toUser = 
                await _bankAccountRepository.GetByIdAsync(toAccountId, cancellationToken);

            var commissionValue = fromUser.UserId != toUser.UserId ? 0.02 : 0.0;

            return Math.Round(amount * commissionValue, 2);
        }

        public async Task MoneyTransactAsync(
            double amount, 
            string fromAccountId, 
            string toAccountId, 
            CancellationToken cancellationToken)
        {
            TransactionValueValidateAndThrow(amount);

            var fromAccount = 
                await _bankAccountRepository.GetByIdAsync(fromAccountId, cancellationToken);
            var toAccount = 
                await _bankAccountRepository.GetByIdAsync(toAccountId, cancellationToken);

            TransactionValidateAndThrow(amount, fromAccount, toAccount);

            var commissionValue = 
                await CalculateCommissionAsync(amount, fromAccountId, toAccountId, cancellationToken);
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

        private void TransactionValueValidateAndThrow(double amount)
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
    }
}
