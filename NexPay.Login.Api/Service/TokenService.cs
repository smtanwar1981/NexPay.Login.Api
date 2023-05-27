using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NexPay.Login.Api.Models;
using NexPay.Login.Api.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NexPay.Login.Api.Service
{
    /// <inheritdoc />
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository; 
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, IUserRepository userRepository, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<LoginRespose> GenerateToken(LoginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Begin Executing GenerateToken() method of TokenService class.");

            string response = string.Empty;
            await Task.Run(() =>
            {
                var validUser = IsValidUser(request);
                if (validUser != null)
                {
                    var issuer = _configuration["Jwt:Issuer"];
                    var audience = _configuration["Jwt:Audience"];
                    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                    var isAdmin = validUser.IsAdmin != null ? validUser.IsAdmin : false;
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("id", validUser.Id.ToString()),
                            new Claim("isAdmin", isAdmin.ToString(), ClaimValueTypes.Boolean),
                            new Claim("displayName", validUser.FirstName.ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, request.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, request.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["SessionTimeOutInMinutes"])),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    //var jwtToken = tokenHandler.WriteToken(token);
                    response = tokenHandler.WriteToken(token);
                }
                else
                {
                    //response = "Not a valid User, please register.";
                    _logger.LogInformation(response);
                    throw new Exception("User not registered.");
                }
            });

            _logger.LogInformation("End Executing GenerateToken() method of TokenService class.");
            var token = new LoginRespose { Token = response };
            return token;
        }

        /// <inheritdoc />
        public string? ValidateToken(string token)
        {
            _logger.LogInformation("Begin Executing ValidateToken() method of TokenService class.");

            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var email = Convert.ToString(jwtToken.Claims.First(x => x.Type == "email").Value);

                _logger.LogInformation("Valid token for User: ", email);
                return email;
            }
            catch
            {
                _logger.LogError("An exception occurred while executing ValidateToken() endpoint of TokenService class");
                throw;
            }
            finally
            {
                _logger.LogInformation("End Executing ValidateToken() method of TokenService class.");
            }
        }

        private User IsValidUser(LoginRequest user)
        {
            var validUser = _userRepository.ValidateUser(user);
            if (validUser is not null)
            {
                return validUser;
            }
            return null;
        }

    }
}
