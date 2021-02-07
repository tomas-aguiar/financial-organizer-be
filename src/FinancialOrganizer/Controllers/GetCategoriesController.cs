using System;
using System.Threading.Tasks;
using FinancialOrganizer.Data;
using FinancialOrganizer.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FinancialOrganizer.Controllers
{
    [ApiController]
    public class GetCategoriesController : ControllerBase
    {
        private readonly IDbIntegration _dbIntegration;

        public GetCategoriesController(IConfiguration configuration, IDbIntegration dbIntegration = null)
        {
            _dbIntegration = dbIntegration ?? new DbIntegration(configuration);
        }
        
        [HttpGet]
        [Route("api/v1/GetCategories")]
        public IActionResult Get()
        {
            try
            {
                var categories = _dbIntegration.RetrieveAllCategories();

                return Ok(categories);
            }
            catch (ArgumentNullException)
            {
                return StatusCode(500);
            }
        }
    }
}