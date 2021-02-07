using System.Threading.Tasks;
using FinancialOrganizer.Controllers;
using FinancialOrganizer.Data.Interfaces;
using FinancialOrganizer.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FinancialOrganizer.Tests.Controllers
{
    [TestClass]
    public class CreateCategoryControllerTest
    {
        private IConfiguration _configuration;
        private IDbIntegration _dbIntegration;
        private CreateCategoryController _controller;

        [TestInitialize]
        public void SetupMocks()
        {
            _configuration = Substitute.For<IConfiguration>();
            _dbIntegration = Substitute.For<IDbIntegration>();
            _controller = new CreateCategoryController(_configuration, _dbIntegration);
        }

        [TestMethod]
        public async Task ReturnsOKResult_WhenAbleToCreate()
        {
            // Arrange
            var newCategory = new Category
            {
                Name = "Test"
            };
            
            // Act
            _dbIntegration.InsertCategory(newCategory).Returns(GetReturnCategory());
            var response = await _controller.Post(newCategory);
            var responseStatus = response.As<ObjectResult>();
            var responseContent = responseStatus.Value.As<Category>();
            newCategory.Id = 1;

            // Assert
            await _dbIntegration.Received(1).InsertCategory(newCategory);
            responseStatus.StatusCode.Should().Be(200);
            responseContent.Should().BeEquivalentTo(newCategory);
        }

        [TestMethod]
        public async Task ReturnsConflictResult_WhenDuplicateCategory()
        {
            // Arrange
            var duplicatedCategory = GetReturnCategory();
            
            // Act
            _dbIntegration.RetrieveCategory(duplicatedCategory.Name).Returns(GetReturnCategory());
            var response = await _controller.Post(duplicatedCategory);
            var responseStatus = response.As<ObjectResult>();
            var responseContent = responseStatus.Value.As<Category>();
            
            // Assert
            _dbIntegration.Received(1).RetrieveCategory(duplicatedCategory.Name);
            responseStatus.StatusCode.Should().Be(409);
            responseContent.Should().BeEquivalentTo(duplicatedCategory);
        }

        [TestMethod]
        public async Task ReturnInternalServerError_WhenErrorOnSavingToDbOccurs()
        {
            // Arrange
            var category = GetReturnCategory();
            
            // Act
            _dbIntegration.RetrieveCategory(category.Name).Throws(new DbUpdateException());
            var response = await _controller.Post(category);
            var responseStatus = response.As<IStatusCodeActionResult>();
            
            // Assert
            _dbIntegration.Received(1).RetrieveCategory(category.Name);
            responseStatus.StatusCode.Should().Be(500);
        }
        
        [TestMethod]
        public async Task ReturnInternalServerError_WhenConcurrentErrorOnSavingToDbOccurs()
        {
            // Arrange
            var category = GetReturnCategory();
            
            // Act
            _dbIntegration.RetrieveCategory(category.Name).Throws(new DbUpdateConcurrencyException());
            var response = await _controller.Post(category);
            var responseStatus = response.As<IStatusCodeActionResult>();
            
            // Assert
            _dbIntegration.Received(1).RetrieveCategory(category.Name);
            responseStatus.StatusCode.Should().Be(500);
        }
        
        [TestMethod]
        public async Task ReturnBadRequest_WhenRequestIsNull()
        {
            // Act
            var response = await _controller.Post(null);
            var responseStatus = response.As<IStatusCodeActionResult>();
            
            // Assert
            responseStatus.StatusCode.Should().Be(400);
        }

        private static Category GetReturnCategory()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Test"
            };

            return category;
        }
    }
}