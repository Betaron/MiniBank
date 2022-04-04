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

        public User GetById(string id)
        { 
            return _userRepository.GetById(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public void Create(User user)
        {
            if (user.Login is null || user.Email is null)
            {
                throw new ValidationException("Неверные данные");
            }
            
            _userRepository.Create(user);
            _unitOfWork.SaveChanges();
        }

        public void Update(User user)
        {
            if (user.Login is null || user.Email is null)
            {
                throw new ValidationException("Неверные данные");
            }
            
            _userRepository.Update(user);
            _unitOfWork.SaveChanges();
        }

        public void Delete(string id)
        {
            var hasAccounts = _accountRepository.GetAll().ToList().Exists(it =>
                it.UserId == id);

            if (hasAccounts)
            {
                throw new ValidationException("Есть привязанные банковские аккаунты");
            }

            _userRepository.Delete(id);
            _unitOfWork.SaveChanges();
        }
    }
}
