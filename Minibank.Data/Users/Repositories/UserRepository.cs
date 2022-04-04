using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MinibankContext _context;

        public UserRepository(MinibankContext context)
        {
            _context = context;
        }

        public User GetById(string id)
        {
            var entity = _context.Users
                .AsNoTracking().FirstOrDefault(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Пользователь с id {id} не найден");
            }

            return new User
            {
                Id = entity.Id.ToString(),
                Login = entity.Login,
                Email = entity.Email
            };
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.AsNoTracking().Select(it => new User()
            {
                Id = it.Id.ToString(),
                Login = it.Login,
                Email = it.Email
            });
        }

        public void Create(User user)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid(),
                Login = user.Login,
                Email = user.Email
            };

            _context.Users.Add(entity);
        }

        public void Update(User user)
        {
            var entity = _context.Users
                .FirstOrDefault(it => it.Id.ToString() == user.Id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Пользователь с id {user.Id} не найден");
            }

            entity.Login = user.Login;
            entity.Email = user.Email;
        }

        public void Delete(string id)
        {
            var entity = _context.Users
                .FirstOrDefault(it => it.Id.ToString() == id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Пользователь с id {id} не найден");
            }

            _context.Users.Remove(entity);
        }

        public bool Exists(string id)
        {
            return _context.Users.Any(it => it.Id.ToString() == id);
        }
    }
}
