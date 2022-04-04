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

        public BankAccount GetById(string id)
        {
            return _bankAccountRepository.GetById(id); ;
        }

        public IEnumerable<BankAccount> GetByUserId(string userId)
        {
            if (!_userRepository.Exists(userId))
            {
                throw new ObjectNotFoundException($"Пользователь с id {userId} не найден");
            }

            return _bankAccountRepository.GetByUserId(userId);
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return _bankAccountRepository.GetAll();
        }

        public void Create(BankAccount account)
        {
            if (account.UserId is null)
            {
                throw new ValidationException("Неверные данные");
            }

            if (!_userRepository.Exists(account.UserId))
            {
                throw new ObjectNotFoundException($"Пользователь с id {account.UserId} не найден");
            }

            _bankAccountRepository.Create(account);
            _unitOfWork.SaveChanges();
        }

        public void Update(BankAccount account)
        {
            if (account.UserId is null)
            {
                throw new ValidationException("Неверные данные");
            }

            _bankAccountRepository.Update(account);
            _unitOfWork.SaveChanges();
        }

        public void Delete(string id)
        {
            var account = _bankAccountRepository.GetById(id);

            if (account.AccountBalance != 0)
            {
                throw new ValidationException("Баланс не нулевой");
            }

            _bankAccountRepository.Delete(id);
            _unitOfWork.SaveChanges();
        }

        public void CloseAccount(string id)
        {
            var account = _bankAccountRepository.GetById(id);

            if (account.AccountBalance != 0)
            {
                throw new ValidationException("Баланс не нулевой");
            }

            if (account.IsActive && account.ClosingDate is not null)
            {
                throw new ValidationException("Аккаунт уже закрыт");
            }

            _bankAccountRepository.CloseAccount(id);
            _unitOfWork.SaveChanges();
        }

        public void UpdateBalance(string id, double amount)
        {
            var account = _bankAccountRepository.GetById(id);

            if (!account.IsActive)
            {
                throw new ValidationException("Счёт закрыт");
            }

            _bankAccountRepository.UpdateBalance(id, amount);
            _unitOfWork.SaveChanges();
        }

        public double CalculateCommission(double amount, string fromAccountId, string toAccountId)
        {
            if (amount < 1)
            {
                throw new ValidationException("Сумма слишком мала");
            }

            var fromUser = _bankAccountRepository.GetById(fromAccountId);
            var toUser = _bankAccountRepository.GetById(toAccountId);

            var commissionValue = fromUser.UserId != toUser.UserId ? 0.02 : 0.0;

            return Math.Round(amount * commissionValue, 2);
        }

        public void MoneyTransaction(double amount, string fromAccountId, string toAccountId)
        {
            if (amount < 1)
            {
                throw new ValidationException("Сумма слишком мала");
            }

            if (fromAccountId == toAccountId)
            {
                throw new ValidationException("Нельзя перевести средства на тот же счёт");
            }

            var fromAccount = _bankAccountRepository.GetById(fromAccountId);
            var toAccount = _bankAccountRepository.GetById(toAccountId);

            if (!fromAccount.IsActive || !toAccount.IsActive)
            {
                throw new ValidationException("Один из счетов неактивен");
            }

            if (fromAccount.AccountBalance < amount)
            {
                throw new ValidationException("Недостаточно средств на счёте");
                
            }

            var commissionValue = CalculateCommission(amount, fromAccountId, toAccountId);
            var creditedAmount = _currencyConverter.ConvertCurrency(amount - commissionValue, fromAccount.Currency, toAccount.Currency);

            _bankAccountRepository.UpdateBalance(fromAccountId, fromAccount.AccountBalance - amount);
            _bankAccountRepository.UpdateBalance(toAccountId, toAccount.AccountBalance + creditedAmount);

            _historyRepository.Create(new MoneyTransferHistoryUnit
            {
                Currency = fromAccount.Currency,
                Amount = amount,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });

            _unitOfWork.SaveChanges();
        }
    }
}
