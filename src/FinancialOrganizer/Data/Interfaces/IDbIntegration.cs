﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialOrganizer.Models;

namespace FinancialOrganizer.Data.Interfaces
{
    public interface IDbIntegration
    {
        DataContext Context { get; }
        Category RetrieveCategory(string name);
        List<Category> RetrieveMultipleCategories(List<string> listName);
        List<Category> RetrieveAllCategories();
        Task<Category> InsertCategory(Category newCategory);
    }
}