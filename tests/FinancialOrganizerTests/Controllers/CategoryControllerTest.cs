using System.Threading.Tasks;
using FinancialOrganizer.Controllers;
using FinancialOrganizer.Data;
using FinancialOrganizer.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace FinancialOrganizer.Tests.Controllers
{
    [TestClass]
    public class CategoryControllerTest
    {
        private readonly CategoryController _controller;
        private readonly DataContext _dataContext;

        public CategoryControllerTest()
        {
            var configuration = Substitute.For<IConfiguration>();
            _dataContext = new DataContext(configuration, "test-database-category");
            _controller = new CategoryController(_dataContext);
        }

        [TestMethod("Returns ok response When request is valid")]
        public async Task ReturnsOk_WhenRequestIsValid()
        {
            var costType = new CostType
            {
                Id = 1,
                Name = "cost-type-test"
            };
            await _dataContext.CostTypes.AddAsync(costType);
            await _dataContext.SaveChangesAsync();
            var request = new Category
            {
                CostTypeId = 1,
                Name = "category-test"
            };

            var response = await _controller.Post(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK);
            request.Id.Should().Be(1);
        }
        
        [TestMethod("Returns bad request response When request is null")]
        public async Task ReturnsBadRequest_WhenRequestIsNull()
        {
            var response = await _controller.Post(null);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        
        [TestMethod("Returns conflict response When request is duplicated")]
        public async Task ReturnsConflict_WhenRequestIsDuplicated()
        {
            var request = new Category
            {
                CostTypeId = 1,
                Name = "category-test-2"
            };
            await _dataContext.Categories.AddAsync(request);
            await _dataContext.SaveChangesAsync();

            var response = await _controller.Post(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        }
        
        [TestMethod("Returns bad request response When costTypeId is not present")]
        public async Task ReturnsBadRequest_WhenCostTypeIdIsMissing()
        {
            var request = new Category
            {
                Name = "category-test-3"
            };

            var response = await _controller.Post(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        
        [TestMethod("Returns not found response When costTypeId is invalid")]
        public async Task ReturnsNotFound_WhenCostTypeIdIsInvalid()
        {
            var request = new Category
            {
                CostTypeId = 2,
                Name = "category-test-3"
            };

            var response = await _controller.Post(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [TestCleanup]
        public void CleanUpDatabase()
        {
            _dataContext.Dispose();
        }
    }
}