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
    [Route("api/v1/accountType")]
    public class AccountTypeController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AccountTypeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountType newAccountType)
        {
            if (newAccountType == null) return BadRequest();

            if (FindAccountTypeByName(newAccountType.Name).Any()) return Conflict();

            await _dataContext.AccountTypes.AddAsync(newAccountType);

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

        private IQueryable<AccountType> FindAccountTypeByName(string name)
        {
            return _dataContext
                .AccountTypes
                .Where(t => t.Name == name);
        }
    }
}