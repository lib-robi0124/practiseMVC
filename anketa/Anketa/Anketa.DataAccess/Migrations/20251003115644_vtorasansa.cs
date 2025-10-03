using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Anketa.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class vtorasansa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QuestionTypeId = table.Column<int>(type: "int", nullable: false),
                    QuestionFormId = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionForms_QuestionFormId",
                        column: x => x.QuestionFormId,
                        principalTable: "QuestionForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionFormId = table.Column<int>(type: "int", nullable: false),
                    ScaleValue = table.Column<int>(type: "int", nullable: true),
                    TextValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnsweredDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_QuestionForms_QuestionFormId",
                        column: x => x.QuestionFormId,
                        principalTable: "QuestionForms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "QuestionForms",
                columns: new[] { "Id", "CreatedDate", "Description", "IsActive", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8984), "Overall Satisfaction", true, "Општо задоволство" },
                    { 2, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8987), "Commitment to the Company", true, "Обврска кон компанијата" },
                    { 3, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8988), "Professional Development", true, "Професионален развој" },
                    { 4, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8989), "Work-Life Balance", true, "Рамнотежа помеѓу работата и животот" },
                    { 5, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8990), "Communication and Collaboration", true, "Комуникација и соработка" },
                    { 6, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8990), "Leadership", true, "Лидерство" },
                    { 7, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8991), "Organizational Culture", true, "Организациска култура" },
                    { 8, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8992), "Work Environment", true, "Работна средина" },
                    { 9, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8992), "Rewards and Recognition", true, "Награди и признанија" },
                    { 10, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8994), "Innovation and Changes", true, "Иновации и промени" },
                    { 11, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8994), "Final Evaluation", true, "Конечна евалуација" },
                    { 12, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(8995), "Open Questions", true, "Отворени прашања" }
                });

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "1-10 Scale", "Scale" },
                    { 2, "Text Answer", "Text" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Administrator" },
                    { 2, "Employee" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "CreatedDate", "FullName", "OU", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, 16130, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9126), "Vasko Gjorgiev", "Production", "16130", 2 },
                    { 2, 16684, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9130), "Zoran Stojanovski", "Production", "16684", 2 },
                    { 3, 16695, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9133), "Pane Naskovski", "Production", "16695", 2 },
                    { 4, 16720, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9135), "Tome Trajanov", "Projects and IT", "16720", 2 },
                    { 5, 16827, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9136), "Zoran Boshkovski", "Production", "16827", 2 },
                    { 6, 16984, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9137), "Dide Petrovski", "Projects and IT", "16984", 2 },
                    { 7, 17011, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9138), "Jovica Gjorgjievski", "Projects and IT", "17011", 2 },
                    { 8, 17055, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9139), "Blagica Besarovska", "Projects and IT", "17055", 2 },
                    { 9, 17064, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9140), "Dragi Naskovski", "Production", "17064", 2 },
                    { 10, 17148, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9140), "Borche Anchevski", "Production", "17148", 2 },
                    { 11, 17772, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9142), "Toni Nacev", "HR", "17772", 2 },
                    { 12, 17884, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9142), "Valentina Kostovska", "HR", "17884", 2 },
                    { 13, 17893, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9143), "Zoran Tripunoski", "Production", "17893", 2 },
                    { 14, 17896, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9144), "Zorancho Taseski", "Projects and IT", "17896", 2 },
                    { 15, 18158, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9145), "Goran Despodovski", "Production", "18158", 2 },
                    { 16, 18162, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9146), "Ljupcho Krstevski", "Production", "18162", 2 },
                    { 17, 18392, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9147), "Sabedin Ljura", "Production", "18392", 2 },
                    { 18, 18412, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9148), "Rade Milenkovski", "Production", "18412", 2 },
                    { 19, 18471, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9149), "Stojka Koneska", "Supply chain", "18471", 2 },
                    { 20, 18529, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9150), "Zharko Nikolovski", "Production", "18529", 2 },
                    { 21, 18533, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9151), "Radica Angelovska", "Projects and IT", "18533", 2 },
                    { 22, 18874, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9152), "Borche Trifunovski", "Production", "18874", 2 },
                    { 23, 18876, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9153), "Pero Stojanovski", "Production", "18876", 2 },
                    { 24, 19370, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9154), "Dragi Petrovski", "Production", "19370", 2 },
                    { 25, 19379, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9155), "Ilo Risteski", "Supply chain", "19379", 2 },
                    { 26, 19767, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9156), "Aleksandar Iliev", "Production", "19767", 2 },
                    { 27, 19775, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9157), "Mile Popovski", "Production", "19775", 2 },
                    { 28, 19776, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9158), "Dragan Hristovski", "Production", "19776", 2 },
                    { 29, 19777, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9159), "Aleksandar Jovchevski", "Production", "19777", 2 },
                    { 30, 19779, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9160), "Ljupcho Andovski", "Supply chain", "19779", 2 },
                    { 31, 19782, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9161), "Ivica Stanchevski", "Production", "19782", 2 },
                    { 32, 19784, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9162), "Biljana Ilievska", "Supply chain", "19784", 2 },
                    { 33, 19787, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9163), "Goran Damjanoski", "Supply chain", "19787", 2 },
                    { 34, 19788, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9164), "Boban Neshovski", "Production", "19788", 2 },
                    { 35, 19795, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9165), "Sashe Taparchevski", "Production", "19795", 2 },
                    { 36, 19796, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9191), "Igor Ristovski", "Production", "19796", 2 },
                    { 37, 19798, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9193), "Ivica Trajkovski", "Production", "19798", 2 },
                    { 38, 19801, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9194), "Vlado Stojanovski", "Production", "19801", 2 },
                    { 39, 19804, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9195), "Goran Spirovski", "Production", "19804", 2 },
                    { 40, 19806, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9195), "Dejan Velkovski", "Production", "19806", 2 },
                    { 41, 19807, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9196), "Stojanche Stefkovski", "Production", "19807", 2 },
                    { 42, 19811, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9197), "Dancho Blazheski", "Production", "19811", 2 },
                    { 43, 19813, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9198), "Ljupcho Lozanovski", "Production", "19813", 2 },
                    { 44, 19818, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9204), "Marjan Nedelkovski", "Projects and IT", "19818", 2 },
                    { 45, 19820, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9205), "Srgjan Stanojevikj", "Production", "19820", 2 },
                    { 46, 19822, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9206), "Dragan Spasevski", "Supply chain", "19822", 2 },
                    { 47, 19823, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9207), "Goran Andonovski", "Projects and IT", "19823", 2 },
                    { 48, 19827, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9208), "Goran Anchovski", "Supply chain", "19827", 2 },
                    { 49, 19833, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9209), "Igor Mircheski", "Supply chain", "19833", 2 },
                    { 50, 19834, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9209), "Goran Nikolovski", "HR", "19834", 2 },
                    { 51, 19838, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9210), "Petar Moskov", "Production", "19838", 2 },
                    { 52, 19840, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9211), "Goran Stojanovski", "Supply chain", "19840", 2 },
                    { 53, 19841, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9212), "Igor Petkovski", "Projects and IT", "19841", 2 },
                    { 54, 19842, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9213), "Nenad Mitrovikj", "Production", "19842", 2 },
                    { 55, 19844, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9214), "Sashko Gjorgjievski", "Supply chain", "19844", 2 },
                    { 56, 19845, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9215), "Nikola Toshevski", "Production", "19845", 2 },
                    { 57, 19848, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9216), "Slobodan Velkovski", "Production", "19848", 2 },
                    { 58, 19849, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9217), "Goce Jankulovski", "Supply chain", "19849", 2 },
                    { 59, 19868, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9218), "Marjan Milovanovikj", "Production", "19868", 2 },
                    { 60, 19877, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9219), "Goran Gavrilovski", "Sales", "19877", 2 },
                    { 61, 19892, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9220), "Irfan Feratovski", "Production", "19892", 2 },
                    { 62, 19899, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9221), "Igor Krpachovski", "Projects and IT", "19899", 2 },
                    { 63, 19911, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9223), "Aleksandar Spasevski", "CEO office", "19911", 2 },
                    { 64, 19916, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9225), "Nevaip Bardi", "Production", "19916", 2 },
                    { 65, 19917, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9226), "Biljana Stoshikj", "Supply chain", "19917", 2 },
                    { 66, 19933, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9227), "Svetlana Jovanova", "Finance Department", "19933", 2 },
                    { 67, 19960, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9227), "Draganche Taleski", "Production", "19960", 2 },
                    { 68, 19963, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9228), "Toni Naumovski", "Production", "19963", 2 },
                    { 69, 19992, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9229), "Metodi Gievski", "Projects and IT", "19992", 2 },
                    { 70, 19993, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9230), "Jovica Velkovski", "Supply chain", "19993", 2 },
                    { 71, 19997, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9231), "Gordana Astardjieva", "Projects and IT", "19997", 2 },
                    { 72, 20023, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9232), "Zharko Ivanovski", "Production", "20023", 2 },
                    { 73, 20024, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9233), "Igorche Janev", "Production", "20024", 2 },
                    { 74, 20033, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9234), "Nikola Panov", "Supply chain", "20033", 2 },
                    { 75, 20034, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9236), "Sasho Mitkovski", "Production", "20034", 2 },
                    { 76, 20038, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9237), "Goran Ilievski", "Production", "20038", 2 },
                    { 77, 20041, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9237), "Kircho Merdjanovski", "Projects and IT", "20041", 2 },
                    { 78, 20052, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9238), "Davor Zdravkovski", "Supply chain", "20052", 2 },
                    { 79, 20072, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9239), "Gorancho Petkovski", "Production", "20072", 2 },
                    { 80, 20076, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9240), "Sashko Cvetanovski", "Production", "20076", 2 },
                    { 81, 20095, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9241), "Ilija Tashevski", "Production", "20095", 2 },
                    { 82, 20117, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9243), "Kire Stefanoski", "Production", "20117", 2 },
                    { 83, 20125, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9244), "Aleksandar Evremov", "Supply chain", "20125", 2 },
                    { 84, 20127, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9245), "Ratko Trajkovski", "Supply chain", "20127", 2 },
                    { 85, 20128, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9246), "Goran Miovski", "Projects and IT", "20128", 2 },
                    { 86, 20131, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9247), "Goran Trajkovski", "Production", "20131", 2 },
                    { 87, 20137, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9247), "Gordana Shegmanovikj", "HR", "20137", 2 },
                    { 88, 20144, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9248), "Igorche Bogdanovski", "Production", "20144", 2 },
                    { 89, 20152, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9249), "Miodrag Petkovikj", "Production", "20152", 2 },
                    { 90, 20159, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9250), "Gorancho Najdovski", "Projects and IT", "20159", 2 },
                    { 91, 20160, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9252), "Dejan Jazadjiev", "Supply chain", "20160", 2 },
                    { 92, 20162, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9253), "Sashko Peshov", "Production", "20162", 2 },
                    { 93, 20163, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9254), "Kiro Radevski", "Production", "20163", 2 },
                    { 94, 20167, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9255), "Sasho Beroski", "Production", "20167", 2 },
                    { 95, 20168, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9256), "Vlatko Changovski", "Projects and IT", "20168", 2 },
                    { 96, 20178, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9257), "Stojan Stavreski", "Projects and IT", "20178", 2 },
                    { 97, 20182, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9257), "Kjemalj Abazi", "Production", "20182", 2 },
                    { 98, 20184, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9258), "Djevat Selimi", "Production", "20184", 2 },
                    { 99, 20191, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9259), "Trajche Dimovski", "Projects and IT", "20191", 2 },
                    { 100, 20195, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9260), "Robert Shijakovski", "Production", "20195", 2 },
                    { 101, 20203, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9262), "Slavisha Crnichin", "Production", "20203", 2 },
                    { 102, 20210, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9263), "Borche Cvetkovski", "Supply chain", "20210", 2 },
                    { 103, 20212, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9264), "Sasha Stefanoski", "Supply chain", "20212", 2 },
                    { 104, 20218, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9265), "Josif Slavkovski", "Production", "20218", 2 },
                    { 105, 20225, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9266), "Goce Stojchevski", "Projects and IT", "20225", 2 },
                    { 106, 20226, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9267), "Donche Nedelkovski", "Production", "20226", 2 },
                    { 107, 20231, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9267), "Ljupcho Shegmanovikj", "Production", "20231", 2 },
                    { 108, 20232, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9268), "Goran Markovski", "Production", "20232", 2 },
                    { 109, 20233, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9269), "Dragan Markovski", "Projects and IT", "20233", 2 },
                    { 110, 20234, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9270), "Ljupcho Veljanovski", "Production", "20234", 2 },
                    { 111, 20235, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9271), "Nikola Angeleski", "Production", "20235", 2 },
                    { 112, 20236, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9272), "Aleksandar Bogoev", "Production", "20236", 2 },
                    { 113, 20238, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9273), "Stevche Velkovski", "Projects and IT", "20238", 2 },
                    { 114, 20245, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9274), "Ljubomir Kochovski", "Projects and IT", "20245", 2 },
                    { 115, 20246, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9275), "Sashko Blazhevski", "Production", "20246", 2 },
                    { 116, 20248, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9276), "Zoranche Borizovski", "Production", "20248", 2 },
                    { 117, 20253, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9276), "Nebojsha Stojmanovikj", "Projects and IT", "20253", 2 },
                    { 118, 20255, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9277), "Ljupcho Pashoski", "Production", "20255", 2 },
                    { 119, 20261, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9278), "Aleksandar Karajanovski", "Production", "20261", 2 },
                    { 120, 20262, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9282), "Dejan Stojanov", "Supply chain", "20262", 2 },
                    { 121, 20263, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9282), "Vladimir Jakimov", "Projects and IT", "20263", 2 },
                    { 122, 20267, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9283), "Goranche Ginoski", "Production", "20267", 2 },
                    { 123, 20271, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9284), "Avdil Mustafa", "Production", "20271", 2 },
                    { 124, 20272, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9285), "Beta Damevska", "Projects and IT", "20272", 2 },
                    { 125, 20275, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9286), "Naser Ilazov", "Production", "20275", 2 },
                    { 126, 20280, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9287), "Viktor Boshkovski", "Production", "20280", 2 },
                    { 127, 20283, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9288), "Zlatko Petrovski", "Supply chain", "20283", 2 },
                    { 128, 20284, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9289), "Aleksandar Stoicev", "Production", "20284", 2 },
                    { 129, 20286, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9290), "Aleksandar Jovanovski", "Production", "20286", 2 },
                    { 130, 20296, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9291), "Goran Jovanovski", "Production", "20296", 2 },
                    { 131, 20297, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9291), "Ivan Maslov", "Projects and IT", "20297", 2 },
                    { 132, 20298, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9292), "Ivica Tripunovski", "Supply chain", "20298", 2 },
                    { 133, 20299, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9293), "Milisav Boshkovikj", "Production", "20299", 2 },
                    { 134, 20300, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9294), "Ilija Pandurski", "Production", "20300", 2 },
                    { 135, 20308, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9295), "Ljuben Trajkoski", "Supply chain", "20308", 2 },
                    { 136, 20316, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9296), "Igor Petrovski", "Projects and IT", "20316", 2 },
                    { 137, 20320, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9297), "Brankica Trajanoska", "Supply chain", "20320", 2 },
                    { 138, 20323, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9323), "Blazhe Dimov", "Projects and IT", "20323", 2 },
                    { 139, 20324, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9324), "Biljana Petrovska", "Supply chain", "20324", 2 },
                    { 140, 20325, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9325), "Zoran Trajkov", "Projects and IT", "20325", 2 },
                    { 141, 20328, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9326), "Slavko Spasovski", "HR", "20328", 2 },
                    { 142, 20332, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9327), "Igorche Gjorgjievski", "Projects and IT", "20332", 2 },
                    { 143, 20335, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9328), "Jovica Boshkovski", "Production", "20335", 2 },
                    { 144, 20341, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9328), "Metodija Blazhevski", "Supply chain", "20341", 2 },
                    { 145, 20350, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9329), "Sasho Petrushevski", "Supply chain", "20350", 2 },
                    { 146, 20351, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9330), "Marjan Stojanovski", "Supply chain", "20351", 2 },
                    { 147, 20357, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9331), "Dejan Petrushevski", "Sales", "20357", 2 },
                    { 148, 20362, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9332), "Sasho Kiprijanovski", "Supply chain", "20362", 2 },
                    { 149, 20363, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9333), "Zoran Mitevski", "Production", "20363", 2 },
                    { 150, 20372, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9334), "Sasho Gjorgjievski", "Production", "20372", 2 },
                    { 151, 20380, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9335), "Orce Angelovski", "Projects and IT", "20380", 2 },
                    { 152, 20381, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9336), "Dejan Danilovski", "Supply chain", "20381", 2 },
                    { 153, 20382, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9337), "Robert Angelovski", "Projects and IT", "20382", 2 },
                    { 154, 20385, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9338), "Bratislav Mihajlovikj", "Supply chain", "20385", 2 },
                    { 155, 20387, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9339), "Ace Mitevski", "Supply chain", "20387", 2 },
                    { 156, 20389, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9340), "Igorche Markovski", "Production", "20389", 2 },
                    { 157, 20390, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9341), "Jovan Markovski", "Supply chain", "20390", 2 },
                    { 158, 20393, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9343), "Boban Hristovski", "Supply chain", "20393", 2 },
                    { 159, 20395, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9344), "Dejan Dimishkovski", "Production", "20395", 2 },
                    { 160, 20397, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9345), "Marjan Simonovski", "Production", "20397", 2 },
                    { 161, 20402, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9345), "Slavica Mladenovska", "Supply chain", "20402", 2 },
                    { 162, 20431, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9346), "Angelina Rajovska", "HR", "20431", 2 },
                    { 163, 20439, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9347), "Dejan Dilevski", "Production", "20439", 2 },
                    { 164, 20443, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9348), "Aleksandar Zotikj", "Supply chain", "20443", 2 },
                    { 165, 20445, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9349), "Jovica Maznevski", "Production", "20445", 2 },
                    { 166, 20447, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9350), "Zvonko Neshkovikj", "Production", "20447", 2 },
                    { 167, 20448, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9351), "Ljupcho Paunkov", "Production", "20448", 2 },
                    { 168, 20449, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9352), "Marjan Zdravkovski", "Production", "20449", 2 },
                    { 169, 20451, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9353), "Igor Jordanovski", "Production", "20451", 2 },
                    { 170, 20453, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9354), "Aleksandar Stamchevski", "Production", "20453", 2 },
                    { 171, 20454, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9355), "Dejan Vasilevski", "Production", "20454", 2 },
                    { 172, 20459, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9356), "Zoran Stojanovski", "Production", "20459", 2 },
                    { 173, 20466, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9357), "Iljaz Prekopuca", "Production", "20466", 2 },
                    { 174, 20468, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9357), "Fadil Tanalari", "Production", "20468", 2 },
                    { 175, 20471, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9358), "Trajche Petrovski", "Projects and IT", "20471", 2 },
                    { 176, 20475, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9363), "Boban Mitrovikj", "Production", "20475", 2 },
                    { 177, 20478, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9364), "Dragan Saveski", "Production", "20478", 2 },
                    { 178, 20489, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9365), "Ajdin Zulfiovski", "Production", "20489", 2 },
                    { 179, 20511, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9366), "Ivan Cibrev", "Supply chain", "20511", 2 },
                    { 180, 20518, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9367), "Slavica Jovchevska", "Supply chain", "20518", 2 },
                    { 181, 20521, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9368), "Elena Damchevska", "Finance Department", "20521", 2 },
                    { 182, 20523, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9368), "Vesna Dimevska", "Supply chain", "20523", 2 },
                    { 183, 20527, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9369), "Kiril Simonoski", "Projects and IT", "20527", 2 },
                    { 184, 20530, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9370), "Goce Atanasoski", "Projects and IT", "20530", 2 },
                    { 185, 20603, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9371), "Goran Stojchevski", "Production", "20603", 2 },
                    { 186, 20621, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9372), "Todorka Ristovska", "CEO office", "20621", 2 },
                    { 187, 20623, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9373), "Elena Blazeva", "Finance Department", "20623", 2 },
                    { 188, 20625, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9374), "Darko Najdenov", "Supply chain", "20625", 2 },
                    { 189, 20632, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9375), "Zoran Mladenovski", "Projects and IT", "20632", 2 },
                    { 190, 20636, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9376), "Natalija Nikoloska", "Supply chain", "20636", 2 },
                    { 191, 20637, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9377), "Aleksandar Krstev", "Supply chain", "20637", 2 },
                    { 192, 20638, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9378), "Elena Kocevska Peceva", "Supply chain", "20638", 2 },
                    { 193, 20640, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9378), "Kiro Risteski", "Production", "20640", 2 },
                    { 194, 20650, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9379), "Dejana Jovanova Krsteva", "Supply chain", "20650", 2 },
                    { 195, 20652, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9380), "Toni Pandilovski", "Projects and IT", "20652", 2 },
                    { 196, 20662, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9381), "Vladimir Shulevski", "Production", "20662", 2 },
                    { 197, 20675, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9383), "Dejan Trajkovski", "HR", "20675", 2 },
                    { 198, 20678, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9384), "Kire Blagoeski", "Supply chain", "20678", 2 },
                    { 199, 20685, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9385), "Petar Brashnarov", "Production", "20685", 2 },
                    { 200, 20694, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9386), "Zvonimir Manchevski", "Production", "20694", 2 },
                    { 201, 20695, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9388), "Aleksandar Dejanovski", "Projects and IT", "20695", 2 },
                    { 202, 20707, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9389), "Selaedin Feratovski", "Projects and IT", "20707", 2 },
                    { 203, 20708, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9390), "Slave Manevski", "Projects and IT", "20708", 2 },
                    { 204, 20723, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9390), "Djevat Saliovski", "Production", "20723", 2 },
                    { 205, 20724, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9391), "Vesna Velichkovska", "HR", "20724", 1 },
                    { 206, 20729, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9392), "Vlatko Dimishkovski", "Projects and IT", "20729", 2 },
                    { 207, 20734, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9393), "Blage Uroshevski", "Production", "20734", 2 },
                    { 208, 20735, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9394), "Stojadin Jankovski", "Production", "20735", 2 },
                    { 209, 20737, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9395), "Zlatko Nikoloski", "Production", "20737", 2 },
                    { 210, 20747, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9396), "Goce Gjorgjievski", "Production", "20747", 2 },
                    { 211, 20751, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9397), "Stefan Tonevski", "Supply chain", "20751", 2 },
                    { 212, 20753, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9398), "Orce Dimovski", "Production", "20753", 2 },
                    { 213, 20758, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9398), "Elena Valkancheva Najdenova", "Sales", "20758", 2 },
                    { 214, 20776, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9399), "Igor Dushanovski", "Projects and IT", "20776", 2 },
                    { 215, 20779, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9401), "Jagoda Velevska", "CEO office", "20779", 2 },
                    { 216, 20781, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9402), "Vladimir Nikolikj", "Supply chain", "20781", 2 },
                    { 217, 20784, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9403), "Dejan Gocevski", "Production", "20784", 2 },
                    { 218, 20787, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9404), "Aleksandar Kostovski", "Production", "20787", 2 },
                    { 219, 20797, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9405), "Cane Nikoloski", "Production", "20797", 2 },
                    { 220, 20800, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9406), "Viktor Stamenkovski", "Projects and IT", "20800", 2 },
                    { 221, 20801, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9406), "Dragana Petrovikj", "Supply chain", "20801", 2 },
                    { 222, 20802, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9407), "Stefan Despodovski", "Supply chain", "20802", 2 },
                    { 223, 20803, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9408), "Marjan Milanovski", "Projects and IT", "20803", 2 },
                    { 224, 20804, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9409), "Dragan Koneski", "Projects and IT", "20804", 2 },
                    { 225, 20814, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9410), "Aleksandar Stojanovski", "Production", "20814", 2 },
                    { 226, 20815, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9411), "Sashko Miloshevski", "Production", "20815", 2 },
                    { 227, 20822, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9412), "Elza Petrovska", "Sales", "20822", 2 },
                    { 228, 20824, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9413), "Darko Zdravkovski", "Production", "20824", 2 },
                    { 229, 20825, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9413), "Kiril Chirkov", "Production", "20825", 2 },
                    { 230, 20827, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9414), "Igor Cvetanoski", "Production", "20827", 2 },
                    { 231, 20831, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9415), "Martin Nikolovski", "Production", "20831", 2 },
                    { 232, 20832, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9416), "Dushko Blazevski", "Supply chain", "20832", 2 },
                    { 233, 20834, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9417), "Muammet Sali", "Projects and IT", "20834", 2 },
                    { 234, 20835, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9418), "Kristijan Janev", "Projects and IT", "20835", 2 },
                    { 235, 20837, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9420), "Dilaver Sali", "Production", "20837", 2 },
                    { 236, 20838, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9421), "Sasho Neshkov", "Production", "20838", 2 },
                    { 237, 20839, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9422), "Goran Ilikj", "Production", "20839", 2 },
                    { 238, 20842, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9422), "Zoran Trendevski", "Production", "20842", 2 },
                    { 239, 20844, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9423), "Igor Lazevski", "Production", "20844", 2 },
                    { 240, 20847, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9424), "Dragan Dragutinovski", "Projects and IT", "20847", 2 },
                    { 241, 20848, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9445), "Hristo Jovanovski", "Production", "20848", 2 },
                    { 242, 20851, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9446), "Goran Moskov", "Production", "20851", 2 },
                    { 243, 20852, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9447), "Zoran Nikolovski", "Projects and IT", "20852", 2 },
                    { 244, 20855, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9448), "Goran Cvetanovski", "Production", "20855", 2 },
                    { 245, 20866, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9449), "Pero Mangarov", "Supply chain", "20866", 2 },
                    { 246, 20871, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9450), "Igor Blazevski", "Production", "20871", 2 },
                    { 247, 20872, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9450), "Spase Belinski", "Projects and IT", "20872", 2 },
                    { 248, 20876, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9451), "Zhivorad Arsenovski", "Production", "20876", 2 },
                    { 249, 20879, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9452), "Ljupcho Pijakovski", "Production", "20879", 2 },
                    { 250, 20883, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9453), "Ljupcho Dimitrijeski", "Projects and IT", "20883", 2 },
                    { 251, 20889, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9454), "Dushan Jovanoski", "Sales", "20889", 2 },
                    { 252, 20893, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9455), "Nikola Nikolovski", "Sales", "20893", 2 },
                    { 253, 20894, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9456), "Dimitar Jankovski", "Production", "20894", 2 },
                    { 254, 20896, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9458), "Imer Ljusjani", "Projects and IT", "20896", 2 },
                    { 255, 20898, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9459), "Bobi Gjogjievski", "Projects and IT", "20898", 2 },
                    { 256, 20899, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9460), "Hristijan Gjorgjevski", "Production", "20899", 2 },
                    { 257, 20903, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9461), "Temelko Sarovski", "Production", "20903", 2 },
                    { 258, 20910, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9462), "Hristijan Simonovski", "Supply chain", "20910", 2 },
                    { 259, 20911, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9463), "Dame Kekenovski", "Production", "20911", 2 },
                    { 260, 20914, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9464), "Afrim Jusufi", "Production", "20914", 2 },
                    { 261, 20915, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9465), "Igor Damjanovski", "Supply chain", "20915", 2 },
                    { 262, 20916, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9465), "Besnik Ibraimi", "Projects and IT", "20916", 2 },
                    { 263, 20917, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9466), "Viktor Velichkovski", "Production", "20917", 2 },
                    { 264, 20919, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9467), "Robert Jovanovski", "Projects and IT", "20919", 2 },
                    { 265, 20920, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9468), "Adnan Feratovski", "Projects and IT", "20920", 2 },
                    { 266, 20924, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9469), "Biljana Chorobenska", "Supply chain", "20924", 2 },
                    { 267, 20927, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9470), "Vladan Trajkovski", "Projects and IT", "20927", 2 },
                    { 268, 20928, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9471), "Vlatko Mitevski", "Production", "20928", 2 },
                    { 269, 20935, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9472), "Adis Nezirovski", "Projects and IT", "20935", 2 },
                    { 270, 20936, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9473), "Asim Nezirovski", "Projects and IT", "20936", 2 },
                    { 271, 20937, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9474), "Goce Spaseski", "Production", "20937", 2 },
                    { 272, 20942, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9475), "Dragi Ickovski", "Projects and IT", "20942", 2 },
                    { 273, 20944, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9476), "Ibrahim Mujovikj", "Production", "20944", 2 },
                    { 274, 20948, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9477), "Boban Grozdanovski", "Projects and IT", "20948", 2 },
                    { 275, 20951, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9478), "Robert Stojanovikj", "Projects and IT", "20951", 2 },
                    { 276, 20953, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9479), "Mihajlo Zafirovikj", "Production", "20953", 2 },
                    { 277, 20955, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9480), "Aleksandra Trgachevska", "Supply chain", "20955", 2 },
                    { 278, 20958, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9481), "Marjanche Ristovski", "Production", "20958", 2 },
                    { 279, 20963, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9482), "Dalibor Cvetkovski", "Production", "20963", 2 },
                    { 280, 20964, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9483), "Ivica Stanoeski", "Projects and IT", "20964", 2 },
                    { 281, 20967, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9484), "Gjorgji Velichkovski", "Supply chain", "20967", 2 },
                    { 282, 20968, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9485), "Karanfilka Giceva", "Supply chain", "20968", 2 },
                    { 283, 20971, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9486), "Djevat Feratovski", "Production", "20971", 2 },
                    { 284, 20973, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9487), "Ivan Mitodevski", "Production", "20973", 2 },
                    { 285, 20975, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9488), "Robert Ristovski", "Projects and IT", "20975", 1 },
                    { 286, 20977, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9488), "Vlatko Dimevski", "Supply chain", "20977", 2 },
                    { 287, 20979, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9489), "Violeta Vidinska", "HR", "20979", 2 },
                    { 288, 20981, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9490), "Aco Jovanovski", "Projects and IT", "20981", 2 },
                    { 289, 20982, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9491), "Rade Panovski", "Production", "20982", 2 },
                    { 290, 20983, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9492), "Slave Joshovikj", "Production", "20983", 2 },
                    { 291, 20988, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9493), "Nenad Petkovikj", "Projects and IT", "20988", 2 },
                    { 292, 20989, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9495), "Borche Livrinski", "Projects and IT", "20989", 2 },
                    { 293, 20994, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9496), "Sanja Lambrinidis", "Supply chain", "20994", 2 },
                    { 294, 20998, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9497), "Ace Jovanovski", "Production", "20998", 2 },
                    { 295, 20999, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9498), "Sashe Smilkovski", "Projects and IT", "20999", 2 },
                    { 296, 21002, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9499), "Leon Danilovski", "Supply chain", "21002", 2 },
                    { 297, 21003, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9499), "Enis Zekjiri", "Projects and IT", "21003", 2 },
                    { 298, 21006, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9500), "Metodija Malkov", "Production", "21006", 2 },
                    { 299, 21010, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9501), "Stefan Risteski", "Projects and IT", "21010", 2 },
                    { 300, 21012, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9502), "Igor Momchilovski", "Production", "21012", 2 },
                    { 301, 21016, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9503), "Oliver Govedarovski", "Projects and IT", "21016", 2 },
                    { 302, 21017, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9504), "Bobi Nikolovski", "Projects and IT", "21017", 2 },
                    { 303, 21020, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9505), "Kristijan Stojanovski", "Supply chain", "21020", 2 },
                    { 304, 21082, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9506), "Rufat Rufati", "Production", "21082", 2 },
                    { 305, 21090, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9506), "Vase Pecevski", "Projects and IT", "21090", 2 },
                    { 306, 21094, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9507), "Kristina Karajanovska", "Sales", "21094", 2 },
                    { 307, 21095, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9508), "Srechko Vidinski", "Production", "21095", 2 },
                    { 308, 21096, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9509), "Nikola Spasevski", "Projects and IT", "21096", 2 },
                    { 309, 21097, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9510), "Zvonko Miloshoski", "Supply chain", "21097", 2 },
                    { 310, 21100, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9511), "Ismail Redzepi", "Production", "21100", 2 },
                    { 311, 21104, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9512), "Aleksandar Kekenovski", "Production", "21104", 2 },
                    { 312, 21112, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9517), "Nikola Nikolovski", "Production", "21112", 2 },
                    { 313, 21117, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9517), "Jovica Stojanovikj", "Production", "21117", 2 },
                    { 314, 21119, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9518), "Vasil Kocevski", "Production", "21119", 2 },
                    { 315, 21121, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9519), "Petre Kushinovski", "Production", "21121", 2 },
                    { 316, 21125, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9520), "Mitko Lebamov", "Projects and IT", "21125", 2 },
                    { 317, 21126, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9521), "Aleksandar Boshkovski", "Production", "21126", 2 },
                    { 318, 21128, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9522), "Gjuner Ismailovski", "Projects and IT", "21128", 2 },
                    { 319, 21131, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9523), "Jovan Markovski", "Production", "21131", 2 },
                    { 320, 21133, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9524), "Kjamuran Muaremovski", "Production", "21133", 2 },
                    { 321, 21134, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9524), "Nikola Panovski", "Projects and IT", "21134", 2 },
                    { 322, 21136, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9525), "Stefan Ristovski", "Production", "21136", 2 },
                    { 323, 21139, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9526), "Jasin Ismailovski", "Production", "21139", 2 },
                    { 324, 21140, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9527), "Borko Sokolovikj", "Production", "21140", 2 },
                    { 325, 21142, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9528), "Stojan Despotoski", "Production", "21142", 2 },
                    { 326, 21143, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9529), "Shezair Lazam", "Production", "21143", 2 },
                    { 327, 21149, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9530), "Jovan Stojanovski", "Projects and IT", "21149", 2 },
                    { 328, 21151, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9531), "Kire Krusharski", "Production", "21151", 2 },
                    { 329, 21152, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9532), "Igorche Kuzmanov", "Production", "21152", 2 },
                    { 330, 21154, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9532), "Goce Zdravevski", "Supply chain", "21154", 2 },
                    { 331, 21156, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9534), "Goran Vasilevski", "Production", "21156", 2 },
                    { 332, 21160, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9535), "Deni Popovski", "Supply chain", "21160", 2 },
                    { 333, 21171, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9536), "Jovan Chankulovski", "Production", "21171", 2 },
                    { 334, 21174, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9537), "Dragi Risteski", "Projects and IT", "21174", 2 },
                    { 335, 21175, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9538), "Zoran Urdarevikj", "Production", "21175", 2 },
                    { 336, 21178, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9539), "Miroslav Martinovski", "Production", "21178", 2 },
                    { 337, 21183, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9540), "Emran Iseinov", "Production", "21183", 2 },
                    { 338, 21184, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9540), "Mirche Milkovski", "Production", "21184", 2 },
                    { 339, 21188, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9541), "Aleksandar Kitanovski", "Production", "21188", 2 },
                    { 340, 21189, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9583), "Dejan Stefanovski", "Production", "21189", 2 },
                    { 341, 21190, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9585), "Viktor Stojchevski", "Production", "21190", 2 },
                    { 342, 21191, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9586), "Dragan Risteski", "Supply chain", "21191", 2 },
                    { 343, 21193, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9586), "Dzemail Ljimani", "Production", "21193", 2 },
                    { 344, 21194, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9587), "Biljana Trajkovska", "Supply chain", "21194", 2 },
                    { 345, 21196, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9588), "Miroslav Krstikj", "Production", "21196", 2 },
                    { 346, 21197, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9589), "Violeta Stojanovska", "CEO office", "21197", 2 },
                    { 347, 21198, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9590), "Kristina Kolaroska", "Finance Department", "21198", 2 },
                    { 348, 21200, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9591), "David Savevski", "Production", "21200", 2 },
                    { 349, 21201, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9592), "Emrah Sali", "Production", "21201", 2 },
                    { 350, 21204, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9594), "Robert Ristovski", "Production", "21204", 2 },
                    { 351, 21206, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9595), "Marjanche Milkovski", "Projects and IT", "21206", 2 },
                    { 352, 21209, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9596), "Ice Trajkoski", "Production", "21209", 2 },
                    { 353, 21212, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9597), "Viktor Ilievski", "Production", "21212", 2 },
                    { 354, 21218, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9598), "Daniel Slavkovski", "Production", "21218", 2 },
                    { 355, 21219, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9599), "Goce Peshevski", "Production", "21219", 2 },
                    { 356, 21224, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9600), "Natasha Mihova", "Finance Department", "21224", 2 },
                    { 357, 21225, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9601), "Bujar Zenuli", "Production", "21225", 2 },
                    { 358, 21227, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9602), "Tamara Stojchevska", "HR", "21227", 2 },
                    { 359, 21229, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9603), "Dragana Velkovikj-Krsteva", "Supply chain", "21229", 2 },
                    { 360, 21231, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9604), "Jovica Stojanovski", "Production", "21231", 2 },
                    { 361, 21233, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9605), "Mario Trajkovski", "Projects and IT", "21233", 2 },
                    { 362, 21240, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9606), "Dancho Kostadinovski", "Projects and IT", "21240", 2 },
                    { 363, 21241, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9607), "Konstantin Koneski", "Supply chain", "21241", 2 },
                    { 364, 21243, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9608), "Nenad Mihajloski", "Production", "21243", 2 },
                    { 365, 21247, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9608), "Ilija Andonoski", "Supply chain", "21247", 2 },
                    { 366, 21252, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9609), "Toni Karovchevikj", "Projects and IT", "21252", 2 },
                    { 367, 21254, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9610), "Hristijan Todorovski", "Projects and IT", "21254", 2 },
                    { 368, 21257, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9611), "Atanas Boshkov", "Production", "21257", 2 },
                    { 369, 21259, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9612), "Damjan Petrovski", "Projects and IT", "21259", 2 },
                    { 370, 21260, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9614), "Viktorija Karafiloska", "Supply chain", "21260", 2 },
                    { 371, 21261, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9614), "Sashko Janevski", "Production", "21261", 2 },
                    { 372, 21262, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9615), "Maja Miloshoska", "Supply chain", "21262", 2 },
                    { 373, 21263, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9616), "Elena Stoilkovska", "HR", "21263", 2 },
                    { 374, 21268, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9617), "Dragan Najdovski", "Projects and IT", "21268", 2 },
                    { 375, 21269, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9618), "Luka Bostandzievski", "Production", "21269", 2 },
                    { 376, 21270, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9619), "Sinisha Voinoski", "Production", "21270", 2 },
                    { 377, 21271, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9620), "Muhamed Mimin", "Production", "21271", 2 },
                    { 378, 21274, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9621), "Nuija Nuijovski", "Projects and IT", "21274", 2 },
                    { 379, 21275, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9622), "Svetlana Davkovska", "Finance Department", "21275", 2 },
                    { 380, 21277, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9623), "Isa Zenelji", "Production", "21277", 2 },
                    { 381, 21280, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9624), "Mario Nikolovski", "Projects and IT", "21280", 2 },
                    { 382, 21281, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9625), "Angel Kostovski", "Production", "21281", 2 },
                    { 383, 21282, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9626), "Hristijan Stevkovski", "Supply chain", "21282", 2 },
                    { 384, 21283, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9627), "Naim Ajvazi", "Production", "21283", 2 },
                    { 385, 21284, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9628), "Miodrag Achkovikj", "Production", "21284", 2 },
                    { 386, 21285, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9628), "Andrej Velichkovski", "Projects and IT", "21285", 2 },
                    { 387, 21286, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9629), "Dejan Smilevski", "Projects and IT", "21286", 2 },
                    { 388, 21288, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9631), "Trajche Trajkovski", "Production", "21288", 2 },
                    { 389, 21290, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9632), "Sashko Dimovski", "Projects and IT", "21290", 2 },
                    { 390, 21292, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9633), "Dushan Manojlovikj", "Production", "21292", 2 },
                    { 391, 21293, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9634), "Zoran Ilieski", "Projects and IT", "21293", 2 },
                    { 392, 21294, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9635), "Antonio Panovski", "Production", "21294", 2 },
                    { 393, 21295, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9636), "Violeta Joshovikj", "HR", "21295", 2 },
                    { 394, 21297, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9637), "Sashka Stojanovska", "HR", "21297", 2 },
                    { 395, 21298, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9638), "Ljupcho Emsherijov", "Production", "21298", 2 },
                    { 396, 21299, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9638), "Nikola Risteski", "Supply chain", "21299", 2 },
                    { 397, 21300, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9639), "Ljupcho Bogojev", "Projects and IT", "21300", 2 },
                    { 398, 21302, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9640), "Erol Idriz", "Projects and IT", "21302", 2 },
                    { 399, 21303, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9641), "Blagoja Jovchevski", "Projects and IT", "21303", 2 },
                    { 400, 21304, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9642), "Stefan Trajkovikj", "Production", "21304", 2 },
                    { 401, 21305, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9643), "Vesna Gjorgjevska", "HR", "21305", 2 },
                    { 402, 21306, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9644), "Mihaela Gecheva", "HR", "21306", 2 },
                    { 403, 21307, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9645), "Marija Malinova", "Supply chain", "21307", 2 },
                    { 404, 21308, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9646), "Viktorija Siljanoska", "Projects and IT", "21308", 2 },
                    { 405, 21309, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9647), "Aleksandar Paunkovikj", "Projects and IT", "21309", 2 },
                    { 406, 21310, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9648), "Stefan Cvetanovski", "Production", "21310", 2 },
                    { 407, 21311, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9649), "Valentina Cibreva", "Finance Department", "21311", 2 },
                    { 408, 21312, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9650), "Milancho Uroshevski", "Supply chain", "21312", 2 },
                    { 409, 21313, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9651), "Jashar Ismaili", "HR", "21313", 2 },
                    { 410, 21314, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9652), "Daniel Neshkovikj", "Daniel", "21314", 2 },
                    { 411, 21315, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9653), "Hristina Jovanovska", "Projects and IT", "21315", 2 },
                    { 412, 21316, new DateTime(2025, 10, 3, 11, 56, 44, 210, DateTimeKind.Utc).AddTicks(9654), "Marjan Georgiev", "Production", "21316", 2 }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "IsRequired", "QuestionFormId", "QuestionTypeId", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, true, 1, 1, "Задоволен сум од мојата моментална работа", 1 },
                    { 2, true, 1, 1, "Чувствувам дека мојата работа е ценета во рамките на компанијата", 1 },
                    { 3, true, 1, 1, "Се чувствувам мотивиран да одам на работа секој ден", 1 },
                    { 4, true, 2, 1, "Се чувствувам горд што работам за оваа компанија", 1 },
                    { 5, true, 2, 1, "Со задоволство ја препорачувам оваа компанија како работно место на пријателите и семејството", 1 },
                    { 6, true, 2, 1, "Се гледам себеси како долгорочно работам во оваа компанија", 1 },
                    { 7, true, 3, 1, "Имам можности за професионален развој и напредување", 1 },
                    { 8, true, 3, 1, "Добивам конструктивна повратна информација за мојата работа", 1 },
                    { 9, true, 3, 1, "Компанијата обезбедува соодветна обука и ресурси за мојот развој", 1 },
                    { 10, true, 4, 1, "Компанијата поддржува здрава рамнотежа помеѓу работата и личниот живот", 1 },
                    { 11, true, 4, 1, "Можам ефикасно да управувам со стресот поврзан со работата", 1 },
                    { 12, true, 4, 1, "Мојот работен распоред ми овозможува да ги исполнувам моите лични обврски", 1 },
                    { 13, true, 5, 1, "Комуникацијата во мојот тим е ефикасна", 1 },
                    { 14, true, 5, 1, "Се чувствувам удобно да ги искажувам моите идеи и мислења на работа", 1 },
                    { 15, true, 5, 1, "Соработката помеѓу одделенијата е ефикасна", 1 },
                    { 16, true, 6, 1, "Му верувам на раководството на компанијата", 1 },
                    { 17, true, 6, 1, "Мојот директен менаџер ме поддржува во остварувањето на моите цели", 1 },
                    { 18, true, 6, 1, "Важните одлуки на компанијата се пренесуваат транспарентно", 1 },
                    { 19, true, 7, 1, "Вредностите на компанијата се усогласуваат со моите лични вредности", 1 },
                    { 20, true, 7, 1, "Се чувствувам вклучено и почитувано на работа", 1 },
                    { 21, true, 7, 1, "Компанијата промовира различност и инклузија", 1 },
                    { 22, true, 8, 1, "Ги имам сите ресурси потребни за ефикасно извршување на моите задачи", 1 },
                    { 23, true, 8, 1, "Физичката работна средина е удобна и поволна за продуктивност", 1 },
                    { 24, true, 8, 1, "Се чувствувам безбедно на работа", 1 },
                    { 26, true, 9, 1, "Задоволен сум од мојот пакет компензации и бенефиции", 1 },
                    { 27, true, 9, 1, "Моите напори и достигнувања се препознаени и ценети", 1 },
                    { 28, true, 9, 1, "Постојат јасни можности за напредување во кариерата во рамките на компанијата", 1 },
                    { 29, true, 10, 1, "Компанијата ги поттикнува иновациите и креативното размислување", 1 },
                    { 30, true, 10, 1, "Подготвен сум да ги усвојам промените имплементирани во компанијата", 1 },
                    { 31, true, 10, 1, "Идеите и предлозите на вработените се разгледуваат и се спроведуваат кога е соодветно", 1 },
                    { 32, true, 11, 1, "Kолку е веројатно да ја препорачате оваа компанија како работно место на пријател или колега", 1 },
                    { 33, true, 11, 2, "Како ја гледате вашата иднина во оваа компанија во следните 2-3 години?", 1 },
                    { 34, true, 11, 2, "разно", 1 },
                    { 35, true, 12, 2, "Што најмногу ви се допаѓа на вашето сегашно работно место?", 1 },
                    { 36, true, 12, 2, "Кои се најголемите предизвици со кои се соочувате на работа?", 1 },
                    { 37, true, 12, 2, "Какви предлози имате за подобрување на работната средина или процесите на компанијата?", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionFormId",
                table: "Answers",
                column: "QuestionFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_UserId",
                table: "Answers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionFormId",
                table: "Questions",
                column: "QuestionFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId",
                table: "Questions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionForms");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
