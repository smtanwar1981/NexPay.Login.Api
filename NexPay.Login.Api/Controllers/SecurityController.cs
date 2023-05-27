using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexPay.Login.Api.Core;
using NexPay.Login.Api.Models;
using NexPay.Login.Api.Service;

namespace NexPay.Login.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<SecurityController> _logger;

        public SecurityController(ITokenService tokenService, ILogger<SecurityController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
            _logger.LogInformation("Intialized SecurityController class");
        }

        [AllowAnonymous]
        [HttpPost("createToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginRespose))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateToken(LoginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Begin Executing CreateToken() endpoint SecurityController class");
            
            LoginRespose token = null;
            if (request is not null)
            {
                _logger.LogInformation("Executing CreateToken endpoint with request: ",request);
                token = await _tokenService?.GenerateToken(request, cancellationToken);
            }
            _logger.LogInformation("End Executing CreateToken() endpoint SecurityController class");
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpGet("validateToken/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ValidateToken(string token)
        {
            _logger.LogInformation("Begin Executing ValidateToken() endpoint of SecurityController class");
            string? response = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    _logger.LogInformation("Executing ValidateToken() endpoint with request: ", token);
                    response = _tokenService?.ValidateToken(token);
                }
                else
                {
                    return BadRequest("Invalid token.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while executing ValidateToken() endpoint of SecurityController class - ", ex);
                return BadRequest(ex.Message);
            }
            _logger.LogInformation("End Executing ValidateToken() endpoint of SecurityController class");
            return Ok(response);
        }
    }
}
