using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IBankAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
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
            if (user.Login is null || user.Email is null)
            {
                throw new ValidationException("Неверные данные");
            }
            
            await _userRepository.CreateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            if (user.Login is null || user.Email is null)
            {
                throw new ValidationException("Неверные данные");
            }
            
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var allAccountsQuery = await _accountRepository.GetAllAsync(cancellationToken);
            var hasAccounts = allAccountsQuery.ToList().Exists(it => it.UserId == id);

            if (hasAccounts)
            {
                throw new ValidationException("Есть привязанные банковские аккаунты");
            }

            await _userRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
