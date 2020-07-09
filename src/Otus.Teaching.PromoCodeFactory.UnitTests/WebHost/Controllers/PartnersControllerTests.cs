using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using Xunit;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers
{
    public class PartnersControllerTests
    {
        [Fact]
        public async void CanNotSetNotActivePartnerPromoCodeLimitAsync()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            var partner = new Partner()
            {
                Id = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8"),
                Name = "Суперигрушки",
                IsActive = false,
            };
            var request = new SetPartnerPromoCodeLimitRequest();

            var mock = new Mock<IRepository<Partner>>();
            mock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);
            var controller = new PartnersController(mock.Object);
 
            // Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(partnerId, request);
 
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ShouldReturnBadRequest()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            var partner = new Partner()
            {
                Id = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8"),
                Name = "Суперигрушки",
                IsActive = false,
            };
            var request = new SetPartnerPromoCodeLimitRequest();

            var mock = new Mock<IRepository<Partner>>();
            mock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);
            var controller = new PartnersController(mock.Object);
 
            // Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(partnerId, request);
 
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}