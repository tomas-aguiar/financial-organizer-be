using System;
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
    [Route("api/v1/account")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AccountController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account newAccount)
        {
            if (newAccount == null || newAccount.AccountTypeId == 0) return BadRequest();
            
            if (FindCategory(newAccount.Name, newAccount.AccountTypeId).Any()) return Conflict();
            
            await CreateCategory(newAccount);

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
            catch (AggregateException)
            {
                return NotFound("AccountType not found.");
            }
        }

        private async Task CreateCategory(Account newAccount)
        {
            newAccount.AccountType = await FindAccountTypeById(newAccount);
            await _dataContext.Accounts.AddAsync(newAccount);
        }

        private async Task<AccountType> FindAccountTypeById(Account newAccount)
        {
            return await _dataContext.AccountTypes.FindAsync(newAccount.AccountTypeId);
        }

        private IQueryable<Account> FindCategory(string name, int accountTypeId)
        {
            return _dataContext.Accounts
                .Where(a => 
                    a.Name == name
                    && a.AccountTypeId == accountTypeId 
                    && a.Status.Equals(true));
        }
    }
}