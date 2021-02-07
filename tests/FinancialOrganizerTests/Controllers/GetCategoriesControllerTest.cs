using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialOrganizer.Controllers;
using FinancialOrganizer.Data.Interfaces;
using FinancialOrganizer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FinancialOrganizer.Tests.Controllers
{
    [TestClass]
    public class GetCategoriesControllerTest
    {
        private GetCategoriesController _controller;
        private IConfiguration _configuration;
        private IDbIntegration _dbIntegration;

        [TestInitialize]
        public void SetupMocks()
        {
            _configuration = Substitute.For<IConfiguration>();
            _dbIntegration = Substitute.For<IDbIntegration>();
            _controller = new GetCategoriesController(_configuration, _dbIntegration);
        }

        [TestMethod]
        public void ReceivesOKResult_WhenListExists()
        {
            // Arrange
            var expectedList = GetCategories();
            
            // Act
            _dbIntegration.RetrieveAllCategories().Returns(GetCategories());
            var response = _controller.Get();
            var result = response.As<ObjectResult>();
            var content = result.Value.As<List<Category>>();
            
            // Assert
            _dbIntegration.Received(1).RetrieveAllCategories();
            result.StatusCode.Should().Be(200);
            content.Should().BeEquivalentTo(expectedList);
        }

        [TestMethod]
        public void ReceivesInternalServerError_WhenExceptionOccursOnRetrieval()
        {
            // Act
            _dbIntegration.RetrieveAllCategories().Throws(new ArgumentNullException());
            var response = _controller.Get();
            var result = response.As<IStatusCodeActionResult>();
            
            // Assert
            _dbIntegration.Received(1).RetrieveAllCategories();
            result.StatusCode.Should().Be(500);
        }

        private List<Category> GetCategories()
        {
            var categoryList = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Incomes"
                },
                new Category
                {
                    Id = 2,
                    Name = "Reserves"
                }
            };

            return categoryList;
        }
    }
}