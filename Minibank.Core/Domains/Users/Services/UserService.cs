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

        public Task<User> GetByIdAsync(string id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return _userRepository.GetAllAsync();
        }

        public async Task CreateAsync(User user)
        {
            if (user.Login is null || user.Email is null)
            {
                throw new ValidationException("Неверные данные");
            }
            
            await _userRepository.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            if (user.Login is null || user.Email is null)
            {
                throw new ValidationException("Неверные данные");
            }
            
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var allAccountsQuery = await _accountRepository.GetAllAsync();
            var hasAccounts = allAccountsQuery.ToList().Exists(it => it.UserId == id);

            if (hasAccounts)
            {
                throw new ValidationException("Есть привязанные банковские аккаунты");
            }

            await _userRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
