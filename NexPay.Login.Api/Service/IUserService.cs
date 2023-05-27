using NexPay.Login.Api.Models;

namespace NexPay.Login.Api.Service
{
    public interface IUserService
    {
        /// <summary>
        /// Add a new user to the syste.
        /// </summary>
        /// <param name="request">User model.</param>
        /// <returns>True or false depending on user registration.</returns>
        Task<UserRegistrationResponse> AddNewUser(User request);

        /// <summary>
        /// Fetch list of Users.
        /// </summary>
        /// <returns>List of existing Users.</returns>
        List<User> GetAllUser();
    }
}
