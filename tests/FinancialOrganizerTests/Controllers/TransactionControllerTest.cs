using System;
using System.Threading.Tasks;
using FinancialOrganizer.Controllers;
using FinancialOrganizer.Data;
using FinancialOrganizer.Handlers;
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
    public class TransactionControllerTest
    {
        private readonly TransactionController _controller;
        private readonly DataContext _dataContext;
        private readonly ITransactionHandler _transactionHandler;

        public TransactionControllerTest()
        {
            var configuration = Substitute.For<IConfiguration>();
            _dataContext = new DataContext(configuration, "test-database-transaction");
            _transactionHandler = new TransactionHandler(_dataContext);
            _controller = new TransactionController(_dataContext, _transactionHandler);
        }

        [TestMethod("Returns no content When request is valid")]
        public async Task ReturnsNoContent_WhenRequestIsValid()
        {
            await _dataContext.Accounts.AddAsync(new Account
            {
                Id = 1,
                Name = "account-test",
            });
            await _dataContext.Categories.AddAsync(new Category
            {
                Id = 1,
                Name = "category-test"
            });
            await _dataContext.SaveChangesAsync();
            var request = new Transaction
            {
                Amount = 0.01m,
                TimeStamp = DateTime.Now,
                Description = "description-test",
                AccountId = 1,
                CategoryId = 1,
                IsBudget = false
            };

            var response = await _controller.Put(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK);
            request.Id.Should().Be(1);
        }
        
        [TestMethod("Returns bad request When request is null")]
        public async Task ReturnsBadRequest_WhenRequestIsNull()
        {
            var response = await _controller.Put(null);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        
        [TestMethod("Returns bad request When account is missing")]
        public async Task ReturnsBadRequest_WhenAccountIsMissing()
        {
            var request = new Transaction
            {
                Amount = 0.01m,
                TimeStamp = DateTime.Now,
                Description = "description-test",
                CategoryId = 1,
                IsBudget = false
            };

            var response = await _controller.Put(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        
        [TestMethod("Returns not found When account is invalid")]
        public async Task ReturnsNotFound_WhenAccountIsInvalid()
        {
            var request = new Transaction
            {
                Amount = 0.01m,
                TimeStamp = DateTime.Now,
                Description = "description-test",
                AccountId = 2,
                CategoryId = 1,
                IsBudget = false
            };

            var response = await _controller.Put(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
        
        [TestMethod("Returns bad request When category is missing")]
        public async Task ReturnsBadRequest_WhenCategoryIsMissing()
        {
            var request = new Transaction
            {
                Amount = 0.01m,
                TimeStamp = DateTime.Now,
                Description = "description-test",
                AccountId = 1,
                IsBudget = false
            };

            var response = await _controller.Put(request);
            var responseStatus = response.As<IStatusCodeActionResult>();

            responseStatus.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        
        [TestMethod("Returns not found When category is invalid")]
        public async Task ReturnsNotFound_WhenCategoryIsInvalid()
        {
            var request = new Transaction
            {
                Amount = 0.01m,
                TimeStamp = DateTime.Now,
                Description = "description-test",
                AccountId = 1,
                CategoryId = 2,
                IsBudget = false
            };

            var response = await _controller.Put(request);
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