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
    public class AccountControllerTest
    {
        private readonly AccountController _controller;
        private readonly DataContext _dbContext;
        
        public AccountControllerTest()
        {
            var configuration = Substitute.For<IConfiguration>();
            _dbContext = new DataContext(configuration, "test-database-account");
            _controller = new AccountController(_dbContext);
        }

        [TestMethod("Returns bad request response When request is null")]
        public async Task ReturnsBadRequest_WhenRequestIsNull()
        {
            var response = await _controller.Post(null);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        
        [TestMethod("Returns bad request response When request has empty accountTypeId")]
        public async Task ReturnsBadRequest_WhenRequestHasEmptyAccountTypeId()
        {
            var request = new Account
            {
                Balance = 10,
                Name = "test-account",
                Status = true
            };
            
            var response = await _controller.Post(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        
        [TestMethod("Returns ok response When request is valid")]
        public async Task ReturnsOK_WhenRequestIsValid()
        {
            var request = new Account
            {
                AccountType = new AccountType
                {
                    Id = 1,
                    Name = "test-account-type"
                },
                AccountTypeId = 1,
                Balance = 10,
                Name = "test-account",
                Status = true
            };
            
            var response = await _controller.Post(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK);
            request.Id.Should().Be(1);
        }
        
        [TestMethod("Returns conflict When request is duplicated")]
        public async Task ReturnsConflict_WhenRequestIsDuplicated()
        {
            var request = new Account
            {
                AccountTypeId = 1,
                Balance = 10,
                Name = "test-account-2",
                Status = true
            };
            await _dbContext.Accounts.AddAsync(request);
            await _dbContext.SaveChangesAsync();
            
            var response = await _controller.Post(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        }

        [TestCleanup]
        public void CleanUpDatabase()
        {
            _dbContext.Dispose();
        }
    }
}