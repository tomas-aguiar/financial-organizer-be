using System.IO;
using System.Net.Mime;
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
        
        public DbSet<Category> Category { get; set; }
        public DbSet<Cost> Cost { get; set; }

        public DataContext(IConfiguration configuration, string connectionString = null)
        {
            _configuration = configuration;
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
