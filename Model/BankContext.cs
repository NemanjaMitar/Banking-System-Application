using System.Data.Entity;

namespace BankingSystem.Model
{
    public class BankContext : DbContext
    {
        public BankContext() : base("name=BankingSystemDb")
        {
        }

        public DbSet<AccountBase> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}