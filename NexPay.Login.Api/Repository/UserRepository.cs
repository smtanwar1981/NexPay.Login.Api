using NexPay.Login.Api.Context;
using NexPay.Login.Api.Models;

namespace NexPay.Login.Api.Repository
{
    public class UserRepository : IUserRepository
    {
        /// <inheritdoc />
        public UserRepository()
        {
            using (var context = new InMemoryDbContext())
            {
                if (context.Users.Count() < 1)
                {
                    var users = new List<User>
                    {
                        new User { Id = Guid.NewGuid().ToString(), Email = "smtanwar@gmail.com", FirstName = "Sandeep", LastName = "Tanwar", Password = "nexpay" },
                        new User { Id = Guid.NewGuid().ToString(), Email = "admin@gmail.com", FirstName = "Admin", LastName = "Admin", Password = "nexpay", IsAdmin = true },
                        new User { Id = Guid.NewGuid().ToString(), Email = "superadmin@gmail.com", FirstName = "Super", LastName = "Admin", Password = "nexpay", IsAdmin = true },
                    };
                    context.Users?.AddRange(users);
                    context.SaveChanges();
                }
            };
        }

        /// <inheritdoc />
        public User GetUserByEmail(string email)
        {
            User user = null;
            using (var context = new InMemoryDbContext())
            { 
                user = context.Users.FirstOrDefault(x => x.Email == email);
            }
            return user;
        }

        /// <inheritdoc />
        public List<User> GetUsers()
        {
            using (var context = new InMemoryDbContext())
            {
                var list = context.Users?.ToList();
                return list;
            };
        }

        /// <inheritdoc />
        public string AddUser(User request)
        {
            string id = string.Empty;
            using (var context = new InMemoryDbContext())
            {
                request.Id = Guid.NewGuid().ToString();
                context.Users?.Add(request);
                context.SaveChanges();
            };
            return request.Id;
        }

        /// <inheritdoc />
        public bool FindUserByEmail(string email)
        {
            bool isEmailDuplicate = false;
            using (var context = new InMemoryDbContext())
            { 
                var existingUser = context.Users?.FirstOrDefault(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
                if (existingUser != null)
                { 
                    isEmailDuplicate = true;
                }
            }
            return isEmailDuplicate;
        }

        /// <inheritdoc />
        public User ValidateUser(LoginRequest request)
        {
            User validUser = null;
            using (var context = new InMemoryDbContext())
            {
                validUser = context.Users.Where(x => x.Email == request.UserName && x.Password == request.Password).FirstOrDefault();
            };
            return validUser;
        }
    }
}
