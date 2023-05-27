using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NexPay.Login.Api.Controllers;
using NexPay.Login.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NexPay.Login.Api.Tests.IntegrationTests
{
    public class SecurityControllerTests
    {
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<ILogger<SecurityController>> _logger;

        public SecurityControllerTests()
        {
            _tokenService = new Mock<ITokenService>();
            _logger = new Mock<ILogger<SecurityController>>();  
        }

        [Fact]
        public void ValidateToken_should_400_bad_request_response()
        {
            string token = string.Empty;

            //arrange
            var emailAddress = string.Empty;
            _tokenService.Setup(x => x.ValidateToken(token))
                    .Returns(emailAddress);
            var usersController = new SecurityController(_tokenService.Object, _logger.Object);

            // act
            var result = usersController.ValidateToken(token);
            var statusCodeResult = (IStatusCodeActionResult)result;

            // assert
            Assert.Equal(400, statusCodeResult.StatusCode);
            Assert.NotNull(result);
        }

        [Fact]
        public void ValidateToken_should_return_valid_response()
        {
            string token = "ehaytenstsetea";

            //arrange
            var emailAddress = "test@test.com";
            _tokenService.Setup(x => x.ValidateToken(token))
                    .Returns(emailAddress);
            var usersController = new SecurityController(_tokenService.Object, _logger.Object);

            // act
            var result = usersController.ValidateToken(token);
            var statusCodeResult = (IStatusCodeActionResult)result;

            // assert
            Assert.Equal(200, statusCodeResult.StatusCode);
            Assert.NotNull(result);
        }
    }
}
