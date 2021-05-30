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
    public class AccountTypeControllerTest
    {
        private readonly AccountTypeController _controller;
        private readonly DataContext _dataContext;

        public AccountTypeControllerTest()
        {
            var configuration = Substitute.For<IConfiguration>();
            _dataContext = new DataContext(configuration, "test-database-account-type");
            _controller = new AccountTypeController(_dataContext);
        }

        [TestMethod("Returns bad request When request is null")]
        public async Task ReturnsBadRequest_WhenRequestIsNull()
        {
            var response = await _controller.Post(null);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        
        [TestMethod("Returns ok response When request is valid")]
        public async Task ReturnsOK_WhenRequestIsValid()
        {
            var request = new AccountType
            {
                Name = "account-type-test-1"
            };
            
            var response = await _controller.Post(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK);
            request.Id.Should().Be(1);
        }

        [TestMethod("Returns conflict response When account type exists")]
        public async Task ReturnsConflict_WhenAccountTypeExists()
        {
            var request = new AccountType
            {
                Name = "account-type-test-2"
            };
            await _dataContext.AccountTypes.AddAsync(request);
            await _dataContext.SaveChangesAsync();
            
            var response = await _controller.Post(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        }

        [TestCleanup]
        public void CleanUpDatabase()
        {
            _dataContext.Dispose();
        }
    }
}