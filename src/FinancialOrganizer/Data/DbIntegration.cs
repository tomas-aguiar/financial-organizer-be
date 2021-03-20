using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialOrganizer.Data.Interfaces;
using FinancialOrganizer.Models;
using Microsoft.Extensions.Configuration;

namespace FinancialOrganizer.Data
{
    public class DbIntegration : IDbIntegration
    {
        public DataContext Context { get; }

        public DbIntegration(IConfiguration configuration)
        {
            Context = new DataContext(configuration);
        }

        #region Categories
        public Category RetrieveCategory(string name)
        {
            var result = Context.Categories
                .SingleOrDefault(c => c.Name.Equals(name));

            return result;
        }

        public List<Category> RetrieveMultipleCategories(List<string> listName)
        {
            return listName.Select(RetrieveCategory).ToList();
        }

        public List<Category> RetrieveAllCategories()
        {
            var result = Context.Categories
                .ToList();

            return result;
        }
        
        public async Task<Category> InsertCategory(Category newCategory)
        {
            var result = await Context.Categories.AddAsync(newCategory);
            await Context.SaveChangesAsync();

            return result.Entity;
        }
        #endregion
        
        #region Flexibility
        public Flexibility RetrieveFlexibility(string name)
        {
            var result = Context.Flexibilities
                .Single(c => c.Name.Equals(name));

            return result;
        }

        public List<Flexibility> RetrieveMultipleFlexibilities(List<string> listName)
        {
            return listName.Select(RetrieveFlexibility).ToList();
        }
        
        public List<Flexibility> RetrieveAllFlexibilities()
        {
            var result = Context.Flexibilities
                .ToList();

            return result;
        }
        #endregion

        #region Order

        public Order RetrieveOrder(string name)
        {
            var result = Context.Orders.Single(o => o.Name.Equals(name));

            return result;
        }

        public List<Order> RetrieveMultipleOrders(List<string> names)
        {
            return names.Select(RetrieveOrder).ToList();
        }

        public List<Order> RetrieveAllOrders()
        {
            var result = Context.Orders.ToList();

            return result;
        }
        #endregion
        
        #region Periodicity

        public Periodicity RetrievePeriodicity(string name)
        {
            var result = Context.Periodicities.SingleOrDefault(p => p.Name.Equals(name));

            return result;
        }

        public List<Periodicity> RetrieveMultiplePeriodicities(List<string> names)
        {
            return names.Select(RetrievePeriodicity).ToList();
        }

        public List<Periodicity> RetrieveAllPeriodicities()
        {
            var result = Context.Periodicities.ToList();

            return result;
        }
        #endregion
    }
}