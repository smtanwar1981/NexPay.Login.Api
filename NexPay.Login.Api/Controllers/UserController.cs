using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexPay.Login.Api.Models;
using NexPay.Login.Api.Service;

namespace NexPay.Login.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService; 
            _logger = logger;   
        }

        [HttpPost("registerUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRegistrationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult>? RegisterUser(User request)
        {
            _logger.LogInformation($"Begin executing RegisterUser() of {nameof(UserController)} class.");

            if (request == null)
            {
                return BadRequest("Request can not be null or empty.");
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest($"{nameof(request.Email)} can not be null or empty.");
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                return BadRequest($"{nameof(request.Password)} can not be null or empty.");
            }
            var result = await _userService.AddNewUser(request);

            _logger.LogInformation($"Finish executing RegisterUser() of {nameof(UserController)} class.");
            return Ok(result);
        }

        [HttpGet("getUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllUsers()
        {
            _logger.LogInformation($"Begin executing GetAllUsers() of {nameof(UserController)} class.");

            var result = _userService.GetAllUser();
            if (result is null || result.Count == 0)
            {
                return NotFound("No user found in the system.");
            }

            _logger.LogInformation($"Finish executing GetAllUsers() of {nameof(UserController)} class.");
            return Ok(result);
        }
    }
}
