using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Core.Domains.BankAccounts.Validators;
using Minibank.Core.Domains.MoneyTransferHistoryUnits;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;
using Xunit;
using Moq;

namespace Minibank.Core.Tests
{
    public class BankAccountServiceTests
    {
        private readonly BankAccountService _bankAccountService;

        private readonly Mock<IBankAccountRepository> _mockBankAccountRepository;
        private readonly Mock<IMoneyTransferHistoryUnitRepository> _mockHistoryRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ICurrencyConverter> _mockCurrencyConverter;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;


        public BankAccountServiceTests()
        {
            _mockBankAccountRepository = new Mock<IBankAccountRepository>();
            _mockHistoryRepository = new Mock<IMoneyTransferHistoryUnitRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockCurrencyConverter = new Mock<ICurrencyConverter>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _bankAccountService = new BankAccountService(
                _mockBankAccountRepository.Object,
                _mockHistoryRepository.Object,
                _mockUserRepository.Object,
                _mockCurrencyConverter.Object,
                _mockUnitOfWork.Object,
                new BankAccountValidator()
            );
        }

        [Fact]
        public async Task GetAccountById_SuccessPath_SameAccount()
        {
            //ARRANGE
            var expectedAccount = new BankAccount();
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(expectedAccount);

            //ACT
            var result =
                await _bankAccountService.GetByIdAsync(Guid.Empty, CancellationToken.None);

            //ASSERT
            Assert.Equal(expectedAccount, result);
        }

        [Fact]
        public async Task GetAccountsByUserId_SuccessPath_SameAccounts()
        {
            //ARRANGE
            var userId = Guid.Empty;
            var expectedAccounts = new List<BankAccount>()
            {
                new BankAccount() {UserId = userId}
            };
            _mockUserRepository.Setup(repos =>
                    repos.UserExistsByIdAsync(Guid.Empty, CancellationToken.None))
                .ReturnsAsync(true);
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByUserIdAsync(userId, CancellationToken.None))
                .ReturnsAsync(expectedAccounts);

            //ACT
            var result = 
                await _bankAccountService.GetByUserIdAsync(userId, CancellationToken.None);

            //ARRANGE
            Assert.Equal(expectedAccounts, result);
        }

        [Fact]
        public async Task GetAccountsByUserId_UserNotExists_ShouldThrowException()
        {
            //ARRANGE
            var userId = Guid.Empty;
            _mockUserRepository.Setup(repos =>
                    repos.UserExistsByIdAsync(userId, CancellationToken.None))
                .ReturnsAsync(false);

            //ACT
            var exception = 
                await Assert.ThrowsAsync<ValidationException>(() =>
                _bankAccountService.GetByUserIdAsync(Guid.Empty, CancellationToken.None));

            //ARRANGE
            Assert.Contains(
                $"Пользователь с id: {userId} не найден",
                exception.ValidationMessage);
        }

        [Fact]
        public async Task GetAllAccounts_SuccessPath_SameAccounts()
        {
            //ARRANGE
            var expectedAccounts = new List<BankAccount>()
            {
                new BankAccount()
            };
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(expectedAccounts);

            //ACT
            var result =
                await _bankAccountService.GetAllAsync(CancellationToken.None);

            //ARRANGE
            Assert.Equal(expectedAccounts, result);
        }

        [Fact]
        public async Task AddAccount_SuccessPath_CreateCalled()
        {
            //ARRANGE
            var validAccount = new BankAccount()
            {
                UserId = Guid.NewGuid(),
                Currency = CurrencyType.RUB
            };
            _mockUserRepository.Setup(repos =>
                    repos.UserExistsByIdAsync(validAccount.UserId, CancellationToken.None))
                .ReturnsAsync(true);

            //ACT
            await _bankAccountService.CreateAsync(validAccount, CancellationToken.None);

            //ASSERT
            _mockBankAccountRepository.Verify(repos =>
                    repos.CreateAsync(validAccount, CancellationToken.None),
                Times.Once());
            _mockUnitOfWork.Verify(saver =>
                    saver.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task AddAccount_EmptyUserId_ShouldThrowException()
        {
            //ARRANGE
            var invalidAccount = new BankAccount()
            {
                UserId = Guid.Empty,
                Currency = default
            };

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => 
                    _bankAccountService.CreateAsync(invalidAccount, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "UserId: Поле не должно быть пустым",
                exception.Message);
        }

        [Fact]
        public async Task AddAccount_InvalidCurrency_ShouldThrowException()
        {
            //ARRANGE
            var invalidAccount = new BankAccount()
            {
                UserId = Guid.NewGuid(),
                Currency = (CurrencyType)(-1)
            };

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                    _bankAccountService.CreateAsync(invalidAccount, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Currency: Поле не должно быть пустым",
                exception.Message);
        }

        [Fact]
        public async Task AddAccount_UserNotFound_ShouldThrowException()
        {
            //ARRANGE
            var missingUserId = Guid.NewGuid();
            var account = new BankAccount()
            {
                UserId = missingUserId,
                Currency = default
            };
            _mockUserRepository.Setup(repos =>
                    repos.UserExistsByIdAsync(account.UserId, CancellationToken.None))
                .ReturnsAsync(false);

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.CreateAsync(account, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                $"Пользователь с id: {missingUserId} не найден",
                exception.ValidationMessage);
        }

        [Fact]
        public async Task UpdateAccount_SuccessPath_UpdateCalled()
        {
            //ARRANGE
            var validAccount = new BankAccount()
            {
                UserId = Guid.NewGuid(),
                Currency = CurrencyType.RUB
            };

            //ACT
            await _bankAccountService.UpdateAsync(validAccount, CancellationToken.None);

            //ASSERT
            _mockBankAccountRepository.Verify(repos =>
                    repos.UpdateAsync(validAccount, CancellationToken.None),
                Times.Once());
            _mockUnitOfWork.Verify(saver =>
                    saver.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAccount_EmptyUserId_ShouldThrowException()
        {
            //ARRANGE
            var invalidAccount = new BankAccount()
            {
                UserId = Guid.Empty,
                Currency = default
            };

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                    _bankAccountService.UpdateAsync(invalidAccount, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "UserId: Поле не должно быть пустым",
                exception.Message);
        }

        [Fact]
        public async Task UpdateAccount_InvalidCurrency_ShouldThrowException()
        {
            //ARRANGE
            var invalidAccount = new BankAccount()
            {
                UserId = Guid.NewGuid(),
                Currency = (CurrencyType)(-1)
            };

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                    _bankAccountService.UpdateAsync(invalidAccount, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Currency: Поле не должно быть пустым",
                exception.Message);
        }

        [Fact]
        public async Task RemoveBankAccount_SuccessPath_DeleteCalled()
        {
            //ARRANGE
            var validAccount = new BankAccount()
            {
                Id = Guid.Empty,
                AccountBalance = 0
            };
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(validAccount.Id, CancellationToken.None))
                .ReturnsAsync(validAccount);

            //ACT
            await _bankAccountService.DeleteAsync(validAccount.Id, CancellationToken.None);

            //ASSERT
            _mockBankAccountRepository.Verify(repos =>
                    repos.DeleteAsync(validAccount.Id, CancellationToken.None),
                Times.Once());
            _mockUnitOfWork.Verify(saver =>
                    saver.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task RemoveBankAccount_NonZeroBalance_ShouldThrowException()
        {
            //ARRANGE
            var invalidAccount = new BankAccount()
            {
                Id = Guid.Empty,
                AccountBalance = 1
            };
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(invalidAccount.Id, CancellationToken.None))
                .ReturnsAsync(invalidAccount);

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.DeleteAsync(invalidAccount.Id, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                $"Баланс не нулевой - {invalidAccount.Id}",
                exception.ValidationMessage);
        }

        [Fact]
        public async Task CloseBankAccount_SuccessPath_CloseCalled()
        {
            //ARRANGE
            var validAccount = new BankAccount()
            {
                Id = Guid.Empty,
                AccountBalance = 0,
                IsActive = true
            };
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(validAccount.Id, CancellationToken.None))
                .ReturnsAsync(validAccount);

            //ACT
            await _bankAccountService.CloseAccountAsync(validAccount.Id, CancellationToken.None);

            //ASSERT
            _mockBankAccountRepository.Verify(repos =>
                    repos.CloseAccountAsync(validAccount.Id, CancellationToken.None),
                Times.Once());
            _mockUnitOfWork.Verify(saver =>
                    saver.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task CloseBankAccount_NonZeroBalance_ShouldThrowException()
        {
            //ARRANGE
            var invalidAccount = new BankAccount()
            {
                Id = Guid.Empty,
                AccountBalance = 1,
                IsActive = true
            };
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(invalidAccount.Id, CancellationToken.None))
                .ReturnsAsync(invalidAccount);

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.CloseAccountAsync(invalidAccount.Id, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                $"Баланс не нулевой - {invalidAccount.Id}",
                exception.ValidationMessage);
        }

        [Fact]
        public async Task CloseBankAccount_AlreadyClosed_ShouldThrowException()
        {
            //ARRANGE
            var invalidAccount = new BankAccount()
            {
                Id = Guid.Empty,
                AccountBalance = 0,
                IsActive = false
            };
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(invalidAccount.Id, CancellationToken.None))
                .ReturnsAsync(invalidAccount);

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.CloseAccountAsync(
                        invalidAccount.Id, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                $"{invalidAccount.Id} - аккаунт неактивен",
                exception.ValidationMessage);
        }

        [Fact]
        public async Task UpdateAccountBalance_SuccessPath_UpdateBalanceCalled()
        {
            //ARRANGE
            var amount = 100;
            var validAccount = new BankAccount()
            {
                Id = Guid.Empty,
                IsActive = true
            };
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(validAccount.Id, CancellationToken.None))
                .ReturnsAsync(validAccount);

            //ACT
            await _bankAccountService.UpdateBalanceAsync(
                validAccount.Id, amount, CancellationToken.None);

            //ASSERT
            _mockBankAccountRepository.Verify(repos =>
                    repos.UpdateBalanceAsync(
                        validAccount.Id, amount, CancellationToken.None),
                Times.Once());
            _mockUnitOfWork.Verify(saver =>
                    saver.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAccountBalance_AccountClosed_ShouldThrowException()
        {
            //ARRANGE
            var amount = 100;
            var invalidAccount = new BankAccount()
            {
                Id = Guid.Empty,
                IsActive = false
            };
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(invalidAccount.Id, CancellationToken.None))
                .ReturnsAsync(invalidAccount);

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.UpdateBalanceAsync(
                        invalidAccount.Id, amount, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                $"{invalidAccount.Id} - аккаунт неактивен",
                exception.ValidationMessage);
        }

        [Theory]
        [InlineData(100, false, 2)]
        [InlineData(100, true, 0)]
        public async Task CalculateCommission_SuccessPath_GetRightAmount(
            double amount, 
            bool sameUser,
            double expected)
        {
            //ARRANGE
            var fromAccountId = Guid.NewGuid();
            var toAccountId = Guid.NewGuid();

            var fromUserId = Guid.NewGuid();
            var toUserId = sameUser ? fromUserId : Guid.NewGuid();

            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(fromAccountId, CancellationToken.None))
                .ReturnsAsync(new BankAccount() {AccountBalance = amount, UserId = fromUserId});
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(toAccountId, CancellationToken.None))
                .ReturnsAsync(new BankAccount() { UserId = toUserId });

            //ACT
            var result = await _bankAccountService.CalculateCommissionAsync(
                amount, fromAccountId, toAccountId, CancellationToken.None);

            //ASSERT
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task CalculateCommission_AmountTooSmall_ShouldThrowException()
        {
            //ARRANGE
            var amount = -1;

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.CalculateCommissionAsync(
                        amount, Guid.Empty, Guid.Empty, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Сумма перевода должна превышать 0",
                exception.ValidationMessage);
        }

        [Theory]
        [InlineData(100, false, 100, 0, 0, 98)]
        [InlineData(100, true, 100, 0, 0, 100)]
        [InlineData(50, false, 200, 150, 150, 199)]
        [InlineData(50, true, 200, 150, 150, 200)]
        public async Task MoneyTransact_SuccessPath_RightUpdateAccountsBalance(
            double amount,
            bool sameUser,
            double fromBalance,
            double toBalance,
            double fromExpectedBalance,
            double toExpectedBalance)
        {
            //ARRANGE
            var validFromAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                AccountBalance = fromBalance,
                UserId = Guid.NewGuid(),
                IsActive = true
            };

            var validToAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                AccountBalance = toBalance,
                UserId = sameUser ? validFromAccount.UserId : Guid.NewGuid(),
                IsActive = true
            };

            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(validFromAccount.Id, CancellationToken.None))
                .ReturnsAsync(validFromAccount);
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(validToAccount.Id, CancellationToken.None))
                .ReturnsAsync(validToAccount);

            _mockCurrencyConverter.Setup(converter =>
                    converter.ConvertCurrencyAsync(
                        toExpectedBalance - toBalance,
                        It.IsAny<CurrencyType>(), 
                        It.IsAny<CurrencyType>(), 
                        CancellationToken.None))
                .ReturnsAsync(toExpectedBalance - toBalance);

            //ACT
            await _bankAccountService.MoneyTransactAsync(
                amount, validFromAccount.Id, validToAccount.Id, CancellationToken.None);

            //ASSERT
            _mockBankAccountRepository.Verify(repos =>
                repos.UpdateBalanceAsync(
                    validFromAccount.Id, fromExpectedBalance, CancellationToken.None),
                Times.Once);
            _mockBankAccountRepository.Verify(repos =>
                repos.UpdateBalanceAsync(
                    validToAccount.Id, toExpectedBalance, CancellationToken.None),
                Times.Once);
            _mockUnitOfWork.Verify(saver =>
                    saver.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task MoneyTransact_SuccessPath_WriteInHistory()
        {
            //ARRANGE
            var validFromAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                AccountBalance = 1,
                UserId = Guid.NewGuid(),
                IsActive = true
            };

            var validToAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                AccountBalance = 0,
                UserId = Guid.NewGuid(),
                IsActive = true
            };

            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(validFromAccount.Id, CancellationToken.None))
                .ReturnsAsync(validFromAccount);
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(validToAccount.Id, CancellationToken.None))
                .ReturnsAsync(validToAccount);

            //ACT
            await _bankAccountService.MoneyTransactAsync(
                1, validFromAccount.Id, validToAccount.Id, CancellationToken.None);

            //ASSERT
            _mockHistoryRepository.Verify(writer =>
                writer.CreateAsync(
                    It.IsAny<MoneyTransferHistoryUnit>(), 
                    CancellationToken.None),
                Times.Once);
        }

        [Fact]
        public async Task MoneyTransact_AmountTooSmall_ShouldThrowException()
        {
            //ARRANGE
            var amount = -1;

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.MoneyTransactAsync(
                        amount, Guid.Empty, Guid.Empty, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Сумма перевода должна превышать 0",
                exception.ValidationMessage);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public async Task MoneyTransact_OneOfAccountsInactive_ShouldThrowException(
            bool fromIsActive, bool toIsActive)
        {
            //ARRANGE
            var fromAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                AccountBalance = 1,
                IsActive = fromIsActive
            };

            var toAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                IsActive = toIsActive
            };

            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(fromAccount.Id, CancellationToken.None))
                .ReturnsAsync(fromAccount);
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(toAccount.Id, CancellationToken.None))
                .ReturnsAsync(toAccount);

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.MoneyTransactAsync(
                        1, fromAccount.Id, toAccount.Id, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                $" - аккаунт неактивен",
                exception.ValidationMessage);
        }

        [Fact]
        public async Task MoneyTransact_SameAccounts_ShouldThrowException()
        {
            //ARRANGE
            var fromAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                AccountBalance = 1,
                IsActive = true
            };

            var toAccount = new BankAccount()
            {
                Id = fromAccount.Id,
                IsActive = true
            };

            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(fromAccount.Id, CancellationToken.None))
                .ReturnsAsync(fromAccount);
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(toAccount.Id, CancellationToken.None))
                .ReturnsAsync(toAccount);

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.MoneyTransactAsync(
                        1, fromAccount.Id, toAccount.Id, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                $"Нельзя перевести средства на тот же счёт - {fromAccount.Id}",
                exception.ValidationMessage);
        }

        [Fact]
        public async Task MoneyTransact_NotEnoughMoney_ShouldThrowException()
        {
            //ARRANGE
            var fromAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                AccountBalance = 0,
                IsActive = true
            };

            var toAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                IsActive = true
            };

            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(fromAccount.Id, CancellationToken.None))
                .ReturnsAsync(fromAccount);
            _mockBankAccountRepository.Setup(repos =>
                    repos.GetByIdAsync(toAccount.Id, CancellationToken.None))
                .ReturnsAsync(toAccount);

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _bankAccountService.MoneyTransactAsync(
                        1, fromAccount.Id, toAccount.Id, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                $"Недостаточно средств на счёте - {fromAccount.Id}",
                exception.ValidationMessage);
        }
    }
}
