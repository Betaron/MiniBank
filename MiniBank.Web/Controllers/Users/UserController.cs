using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Controllers.Users.Dto;

namespace Minibank.Web.Controllers.Users
{
    [ApiController]
    [Route("user")]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Searches for a user by his id
        /// </summary>
        /// <param name="id">User identification number</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Found user</returns>
        [HttpGet("{id}")]
        public async Task<UserDto> GetById(string id, CancellationToken cancellationToken)
        {
            var model = await _userService.GetByIdAsync(id, cancellationToken);

            return new UserDto
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            };
        }

        /// <summary>
        /// Find all users
        /// </summary>
        /// <returns>All users from repository</returns>
        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAll(CancellationToken cancellationToken)
        {
            var models = await _userService.GetAllAsync(cancellationToken);

            return models.Select(it => new UserDto
                {
                    Id = it.Id,
                    Login = it.Login,
                    Email = it.Email
                });
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="model">Template user</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        public Task Create(CreateUserDto model, CancellationToken cancellationToken)
        {
            return _userService.CreateAsync(new User
            {
                Login = model.Login,
                Email = model.Email
            }, cancellationToken);
        }

        /// <summary>
        /// Changes user based on the passed
        /// </summary>
        /// <param name="model">User to be changed</param>
        [HttpPut("{id}")]
        public Task Update(
            string id, UpdateUserDto model, CancellationToken cancellationToken)
        {
            return _userService.UpdateAsync(new User
            {
                Id = id,
                Login = model.Login,
                Email = model.Email
            }, cancellationToken);
        }

        /// <summary>
        /// Deletes a user by id
        /// </summary>
        /// <param name="id">User identification number</param>
        [HttpDelete("{id}")]
        public Task Delete(string id, CancellationToken cancellationToken)
        {
            return _userService.DeleteAsync(id, cancellationToken);
        }
    }
}
