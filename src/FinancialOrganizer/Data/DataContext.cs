using FinancialOrganizer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinancialOrganizer.Data
{
    public class DataContext : DbContext
    {
        private readonly string _connectionString;
        private readonly bool _isDefaultConnection;
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CostType> CostTypes { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }

        public DataContext(IConfiguration configuration, string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                _connectionString = configuration.GetConnectionString("DefaultConnectionString");
                _isDefaultConnection = true;
            }
            else
            {
                _connectionString = connectionString;
                _isDefaultConnection = false;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_isDefaultConnection)
            {
                optionsBuilder.UseMySQL(_connectionString);
            }
            else
            {
                optionsBuilder.UseInMemoryDatabase(_connectionString);
            }
        }
    }
}
