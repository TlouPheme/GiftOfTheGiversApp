using GiftOfTheGiversApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGiversApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddControllersWithViews();

        // The tax certificate function's address and key come from configuration,
        // so Azure can point at the deployed function app without a code change.
        builder.Services.AddHttpClient("TaxCertificate", client =>
        {
            var function = builder.Configuration.GetSection("TaxCertificateFunction");
            client.BaseAddress = new Uri(function["BaseUrl"] ?? "http://localhost:7264/");
            client.Timeout = TimeSpan.FromSeconds(10);

            var key = function["FunctionKey"];
            if (!string.IsNullOrWhiteSpace(key))
            {
                client.DefaultRequestHeaders.Add("x-functions-key", key);
            }
        });

        var app = builder.Build();

        await DbInitializer.SeedAsync(app.Services);

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
