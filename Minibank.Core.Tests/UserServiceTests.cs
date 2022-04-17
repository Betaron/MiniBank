using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Domains.Users.Validators;
using Minibank.Core.Exceptions;
using Xunit;
using Moq;

namespace Minibank.Core.Tests
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;

        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IBankAccountRepository> _mockBankAccountRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockBankAccountRepository = new Mock<IBankAccountRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _userService = new UserService(
                _mockUserRepository.Object,
                _mockBankAccountRepository.Object,
                _mockUnitOfWork.Object,
                new UserValidator(_mockUserRepository.Object));
        }

        [Fact]
        public async Task GetUserById_SuccessPath_SameUser()
        {
            //ARRANGE
            var expectedUser = new User();
            _mockUserRepository.Setup(repos =>
                    repos.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(expectedUser);

            //ACT
            var result =
                await _userService.GetByIdAsync(Guid.Empty, CancellationToken.None);

            //ASSERT
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetAllUsers_SuccessPath_ListWithSameUsers()
        {
            //ARRANGE
            var expectedUsers = new List<User>()
            {
                new User()
            };
            _mockUserRepository.Setup(repos =>
                    repos.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(expectedUsers);

            //ACT
            var result =
                await _userService.GetAllAsync(CancellationToken.None);

            //ASSERT
            Assert.Equal(expectedUsers, expectedUsers);
        }

        [Fact]
        public async Task AddUser_SuccessPath_CreateCalled()
        {
            //ARRANGE
            var expectedUser = new User()
            {
                Login = "someLogin",
                Email = "someEmail@email.com"
            };
            _mockUserRepository.Setup(repos =>
                repos.CreateAsync(expectedUser, CancellationToken.None));

            //ACT
            await _userService.CreateAsync(expectedUser, CancellationToken.None);

            //ASSERT
            _mockUserRepository.Verify(repos =>
                repos.CreateAsync(expectedUser, CancellationToken.None),
                Times.Once());
            _mockUnitOfWork.Verify(saver =>
                saver.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task AddUser_EmptyLogin_ShouldThrowException()
        {
            //ARRANGE
            var user = new User()
            {
                Login = "",
                Email = "someEmail@email.com"
            };

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                _userService.CreateAsync(user, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Login: Поле не должно быть пустым",
                exception.Message);
        }

        [Fact]
        public async Task AddUser_EmptyEmail_ShouldThrowException()
        {
            //ARRANGE
            var user = new User()
            {
                Login = "someLogin",
                Email = ""
            };

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                    _userService.CreateAsync(user, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Email: Поле не должно быть пустым",
                exception.Message);
        }

        [Fact]
        public async Task AddUser_LoginAlreadyExists_ShouldThrowException()
        {
            //ARRANGE
            var user = new User()
            {
                Login = "someLogin",
                Email = "someEmail@email.com"
            };
            _mockUserRepository.Setup(repos =>
                    repos.UserExistsByLoginAsync("someLogin", CancellationToken.None))
                .ReturnsAsync(true);

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                    _userService.CreateAsync(user, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Login: someLogin - занят",
                exception.Message);
        }

        [Fact]
        public async Task UpdateUser_SuccessPath_UpdateCalled()
        {
            //ARRANGE
            var expectedUser = new User()
            {
                Login = "someLogin",
                Email = "someEmail@email.com"
            };
            _mockUserRepository.Setup(repos =>
                repos.UpdateAsync(expectedUser, CancellationToken.None));

            //ACT
            await _userService.UpdateAsync(expectedUser, CancellationToken.None);

            //ASSERT
            _mockUserRepository.Verify(repos =>
                repos.UpdateAsync(expectedUser, CancellationToken.None),
                Times.Once());
            _mockUnitOfWork.Verify(saver =>
                saver.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateUser_EmptyLogin_ShouldThrowException()
        {
            //ARRANGE
            var user = new User()
            {
                Login = "",
                Email = "someEmail@email.com"
            };

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                _userService.UpdateAsync(user, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Login: Поле не должно быть пустым",
                exception.Message);
        }

        [Fact]
        public async Task UpdateUser_EmptyEmail_ShouldThrowException()
        {
            //ARRANGE
            var user = new User()
            {
                Login = "someLogin",
                Email = ""
            };

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                    _userService.UpdateAsync(user, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Email: Поле не должно быть пустым",
                exception.Message);
        }

        [Fact]
        public async Task UpdateUser_LoginAlreadyExists_ShouldThrowException()
        {
            //ARRANGE
            var user = new User()
            {
                Login = "someLogin",
                Email = "someEmail@email.com"
            };
            _mockUserRepository.Setup(repos =>
                    repos.UserExistsByLoginAsync("someLogin", CancellationToken.None))
                .ReturnsAsync(true);

            //ACT
            var exception =
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                    _userService.UpdateAsync(user, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "Login: someLogin - занят",
                exception.Message);
        }

        [Fact]
        public async Task RemoveUser_SuccessPath_DeleteCalled()
        {
            //ARRANGE
            var userId = Guid.Empty;

            //ACT
            await _userService.DeleteAsync(userId, CancellationToken.None);

            //ASSERT
            _mockUserRepository.Verify(repos =>
                    repos.DeleteAsync(userId, CancellationToken.None),
                Times.Once());
            _mockUnitOfWork.Verify(saver =>
                    saver.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task RemoveUser_HasAccounts_ShouldThrowException()
        {
            //ARRANGE
            var userId = Guid.Empty;
            _mockBankAccountRepository.Setup(repos =>
                    repos.ExistsByUserIdAsync(userId, CancellationToken.None))
                .ReturnsAsync(true);

            //ACT
            var exception =
                await Assert.ThrowsAsync<ValidationException>(() =>
                    _userService.DeleteAsync(userId, CancellationToken.None));

            //ASSERT
            Assert.Contains(
                "У пользователя с id: 00000000-0000-0000-0000-000000000000 есть привязанные банковские аккаунты",
                exception.ValidationMessage);
        }
    }
}
