using Microsoft.EntityFrameworkCore;
using NexPay.Login.Api.Models;

namespace NexPay.Login.Api.Context
{
    public class InMemoryDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "UsersDb");
        }
        public DbSet<User>? Users { get; set; }
    }
}
