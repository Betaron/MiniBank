using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;
using Minibank.Data.BankAccounts.Repositories;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        internal static List<UserDbModel> UsersStorage { get; } = new();

        public User GetById(string id)
        {
            var entity = UsersStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return new User
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email
            };
        }

        public IEnumerable<User> GetAll()
        {
            if (UsersStorage.Count == 0)
            {
                throw new NotFoundException();
            }

            return UsersStorage.Select(it => new User()
            {
                Id = it.Id,
                Login = it.Login,
                Email = it.Email
            });
        }

        public void Create(User user)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Login = user.Login,
                Email = user.Email
            };

            UsersStorage.Add(entity);
        }

        public void Update(User user)
        {
            var entity = UsersStorage.FirstOrDefault(it => it.Id == user.Id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            entity.Login = user.Login;
            entity.Email = user.Email;
        }

        public void Delete(string id)
        {
            var entity = UsersStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            UsersStorage.Remove(entity);
        }

        public bool HasBankAccounts(string id)
        {
            return 
                BankAccountRepository.AccountsStorage.FirstOrDefault(it => 
                    it.UserId == id) is not null;
;       }
    }
}
