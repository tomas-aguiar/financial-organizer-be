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
            var result = Context.Category
                .SingleOrDefault(c => c.Name.Equals(name));

            return result;
        }

        public List<Category> RetrieveMultipleCategories(List<string> listName)
        {
            return listName.Select(RetrieveCategory).ToList();
        }

        public List<Category> RetrieveAllCategories()
        {
            var result = Context.Category
                .ToList();

            return result;
        }
        
        public async Task<Category> InsertCategory(Category newCategory)
        {
            var result = await Context.Category.AddAsync(newCategory);
            await Context.SaveChangesAsync();

            return result.Entity;
        }
        #endregion
        
        #region Costs
        public Cost RetrieveCost(string name)
        {
            var result = Context.Cost
                .Single(c => c.Name.Equals(name));

            return result;
        }

        public List<Cost> RetrieveMultipleCosts(List<string> listName)
        {
            return listName.Select(RetrieveCost).ToList();
        }
        
        public List<Cost> RetrieveAllCosts()
        {
            var result = Context.Cost
                .ToList();

            return result;
        }
        #endregion
    }
}