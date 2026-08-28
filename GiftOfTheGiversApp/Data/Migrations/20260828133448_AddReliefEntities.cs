using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftOfTheGiversApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReliefEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodsCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    IsPredefined = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    RequiredAid = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonetaryDonations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DonationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AmountZar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "INTEGER", nullable: false),
                    DonorName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonetaryDonations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 120, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Skills = table.Column<string>(type: "TEXT", maxLength: 400, nullable: true),
                    Availability = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsDonations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DonationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ItemCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    IsAnonymous = table.Column<bool>(type: "INTEGER", nullable: false),
                    DonorName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsDonations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsDonations_GoodsCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "GoodsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsDonations_CategoryId",
                table: "GoodsDonations",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodsDonations");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "MonetaryDonations");

            migrationBuilder.DropTable(
                name: "VolunteerApplications");

            migrationBuilder.DropTable(
                name: "GoodsCategories");
        }
    }
}
