using NexPay.Login.Api.Models;

namespace NexPay.Login.Api.Service
{
    /// <summary>
    /// A service contract to perform security token operations
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// This method will create jwt security token based on the user credentials.
        /// </summary>
        /// <param name="request">Request parameter having user credentials.</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A token as string.</returns>
        public Task<LoginRespose> GenerateToken(LoginRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// This method will validate an existing token. 
        /// </summary>
        /// <param name="token">Token string.</param>
        /// <returns>An email id.</returns>
        public string? ValidateToken(string token);
    }
}
