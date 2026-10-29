using GiftOfTheGiversApp.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.RegularExpressions;

namespace GiftOfTheGivers.Tests
{
    public class DonationFlowIntegrationTests
    {
        [Fact]
        public async Task MoneyDonation_ThroughForm_SavesAndShowsCertificate()
        {
            // Arrange
            using var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            // Step 1: open the donate page and read the anti-forgery token from it
            var page = await client.GetStringAsync("/Donations/Create?tab=money");

            var token = Regex.Match(page, @"<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" />").Groups[1].Value;

            // Step 2 (Act): post the form fields to /Donations/CreateMoney
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "__RequestVerificationToken", token },
                { "Money.DonationDate", "2023-01-01" },
                { "Money.AmountZar", "250" },
                { "Money.IsAnonymous", "true" }
            });

            var response = await client.PostAsync("/Donations/CreateMoney", form);

            // Step 3 (Assert): redirected, saved in the database, certificate shown
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(1, db.MonetaryDonations.Count(d => d.AmountZar == 250));

            var receipt = await client.GetStringAsync(response.Headers.Location!.ToString());
            Assert.Contains("GOTG-TEST-1", receipt);
        }

        [Fact]
        public async Task MoneyDonation_WithZeroAmount_ShowsErrorAndSavesNothing()
        {
            // Arrange
            using var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            // Step 1: open the donate page and read the anti-forgery token from it
            var page = await client.GetStringAsync("/Donations/Create?tab=money");
            var token = Regex.Match(page, @"<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" />").Groups[1].Value;
            // Step 2 (Act): post the form fields to /Donations/CreateMoney
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "__RequestVerificationToken", token },
                { "Money.DonationDate", "2023-01-01" },
                { "Money.AmountZar", "0" },
                { "Money.IsAnonymous", "false" },
                { "Money.DonorName", "John Doe" }
            });
            var response = await client.PostAsync("/Donations/CreateMoney", form);
            // Step 3 (Assert): should return to the form with a validation error
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("must be between", responseBody);

            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(0, db.MonetaryDonations.Count(d => d.AmountZar == 0));
        }
    }
}
