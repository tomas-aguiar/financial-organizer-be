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
    [Route("api/v1/costType")]
    public class CostTypeController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CostTypeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CostType newCostType)
        {
            if (newCostType == null) return BadRequest();

            if (FindCostTypesByName(newCostType.Name).Any()) return Conflict();
            
            await _dataContext.CostTypes.AddAsync(newCostType);

            try
            {
                await _dataContext.SaveChangesAsync();
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

        private IQueryable<CostType> FindCostTypesByName(string name)
        {
            return _dataContext.CostTypes.Where(t => t.Name == name);
        }
    }
}