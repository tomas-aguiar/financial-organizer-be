using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FinancialOrganizer.Data;
using FinancialOrganizer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialOrganizer.Controllers
{
    [ApiController]
    [Route("api/v1/category")]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category newCategory)
        {
            if (newCategory == null || newCategory.CostTypeId == 0) return BadRequest();

            if (FindExistingCategory(newCategory).Any()) return Conflict();

            newCategory.CostType = await FindCostTypeById(newCategory);
            if (newCategory.CostType == null) return NotFound("CostType not found.");

            try
            {
                await CreateCategory(newCategory);
                return Ok();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (DBConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task CreateCategory(Category newCategory)
        {
            await _dataContext.Categories.AddAsync(newCategory);
            await _dataContext.SaveChangesAsync();
        }

        private async Task<CostType> FindCostTypeById(Category newCategory)
        {
            return await _dataContext.CostTypes.FindAsync(newCategory.CostTypeId);
        }

        private IQueryable<Category> FindExistingCategory(Category newCategory)
        {
            return _dataContext
                .Categories.Where(c => 
                    c.Name == newCategory.Name 
                    && c.CostTypeId == newCategory.CostTypeId);
        }
    }
}