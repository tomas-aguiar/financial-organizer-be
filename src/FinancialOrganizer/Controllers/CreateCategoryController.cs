using System;
using System.Data.Common;
using System.Threading.Tasks;
using FinancialOrganizer.Data;
using FinancialOrganizer.Data.Interfaces;
using FinancialOrganizer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinancialOrganizer.Controllers
{
    [ApiController]
    public class CreateCategoryController : ControllerBase
    {
        private IDbIntegration _dbIntegration;

        public CreateCategoryController(IConfiguration configuration, IDbIntegration dbIntegration = null)
        {
            _dbIntegration = dbIntegration ?? new DbIntegration(configuration);
        }
        
        [HttpPost]
        [Route("api/v1/CreateCategory")]
        public async Task<IActionResult> Post([FromBody] Category newCategory)
        {
            if (newCategory == null)
                return BadRequest();

            try
            {
                var existingCategory = GetCategory(newCategory.Name);
                if (existingCategory != null) 
                    return Conflict(existingCategory);
                
                var addedCategory = await _dbIntegration.InsertCategory(newCategory);

                return Ok(addedCategory);
            }
            catch (DbException)
            {
                return StatusCode(500);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500);
            }
        }

        private Category GetCategory(string newCategoryName)
        {
            var result = _dbIntegration.RetrieveCategory(newCategoryName);

            return result;
        }
    }
}