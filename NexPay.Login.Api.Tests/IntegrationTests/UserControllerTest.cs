using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NexPay.Login.Api.Controllers;
using NexPay.Login.Api.Models;
using NexPay.Login.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NexPay.Login.Api.Tests
{
    public  class UserControllerTest
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<ILogger<UserController>> _logger;

        public UserControllerTest()
        {
            _userService = new Mock<IUserService>();
            _logger = new Mock<ILogger<UserController>>();
        }

        [Fact]
        public void GetAllUsers_should_return_list_of_users()
        {
            //arrange
            var mockUsersList = GetUsersList();
            _userService.Setup(x => x.GetAllUser())
                    .Returns(mockUsersList);
            var usersController = new UserController(_userService.Object, _logger.Object);

            // act
            var result = usersController.GetAllUsers();
            var statusCodeResult = (IStatusCodeActionResult)result;

            // assert
            Assert.Equal(200, statusCodeResult.StatusCode);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAllUsers_should_return_404_status_code_when_no_user_found()
        {
            //arrange
            var mockUsersList = new List<User>();
            _userService.Setup(x => x.GetAllUser())
                    .Returns(mockUsersList);
            var usersController = new UserController(_userService.Object, _logger.Object);

            // act
            var result = usersController.GetAllUsers();
            var statusCodeResult = (IStatusCodeActionResult)result;

            // assert
            Assert.Equal(404, statusCodeResult.StatusCode);
            Assert.NotNull(result);
        }

        private List<User> GetUsersList()
        {
            return new List<User>
            { 
                new User
                { 
                    Id = new Guid().ToString(),
                    Email = "test1@test.com",
                    FirstName = "fname1",
                    LastName = "lname1",
                    Password = "test123"
                },
                new User
                {
                    Id = new Guid().ToString(),
                    Email = "test2@test.com",
                    FirstName = "fname2",
                    LastName = "lname2",
                    Password = "test123"
                },
                new User
                {
                    Id = new Guid().ToString(),
                    Email = "test3@test.com",
                    FirstName = "fname3",
                    LastName = "lname3",
                    Password = "test123"
                }
            };
        }
    }
}
