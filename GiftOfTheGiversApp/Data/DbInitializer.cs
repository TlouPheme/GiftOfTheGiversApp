using GiftOfTheGiversApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGiversApp.Data;

public static class DbInitializer
{
    public const string StaffEmail = "operations@giftofthegivers.org";
    public const string StaffPassword = "Hope1992!";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        await db.Database.MigrateAsync();

        if (!db.GoodsCategories.Any())
        {
            db.GoodsCategories.AddRange(
                new GoodsCategory { Name = "Food parcels", Description = "Non-perishable food packs for families in distress.", IsPredefined = true },
                new GoodsCategory { Name = "Bottled water", Description = "Drinking water for drought and flood-affected communities.", IsPredefined = true },
                new GoodsCategory { Name = "Clothing", Description = "Clothes, blankets and shoes for displaced people.", IsPredefined = true },
                new GoodsCategory { Name = "Medical supplies", Description = "First-aid kits and hygiene packs.", IsPredefined = false }
            );
            await db.SaveChangesAsync();
        }

        if (!db.Incidents.Any())
        {
            db.Incidents.AddRange(
                new Incident
                {
                    Title = "Mthatha flood relief",
                    Location = "King Sabata Dalindyebo, Eastern Cape",
                    StartDate = new DateTime(2026, 6, 10),
                    Description = "Flash flooding destroyed homes and contaminated municipal water. Field teams are distributing drinking water, food parcels and emergency shelter.",
                    RequiredAid = "Water, Food parcels, Shelter",
                    IsActive = true
                },
                new Incident
                {
                    Title = "Nelson Mandela Bay drought response",
                    Location = "Gqeberha, Eastern Cape",
                    StartDate = new DateTime(2026, 1, 18),
                    Description = "Prolonged drought has left several wards with unreliable tap water. Gift of the Givers is running water trucks and supporting borehole repairs.",
                    RequiredAid = "Water, Food parcels",
                    IsActive = true
                },
                new Incident
                {
                    Title = "eThekwini storm recovery",
                    Location = "Durban, KwaZulu-Natal",
                    StartDate = new DateTime(2026, 3, 2),
                    Description = "Severe storms flooded informal settlements. Teams are replacing ruined clothing, supporting clinics and helping families return to safe shelter.",
                    RequiredAid = "Clothing, Medical supplies, Shelter",
                    IsActive = true
                },
                new Incident
                {
                    Title = "Sudan humanitarian corridor",
                    Location = "Port Sudan and surrounding camps",
                    StartDate = new DateTime(2025, 11, 4),
                    Description = "Ongoing conflict has displaced families into overcrowded camps. The appeal covers food, primary healthcare and clean water.",
                    RequiredAid = "Food parcels, Medical supplies, Water",
                    IsActive = true
                },
                new Incident
                {
                    Title = "Knysna wildfire containment",
                    Location = "Garden Route, Western Cape",
                    StartDate = new DateTime(2025, 12, 14),
                    EndDate = new DateTime(2026, 1, 9),
                    Description = "Emergency feeding and clothing support during the Garden Route fires. This incident is closed and kept on record for reporting.",
                    RequiredAid = "Food parcels, Clothing",
                    IsActive = false
                }
            );
            await db.SaveChangesAsync();
        }

        if (!db.MonetaryDonations.Any())
        {
            db.MonetaryDonations.AddRange(
                new MonetaryDonation { DonationDate = new DateTime(2026, 8, 12), AmountZar = 25000, DonorName = "Pietermaritzburg Rotary Club", Message = "For Mthatha water tankers." },
                new MonetaryDonation { DonationDate = new DateTime(2026, 8, 18), AmountZar = 5000, IsAnonymous = true },
                new MonetaryDonation { DonationDate = new DateTime(2026, 8, 21), AmountZar = 12000, DonorName = "Ayesha Naidoo", Message = "Monthly gift." }
            );
            await db.SaveChangesAsync();
        }

        if (!db.GoodsDonations.Any())
        {
            var food = await db.GoodsCategories.FirstAsync(c => c.Name == "Food parcels");
            var water = await db.GoodsCategories.FirstAsync(c => c.Name == "Bottled water");
            var clothes = await db.GoodsCategories.FirstAsync(c => c.Name == "Clothing");
            db.GoodsDonations.AddRange(
                new GoodsDonation { DonationDate = new DateTime(2026, 8, 14), ItemCount = 80, CategoryId = food.Id, Description = "Family food parcels with rice, beans, oil and tinned fish.", DonorName = "Spar Midlands" },
                new GoodsDonation { DonationDate = new DateTime(2026, 8, 19), ItemCount = 240, CategoryId = water.Id, Description = "5-litre bottled water for flood-affected households.", IsAnonymous = true },
                new GoodsDonation { DonationDate = new DateTime(2026, 8, 22), ItemCount = 45, CategoryId = clothes.Id, Description = "Blankets and children's clothing.", DonorName = "Westville Mosque collection" }
            );
            await db.SaveChangesAsync();
        }

        if (!db.VolunteerApplications.Any())
        {
            db.VolunteerApplications.AddRange(
                new VolunteerApplication
                {
                    FullName = "Thandiwe Mokoena",
                    Phone = "082 441 2290",
                    Email = "thandiwe.m@example.com",
                    City = "Johannesburg",
                    Skills = "Nursing, first aid, isiZulu and English",
                    Availability = "Weekends and public holidays",
                    SubmittedAt = new DateTime(2026, 8, 8, 9, 14, 0, DateTimeKind.Utc)
                },
                new VolunteerApplication
                {
                    FullName = "Johan Pretorius",
                    Phone = "071 883 4412",
                    Email = "jpretorius@example.com",
                    City = "Gqeberha",
                    Skills = "Heavy-duty driving, warehouse packing",
                    Availability = "After 17:00 on weekdays",
                    SubmittedAt = new DateTime(2026, 8, 16, 15, 40, 0, DateTimeKind.Utc)
                }
            );
            await db.SaveChangesAsync();
        }

        if (await users.FindByEmailAsync(StaffEmail) is null)
        {
            var staff = new IdentityUser
            {
                UserName = StaffEmail,
                Email = StaffEmail,
                EmailConfirmed = true
            };
            var result = await users.CreateAsync(staff, StaffPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Could not seed staff user: " +
                    string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
