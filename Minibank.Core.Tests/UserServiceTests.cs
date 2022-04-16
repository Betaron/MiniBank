using System;
using Minibank.Core.Domains.Users.Services;
using Xunit;
using Moq;

namespace Minibank.Core.Tests
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;
        
        public UserServiceTests(IUserService userService)
        {
            _userService = userService;
        }
    }
}
