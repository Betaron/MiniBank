using FluentValidation;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Validators;
using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmptyFieldsValidator _userValidator;

        public UserService(
            IUserRepository userRepository, 
            IBankAccountRepository accountRepository,
            IUnitOfWork unitOfWork, 
            EmptyFieldsValidator userValidator)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
        }

        public Task<User> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return _userRepository.GetByIdAsync(id, cancellationToken);
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _userRepository.GetAllAsync(cancellationToken);
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _userValidator.ValidateAndThrowAsync(user, cancellationToken);
            
            await _userRepository.CreateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            await _userValidator.ValidateAndThrowAsync(user, cancellationToken);

            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            await AccountExistenceValidateAndThrowAsync(id, cancellationToken);

            await _userRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task AccountExistenceValidateAndThrowAsync(
            string id, CancellationToken cancellationToken)
        {
            var allAccountsQuery = 
                await _accountRepository.GetAllAsync(cancellationToken);
            var hasAccounts = 
                allAccountsQuery.ToList().Exists(it => it.UserId == id);

            if (hasAccounts)
            {
                throw new ValidationException(
                    "Есть привязанные банковские аккаунты");
            }
        }
    }
}
