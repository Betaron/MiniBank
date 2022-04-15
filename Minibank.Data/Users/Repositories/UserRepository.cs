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

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(it => 
                    it.Id == id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Пользователь с id {id} не найден");
            }

            return new User
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email
            };
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            var data= _context.Users.AsNoTracking().Select(it => new User()
            {
                Id = it.Id,
                Login = it.Login,
                Email = it.Email
            });

            return await data.ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid(),
                Login = user.Login,
                Email = user.Email
            };

            await _context.Users.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(it => 
                    it.Id == user.Id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Пользователь с id {user.Id} не найден");
            }

            entity.Login = user.Login;
            entity.Email = user.Email;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(it => 
                    it.Id == id, cancellationToken);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Пользователь с id {id} не найден");
            }

            _context.Users.Remove(entity);
        }

        public Task<bool> UserExistsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Users.AnyAsync(it =>
                it.Id == id, cancellationToken);
        }

        public Task<bool> UserExistsByLoginAsync(string login, CancellationToken cancellationToken)
        {
            return _context.Users.AnyAsync(it =>
                it.Login == login, cancellationToken);
        }
    }
}
