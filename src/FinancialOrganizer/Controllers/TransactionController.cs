using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using FinancialOrganizer.Data;
using FinancialOrganizer.Handlers;
using FinancialOrganizer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialOrganizer.Controllers
{
    [ApiController]
    [Route("api/v1/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ITransactionHandler _transactionHandler;

        public TransactionController(DataContext dataContext, ITransactionHandler transactionHandler)
        {
            _dataContext = dataContext;
            _transactionHandler = transactionHandler;
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Transaction newTransaction)
        {
            if (newTransaction == null 
                || newTransaction.AccountId == 0 
                || newTransaction.CategoryId == 0) return BadRequest();

            newTransaction.Account = await _dataContext.Accounts.FindAsync(newTransaction.AccountId);
            newTransaction.Category = await _dataContext.Categories.FindAsync(newTransaction.CategoryId);
            if (newTransaction.Account == null 
                || newTransaction.Category == null) return NotFound("Account not found.");
            
            var transactionCreated = await _dataContext.Transactions.AddAsync(newTransaction);

            try
            {
                await _dataContext.SaveChangesAsync();
                await _transactionHandler.Handle(transactionCreated.Entity);
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
            catch (KeyNotFoundException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}