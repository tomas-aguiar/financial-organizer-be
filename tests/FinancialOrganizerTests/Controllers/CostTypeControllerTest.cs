﻿using System.Threading.Tasks;
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
    public class CostTypeControllerTest
    {
        private readonly CostTypeController _controller;
        private readonly DataContext _dataContext;

        public CostTypeControllerTest()
        {
            var configuration = Substitute.For<IConfiguration>();
            _dataContext = new DataContext(configuration, "test-database-cost-type");
            _controller = new CostTypeController(_dataContext);
        }

        [TestMethod("Returns ok response When request is valid")]
        public async Task ReturnsOk_WhenRequestIsValid()
        {
            var request = new CostType
            {
                Name = "cost-type-test"
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
            var request = new CostType
            {
                Name = "cost-type-test-2"
            };
            await _dataContext.CostTypes.AddAsync(request);
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