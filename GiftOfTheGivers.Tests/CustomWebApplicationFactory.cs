using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using GiftOfTheGiversApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using GiftOfTheGiversApp;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace GiftOfTheGivers.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly SqliteConnection _connection = new SqliteConnection("Data Source=:memory:");

        public CustomWebApplicationFactory()
        {
            _connection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // a) Remove the real database
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // b) Use an in-memory SQLite database instead
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_connection));

                // c) Replace the tax certificate function with the fake
                services.AddHttpClient("TaxCertificate")
                    .ConfigurePrimaryHttpMessageHandler(() => new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(
                            "{\"certificateNumber\":\"GOTG-TEST-1\"}",
                            System.Text.Encoding.UTF8,
                            "application/json")
                    }));
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connection.Dispose();
        }
    }
}
