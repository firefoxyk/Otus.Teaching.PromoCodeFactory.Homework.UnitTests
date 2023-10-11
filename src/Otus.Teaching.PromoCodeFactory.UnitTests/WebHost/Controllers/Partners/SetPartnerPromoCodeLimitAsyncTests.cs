using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Dsl;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Namotion.Reflection;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Builder;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using Xunit;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;
        private readonly SetPartnerPromoCodeLimitRequest _partnersPromoCodeLimitRequest;
        private readonly Guid _partnerId;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
            _partnersPromoCodeLimitRequest = fixture.Build<SetPartnerPromoCodeLimitRequest>().Create();
            _partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
        }

        public Partner CreateBasePartner()
        {
            var partner = new Partner()
            {
                Id = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8"),
                Name = "Суперигрушки",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>() {
                    new PartnerPromoCodeLimit() {
                        Id = Guid.Parse("e00633a5-978a-420e-a7d6-3e1dab116393"),
                        CreateDate = new DateTime(2020, 07, 9),
                        EndDate = new DateTime(2020, 10, 9),
                        Limit = 100
                    }
                }
            };

            return partner;
        }

        public PartnerPromoCodeLimit CreatePartnerPromoCodeLimit(DateTime date)
        {
            return new PartnerPromoCodeLimit()
            {
                Id = Guid.Parse("e00633a5-978a-420e-a7d6-3e1dab116393"),
                CreateDate = new DateTime(2020, 07, 9),
                EndDate = date,
                Limit = 100
            };
        }


        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            Partner partner = null;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(_partnerId))
                .ReturnsAsync(partner);

            // Act
            var result =
                await _partnersController.SetPartnerPromoCodeLimitAsync(_partnerId, _partnersPromoCodeLimitRequest);

            // Assert
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequest()
        {
            // Arrange
            var partner = CreateBasePartner();
            partner.IsActive = false;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(_partnerId))
                .ReturnsAsync(partner);

            // Act
            var result =
                await _partnersController.SetPartnerPromoCodeLimitAsync(_partnerId, _partnersPromoCodeLimitRequest);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void
            SetPartnerPromoCodeLimitAsync_SetNewLimit_ReturnsBadRequest_ActiveLimitCancelDate_DateTimeNow()
        {
            // Arrange
            var partner = CreateBasePartner();
            var date = DateTime.Now.AddMonths(1);
            partner.PartnerLimits = new List<PartnerPromoCodeLimit>() { CreatePartnerPromoCodeLimit(date) };
            var expectedDate = DateTime.Now.AddMonths(1).Month;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(_partnerId))
                .ReturnsAsync(partner);

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(_partnerId, _partnersPromoCodeLimitRequest);

            // Assert
            partner.PartnerLimits.ElementAt(0).EndDate.Month.Should().Be(expectedDate);
        }


        [Fact]
        public async void
            SetPartnerPromoCodeLimitAsync_SetNewLimit_LowerNull_ReturnBadRequest()
        {
            // Arrange
            var partner = CreateBasePartner();
            _partnersPromoCodeLimitRequest.Limit = -1;
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(_partnerId))
                .ReturnsAsync(partner);

            // Act
            var result =
                await _partnersController.SetPartnerPromoCodeLimitAsync(_partnerId, _partnersPromoCodeLimitRequest);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsyncTests_ValidSave_SuccessUpdate()
        {
            // Arrange
            var partner = PartnersBuilder.CreateBasePartner();
            var partnerId = partner.Id;
            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            _partnersRepositoryMock.Verify(repo => repo.UpdateAsync(partner), Times.Once);
        }



        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        public async void
        SetPartnerPromoCodeLimitAsync_RequestNotNull_NumberIssuedPromoCodes_is_0_And_RequestDateBeforeNow_NumberIssuedPromoCodes_TheSame(int months)
        {
            // Arrange
            var partner = CreateBasePartner();
            int expectedNum = 10;
            partner.NumberIssuedPromoCodes = expectedNum;
            _partnersPromoCodeLimitRequest.EndDate = DateTime.Now.AddMonths(months);
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(_partnerId))
                .ReturnsAsync(partner);

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(_partnerId, _partnersPromoCodeLimitRequest);

            switch (months)
            {
                // Assert
                case 1:
                    partner.NumberIssuedPromoCodes.Should().Be(0);
                    break;
                case -1:
                    partner.NumberIssuedPromoCodes.Should().Be(expectedNum);
                    break;
            }
        }
    }
}
