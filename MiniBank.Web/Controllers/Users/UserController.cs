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
        /// <returns>Found user</returns>
        [HttpGet("{id}")]
        public UserDto GetById(string id)
        {
            var model = _userService.GetById(id);

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
        public IEnumerable<UserDto> GetAll()
        {
            return _userService.GetAll()
                .Select(it => new UserDto
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
        [HttpPost]
        public void Create(NewUserDto model)
        {
            _userService.Create(new User
            {
                Login = model.Login,
                Email = model.Email
            });
        }

        /// <summary>
        /// Changes user based on the passed
        /// </summary>
        /// <param name="model">User to be changed</param>
        [HttpPut("{id}")]
        public void Update(string id, NewUserDto model)
        {
            _userService.Update(new User
            {
                Id = id,
                Login = model.Login,
                Email = model.Email
            });
        }

        /// <summary>
        /// Deletes a user by id
        /// </summary>
        /// <param name="id">User identification number</param>
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _userService.Delete(id);
        }
    }
}
