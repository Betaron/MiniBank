using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domains.Users.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetById(string id)
        {
            var user = _userRepository.GetById(id);
            if (user is null)
            {
                throw new NotFoundException();
            }

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            var users = _userRepository.GetAll();
            if (users is null)
            {
                throw new NotFoundException();
            }

            return users;
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
            if (_userRepository.GetById(user.Id) is null)
            {
                throw new NotFoundException();
            }
            
            _userRepository.Update(user);
        }

        public void Delete(string id)
        {
            if (_userRepository.GetById(id) is null)
            {
                throw new NotFoundException();
            }

            if (_userRepository.HasBankAccounts(id))
            {
                throw new ValidationException("Есть привязанные банковские аккаунты");
            }

            _userRepository.Delete(id);
        }
    }
}
