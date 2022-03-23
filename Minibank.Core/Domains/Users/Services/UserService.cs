using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
        }

        public void Update(User user)
        {
            if (user.Login is null || user.Email is null)
            {
                throw new ValidationException("Неверные данные");
            }
            
            _userRepository.Update(user);
        }

        public void Delete(string id)
        {
            var hasAccounts = _userRepository.HasBankAccounts(id);

            if (hasAccounts)
            {
                throw new ValidationException("Есть привязанные банковские аккаунты");
            }

            _userRepository.Delete(id);
        }
    }
}
