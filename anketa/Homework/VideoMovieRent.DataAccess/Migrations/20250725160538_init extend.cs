using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VideoMovieRent.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initextend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Casts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Part = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genre = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Length = table.Column<TimeSpan>(type: "time", nullable: false),
                    AgeRestriction = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RentedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSubscriptionExpired = table.Column<bool>(type: "bit", nullable: false),
                    SubscriptionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "12233445", "peroperov" },
                    { 2, "admin123", "admin" }
                });

            migrationBuilder.InsertData(
                table: "Casts",
                columns: new[] { "Id", "MovieId", "Name", "Part" },
                values: new object[,]
                {
                    { 1, 1, "Ken Watanabe", 1 },
                    { 2, 2, "Park Chan-wook", 2 },
                    { 3, 2, "Lee Byung-hun", 1 },
                    { 4, 3, "Michael B. Jordan", 3 },
                    { 5, 4, "Marion Cotillard", 1 },
                    { 6, 7, "Tom Hardy", 1 },
                    { 7, 8, "Jessica Chastain", 2 },
                    { 8, 9, "Donnie Yen", 1 },
                    { 9, 3, "Zoe Saldana", 3 },
                    { 10, 6, "Owen Wilson", 1 }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "AgeRestriction", "Genre", "ImagePath", "IsAvailable", "Language", "Length", "Quantity", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 1, 13, 5, "/images/movie/NeonShados.jpg", true, 5, new TimeSpan(0, 2, 23, 0, 0), 5, new DateTime(2012, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Neon Shadows" },
                    { 2, 18, 3, "/images/movie/TheSilentForest.jpg", true, 6, new TimeSpan(0, 2, 12, 0, 0), 3, new DateTime(2019, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Silent Forest" },
                    { 3, 13, 5, "/images/movie/Paradox.jpg", false, 1, new TimeSpan(0, 2, 23, 0, 0), 0, new DateTime(2012, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quantum Paradox" },
                    { 4, 10, 2, "/images/movie/voyage.jpg", true, 3, new TimeSpan(0, 2, 49, 0, 0), 4, new DateTime(2014, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Celestial Voyage" },
                    { 5, 18, 4, "/images/movie/whispers.jpg", true, 2, new TimeSpan(0, 2, 26, 0, 0), 2, new DateTime(1980, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Whispers in the Dark" },
                    { 6, 12, 6, "/images/movie/paris.jpg", true, 3, new TimeSpan(0, 2, 2, 0, 0), 3, new DateTime(2001, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Midnight in Paris" },
                    { 7, 13, 8, "/images/movie/letters.jpg", false, 4, new TimeSpan(0, 1, 56, 0, 0), 0, new DateTime(1997, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Forgotten Letter" },
                    { 8, 17, 4, "/images/movie/peak.jpg", true, 1, new TimeSpan(0, 1, 44, 0, 0), 6, new DateTime(2017, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Crimson Peak" },
                    { 9, 13, 1, "/images/movie/agent.jpg", true, 7, new TimeSpan(0, 2, 28, 0, 0), 7, new DateTime(2010, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rogue Agent" },
                    { 10, 15, 7, "/images/movie/daugther.jpg", true, 2, new TimeSpan(0, 1, 58, 0, 0), 2, new DateTime(2006, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Alchemist's Daughter" }
                });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "MovieId", "RentedOn", "ReturnedOn", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 7, 15, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8187), new DateTime(2025, 7, 20, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8196), "Neon Shadows", 1 },
                    { 2, 2, new DateTime(2025, 7, 17, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8200), new DateTime(2025, 7, 23, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8201), "The Silent Forest", 2 },
                    { 3, 3, new DateTime(2025, 7, 18, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8203), new DateTime(2025, 7, 24, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8205), "Quantum Paradox", 3 },
                    { 4, 4, new DateTime(2025, 7, 20, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8207), new DateTime(2025, 7, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8208), "Celestial Voyage", 4 },
                    { 5, 5, new DateTime(2025, 7, 13, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8210), new DateTime(2025, 7, 18, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8211), "Whispers in the Dark", 5 },
                    { 6, 6, new DateTime(2025, 7, 22, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8213), new DateTime(2025, 7, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8214), "Midnight in Paris", 6 },
                    { 7, 7, new DateTime(2025, 7, 21, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8216), new DateTime(2025, 7, 24, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8217), "The Forgotten Letter", 7 },
                    { 8, 8, new DateTime(2025, 7, 23, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8219), new DateTime(2025, 7, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8221), "Crimson Peak", 8 },
                    { 9, 9, new DateTime(2025, 7, 19, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8222), new DateTime(2025, 7, 23, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8224), "Rogue Agent", 9 },
                    { 10, 10, new DateTime(2025, 7, 24, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8226), new DateTime(2025, 7, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8227), "The Alchemist's Daughter", 10 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "CardNumber", "CreatedOn", "FullName", "IsSubscriptionExpired", "SubscriptionType" },
                values: new object[,]
                {
                    { 1, 25, "1111-2222", new DateTime(2025, 1, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(7975), "Emma Watson", false, 1 },
                    { 2, 30, "2222-3333", new DateTime(2024, 11, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8025), "James Wilson", true, 2 },
                    { 3, 22, "3333-4444", new DateTime(2024, 7, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8029), "Olivia Martinez", false, 1 },
                    { 4, 28, "4444-5555", new DateTime(2025, 4, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8032), "Liam Anderson", false, 2 },
                    { 5, 35, "5555-6666", new DateTime(2023, 7, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8034), "Sophia Garcia", true, 1 },
                    { 6, 40, "6666-7777", new DateTime(2024, 9, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8037), "Noah Taylor", false, 2 },
                    { 7, 29, "7777-8888", new DateTime(2025, 2, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8039), "Ava Lopez", false, 1 },
                    { 8, 33, "8888-9999", new DateTime(2025, 6, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8041), "William Hernandez", true, 2 },
                    { 9, 21, "9999-0000", new DateTime(2024, 12, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8045), "Mia Gonzalez", false, 1 },
                    { 10, 27, "0000-1111", new DateTime(2025, 3, 25, 18, 5, 37, 849, DateTimeKind.Local).AddTicks(8047), "Benjamin Clark", true, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Casts");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
