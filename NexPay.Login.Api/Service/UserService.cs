using NexPay.Login.Api.Models;
using NexPay.Login.Api.Repository;
using NexPay.Publisher.Service;
using System.Text.RegularExpressions;

namespace NexPay.Login.Api.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMessagePublisher _messagePublisher;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IMessagePublisher messagePublisher)
        {
            _userRepository = userRepository;
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        /// <inheritdoc />
        public async Task<UserRegistrationResponse> AddNewUser(User request)
        {
            _logger.LogInformation($"Begin executing AddNewUser() method of ${nameof(UserService)} class.");

            var response = new UserRegistrationResponse();
            if (request == null)
            {
                throw new ArgumentNullException($"Request can not be null or empty");
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentNullException($"{nameof(request.Email)} can not be null or empty.");
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentNullException($"{nameof(request.Password)} can not be null or empty.");
            }
            if (!IsEmailValid(request.Email))
            {
                throw new ArgumentException($"Invalid Email Address");
            }
            if (_userRepository.FindUserByEmail(request.Email))
            {
                throw new ArgumentException("Email already exist in the systemn, please use another email address.");
            }
            string addNewUserStatus = string.Empty;
            addNewUserStatus = _userRepository.AddUser(request);
            if (!string.IsNullOrEmpty(addNewUserStatus))
            {
                response.UserRegistered = true;
                var user = _userRepository.GetUserByEmail(request.Email);
                if (user != null)
                {
                    _messagePublisher.PublishMessage(user);
                }
            }
            else
            {
                response.UserRegistered = false;
            }

            _logger.LogInformation($"Finish executing AddNewUser() method of ${nameof(UserService)} class.");
            return response;
        }

        /// <inheritdoc />
        public List<User> GetAllUser()
        {
            _logger.LogInformation($"Begin executing GetAllUser() method of ${nameof(UserService)} class.");

            try
            {
                _logger.LogInformation($"Finish executing GetAllUser() method of ${nameof(UserService)} class.");
                return _userRepository.GetUsers().ToList();
            }
            catch
            {
                _logger.LogError("Unable to fetch users.");
                throw;
            }
        }

        private bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}
