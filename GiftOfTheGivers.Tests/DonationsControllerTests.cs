using GiftOfTheGivers.Helpers;
using GiftOfTheGiversApp.Controllers;
using GiftOfTheGiversApp.Data;
using GiftOfTheGiversApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net;
using Xunit;

namespace GiftOfTheGivers.Tests
{
    public class DonationsControllerTests
    {
        // Each test gets its own empty in-memory database.
        private static ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private static DonationsController CreateController(ApplicationDbContext context, IHttpClientFactory? httpClientFactory = null)
        {
            var controller = new DonationsController(context, httpClientFactory ?? Mock.Of<IHttpClientFactory>());
            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>());

            return controller;
        }

        [Fact]
        public async Task CreateGoods_WithValidData_SavesDonationAndRedirects()
        {
            // Arrange
            using var context = CreateContext();
            var controller = CreateController(context);

            var goods = new GoodsDonation
            {
                DonorName = "John Doe",
                Description = "Canned food",
                ItemCount = 10,
                CategoryId = 1
            };

            // Act
            var result = await controller.CreateGoods(goods);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(1, context.GoodsDonations.Count());
        }

        [Fact]
        public async Task CreateGoods_WithInvalidData_ReturnsViewAndSavesNothing()
        {
            // Arrange
            using var context = CreateContext();
            var controller = CreateController(context);

            var goods = new GoodsDonation
            {
                DonorName = "John Doe",
                Description = "Canned food",
                ItemCount = 10,
                CategoryId = 1
            };

            controller.ModelState.AddModelError("Goods.ItemCount", "Required");

            // Act
            var result = await controller.CreateGoods(goods);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(0, context.GoodsDonations.Count());
        }

        [Fact]
        public async Task CreateMoney_FunctionReturnsCertificate_ShowsFunctionNumber()
        {
            // Arrange
            using var context = CreateContext();

            var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"certificateNumber\":\"GOTG-TEST-1\"}",
                    System.Text.Encoding.UTF8,
                    "application/json")
            });

            var factory = new Mock<IHttpClientFactory>();
            factory
                .Setup(f => f.CreateClient("TaxCertificate"))
                .Returns(new HttpClient(handler) { BaseAddress = new Uri("http://test/") });

            var controller = CreateController(context, factory.Object);

            var money = new MonetaryDonation
            {
                AmountZar = 250,
                IsAnonymous = true,
                DonationDate = DateTime.Today
            };

            // Act
            var result = await controller.CreateMoney(money);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(1, context.MonetaryDonations.Count());
            Assert.Contains("GOTG-TEST-1", (string)controller.TempData["Status"]!);
            factory.Verify(f => f.CreateClient("TaxCertificate"), Times.Once);
        }

        [Fact]
        public async Task CreateMoney_FunctionDown_StillSavesWithLocalCertificate()
        {
            // Arrange
            using var context = CreateContext();

            var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var factory = new Mock<IHttpClientFactory>();
            factory
                .Setup(f => f.CreateClient("TaxCertificate"))
                .Returns(new HttpClient(handler) { BaseAddress = new Uri("http://test/") });

            var controller = CreateController(context, factory.Object);

            var money = new MonetaryDonation
            {
                AmountZar = 250,
                IsAnonymous = true,
                DonationDate = DateTime.Today
            };

            // Act
            var result = await controller.CreateMoney(money);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(1, context.MonetaryDonations.Count());
            var saved = context.MonetaryDonations.First();
            var expectedCertificate = DonationHelper.FormatCertificateNumber(saved.Id, saved.DonationDate);
            Assert.Contains(expectedCertificate, (string)controller.TempData["Status"]!);
            factory.Verify(f => f.CreateClient("TaxCertificate"), Times.Once);
        }
    }
}
