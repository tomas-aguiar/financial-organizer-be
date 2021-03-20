using Microsoft.EntityFrameworkCore;
using FinancialOrganizer.Models;
using Microsoft.Extensions.Configuration;

namespace FinancialOrganizer.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly bool _isDefaultConnection;
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Flexibility> Flexibilities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Periodicity> Periodicities { get; set; }

        public DataContext(IConfiguration configuration, string connectionString = null)
        {
            _configuration = configuration;
            if (string.IsNullOrEmpty(connectionString))
            {
                _connectionString = _configuration.GetConnectionString("DefaultConnectionString");
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
