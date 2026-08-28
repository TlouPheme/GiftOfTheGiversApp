using GiftOfTheGiversApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGiversApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<GoodsCategory> GoodsCategories => Set<GoodsCategory>();
    public DbSet<MonetaryDonation> MonetaryDonations => Set<MonetaryDonation>();
    public DbSet<GoodsDonation> GoodsDonations => Set<GoodsDonation>();
    public DbSet<VolunteerApplication> VolunteerApplications => Set<VolunteerApplication>();
}
