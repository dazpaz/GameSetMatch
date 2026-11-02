using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminConsole.DbMigration.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Surface = table.Column<int>(type: "int", nullable: false),
                    MensSingles = table.Column<bool>(type: "bit", nullable: false),
                    WomensSingles = table.Column<bool>(type: "bit", nullable: false),
                    MensDoubles = table.Column<bool>(type: "bit", nullable: false),
                    WomensDoubles = table.Column<bool>(type: "bit", nullable: false),
                    MixedDoubles = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tournaments",
                columns: new[] { "Id", "Title", "Location", "Level", "Surface", "MensSingles", "WomensSingles", "MensDoubles", "WomensDoubles", "MixedDoubles", "IsActive" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "Australian Open", "Melbourne, Australia", 0, 2, true, true, true, true, true, true },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "Monte-Carlo Masters", "Monte Carlo, Monaco", 1, 1, true, false, true, false, true, true },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Wimbledon", "London, United Kingdom", 0, 0, true, true, true, true, true, true },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "Indian Wells Masters", "Indian Wells, United States", 1, 2, true, true, true, true, false, true },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "Barcelona Open", "Barcelona, Spain", 2, 1, true, false, true, false, false, true },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "Stuttgart Open", "Stuttgart, Germany", 3, 0, false, true, false, true, false, false },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "Lyon Challenger", "Lyon, France", 4, 3, true, false, false, true, false, true },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "US Open", "New York, United States", 0, 2, true, true, false, false, true, true },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "French Open", "Paris, France", 0, 1, true, true, true, true, true, true },
                    { new Guid("00000000-0000-0000-0000-00000000000a"), "Miami Open", "Miami, United States", 1, 2, true, true, true, true, false, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tournaments");
        }
    }
}
