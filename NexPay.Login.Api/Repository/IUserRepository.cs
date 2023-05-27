using NexPay.Login.Api.Models;

namespace NexPay.Login.Api.Repository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get the list of existing users.
        /// </summary>
        /// <returns>A list of users.</returns>
        public List<User> GetUsers();

        /// <summary>
        /// Add a new user to the database.
        /// </summary>
        /// <param name="request">User object.</param>
        /// <returns>Newly added user Id.</returns>
        public string AddUser(User request);

        /// <summary>
        /// Validate a User using Email and Password.
        /// </summary>
        /// <param name="request">Request parameter having Email and Password.</param>
        /// <returns>A boolean value as true if User is valid and false if User is invalid.</returns>
        public User ValidateUser(LoginRequest request);

        /// <summary>
        /// To find if user exist with the same email address.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <returns>Either true or false.</returns>
        bool FindUserByEmail(string email);

        /// <summary>
        /// Get user by email address.
        /// </summary>
        /// <param name="email">Unique user email address.</param>
        /// <returns>User object</returns>
        User GetUserByEmail(string email);
    }
}
