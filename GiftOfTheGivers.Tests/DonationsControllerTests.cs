using GiftOfTheGiversApp.Controllers;
using GiftOfTheGiversApp.Data;
using GiftOfTheGiversApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
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

        private static DonationsController CreateController(ApplicationDbContext context)
        {
            var controller = new DonationsController(context, Mock.Of<IHttpClientFactory>());
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
    }
}
