using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GlasAnketa.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initdobra : Migration
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
                    OU2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                        principalColumn: "Id");
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id");
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
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionFormId = table.Column<int>(type: "int", nullable: false),
                    ScaleValue = table.Column<int>(type: "int", nullable: true),
                    TextValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    { 1, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8354), "Overall Satisfaction", true, "Општо задоволство" },
                    { 2, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8358), "Commitment to the Company", true, "Обврска кон компанијата" },
                    { 3, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8360), "Professional Development", true, "Професионален развој" },
                    { 4, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8361), "Work-Life Balance", true, "Рамнотежа помеѓу работата и животот" },
                    { 5, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8361), "Communication and Collaboration", true, "Комуникација и соработка" },
                    { 6, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8362), "Leadership", true, "Лидерство" },
                    { 7, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8363), "Organizational Culture", true, "Организациска култура" },
                    { 8, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8364), "Work Environment", true, "Работна средина" },
                    { 9, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8365), "Rewards and Recognition", true, "Награди и признанија" },
                    { 10, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8366), "Innovation and Changes", true, "Иновации и промени" },
                    { 11, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8367), "Final Evaluation", true, "Конечна евалуација" },
                    { 12, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8368), "Open Questions", true, "Отворени прашања" }
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
                columns: new[] { "Id", "CompanyId", "CreatedDate", "FullName", "OU", "OU2", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, 16130, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7381), "Vasko Gjorgiev", "Production", "Rolling Unit", "16130", 2 },
                    { 2, 16684, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7386), "Zoran Stojanovski", "Production", "Rolling Unit", "16684", 2 },
                    { 3, 16695, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7388), "Pane Naskovski", "Production", "Rolling Unit", "16695", 2 },
                    { 4, 16720, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7389), "Tome Trajanov", "Projects and IT", "Crane Maintenance & Internal Transport", "16720", 2 },
                    { 5, 16827, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7391), "Zoran Boshkovski", "Production", "Rolling Unit", "16827", 2 },
                    { 6, 16984, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7392), "Dide Petrovski", "Projects and IT", "Central mechanical maintenance", "16984", 2 },
                    { 7, 17011, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7393), "Jovica Gjorgievski", "Projects and IT", "Crane Maintenance & Internal Transport", "17011", 2 },
                    { 8, 17055, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7394), "Blagica Besarovska", "Projects and IT", "Crane Maintenance & Internal Transport", "17055", 2 },
                    { 9, 17064, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7396), "Dragi Naskovski", "Production", "Coating Unit", "17064", 2 },
                    { 10, 17148, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7397), "Borche Anchevski", "Production", "Coating Unit", "17148", 2 },
                    { 11, 17772, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7398), "Toni Nacev", "HR", "Facility", "17772", 2 },
                    { 12, 17884, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7400), "Valentina Kostovska", "HR", "Human Resources", "17884", 2 },
                    { 13, 17893, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7401), "Zoran Tripunoski", "Production", "Rolling Unit", "17893", 2 },
                    { 14, 17896, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7402), "Zorancho Taseski", "Projects and IT", "Crane Maintenance & Internal Transport", "17896", 2 },
                    { 15, 18158, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7403), "Goran Despodovski", "Production", "Coating Unit", "18158", 2 },
                    { 16, 18162, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7404), "Ljupcho Krstevski", "Production", "Rolling Unit", "18162", 2 },
                    { 17, 18392, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7405), "Sabedin Ljura", "Production", "Coating Unit", "18392", 2 },
                    { 18, 18412, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7407), "Rade Milenkovski", "Production", "Coating Unit", "18412", 2 },
                    { 19, 18471, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7408), "Stojka Koneska", "Supply chain", "Customer service & Logistics", "18471", 2 },
                    { 20, 18529, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7409), "Zharko Nikolovski", "Production", "Rolling Unit", "18529", 2 },
                    { 21, 18533, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7410), "Radica Angelovska", "Projects and IT", "Crane Maintenance & Internal Transport", "18533", 2 },
                    { 22, 18874, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7411), "Borche Trifunovski", "Production", "Coating Unit", "18874", 2 },
                    { 23, 18876, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7412), "Pero Stojanovski", "Production", "Coating Unit", "18876", 2 },
                    { 24, 19370, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7414), "Dragi Petrovski", "Production", "Coating Unit", "19370", 2 },
                    { 25, 19379, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7415), "Ilo Risteski", "Supply chain", "Quality control", "19379", 2 },
                    { 26, 19767, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7416), "Aleksandar Iliev", "Production", "Coating Unit", "19767", 2 },
                    { 27, 19775, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7417), "Mile Popovski", "Production", "Rolling Unit", "19775", 2 },
                    { 28, 19776, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7418), "Dragan Hristovski", "Production", "Coating Unit", "19776", 2 },
                    { 29, 19777, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7420), "Aleksandar Jovchevski", "Production", "Coating Unit", "19777", 2 },
                    { 30, 19779, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7421), "Ljupcho Andovski", "Supply chain", "Quality control", "19779", 2 },
                    { 31, 19782, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7422), "Ivica Stanchevski", "Production", "Rolling Unit", "19782", 2 },
                    { 32, 19784, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7451), "Biljana Ilievska", "Supply chain", "Quality control", "19784", 2 },
                    { 33, 19787, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7453), "Goran Damjanoski", "Supply chain", "Quality control", "19787", 2 },
                    { 34, 19788, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7454), "Boban Neshovski", "Production", "Rolling Unit", "19788", 2 },
                    { 35, 19795, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7456), "Sashe Taparchevski", "Production", "Coating Unit", "19795", 2 },
                    { 36, 19796, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7457), "Igor Ristovski", "Production", "Coating Unit", "19796", 2 },
                    { 37, 19798, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7458), "Ivica Trajkovski", "Production", "Rolling Unit", "19798", 2 },
                    { 38, 19801, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7459), "Vlado Stojanovski", "Production", "Coating Unit", "19801", 2 },
                    { 39, 19804, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7461), "Goran Spirovski", "Production", "Coating Unit", "19804", 2 },
                    { 40, 19806, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7462), "Dejan Velkovski", "Production", "Coating Unit", "19806", 2 },
                    { 41, 19807, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7463), "Stojanche Stefkovski", "Production", "Coating Unit", "19807", 2 },
                    { 42, 19811, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7464), "Dancho Blazheski", "Production", "Rolling Unit", "19811", 2 },
                    { 43, 19813, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7465), "Ljupcho Lozanovski", "Production", "Coating Unit", "19813", 2 },
                    { 44, 19818, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7467), "Marjan Nedelkovski", "Projects and IT", "High voltage", "19818", 2 },
                    { 45, 19820, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7468), "Srgjan Stanojevikj", "Production", "Coating Unit", "19820", 2 },
                    { 46, 19822, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7469), "Dragan Spasevski", "Supply chain", "Quality control", "19822", 2 },
                    { 47, 19823, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7470), "Goran Andonovski", "Projects and IT", "Crane Maintenance & Internal Transport", "19823", 2 },
                    { 48, 19827, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7471), "Goran Anchovski", "Supply chain", "Planning & Strategy", "19827", 2 },
                    { 49, 19833, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7472), "Igor Mircheski", "Supply chain", "Stores", "19833", 2 },
                    { 50, 19834, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7474), "Goran Nikolovski", "HR", "Facility", "19834", 2 },
                    { 51, 19838, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7475), "Petar Moskov", "Production", "Rolling Unit", "19838", 2 },
                    { 52, 19840, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7477), "Goran Stojanovski", "Supply chain", "Planning & Strategy", "19840", 2 },
                    { 53, 19841, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7478), "Igor Petkovski", "Projects and IT", "Crane Maintenance & Internal Transport", "19841", 2 },
                    { 54, 19842, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7479), "Nenad Mitrovikj", "Production", "Coating Unit", "19842", 2 },
                    { 55, 19844, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7480), "Sashko Gjorgjievski", "Supply chain", "Quality control", "19844", 2 },
                    { 56, 19845, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7481), "Nikola Toshevski", "Production", "Coating Unit", "19845", 2 },
                    { 57, 19848, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7487), "Slobodan Velkovski", "Production", "Coating Unit", "19848", 2 },
                    { 58, 19849, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7488), "Goce Jankulovski", "Supply chain", "Planning & Strategy", "19849", 2 },
                    { 59, 19868, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7489), "Marjan Milovanovikj", "Production", "Rolling Unit", "19868", 2 },
                    { 60, 19877, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7491), "Goran Gavrilovski", "Sales", "Sales", "19877", 2 },
                    { 61, 19892, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7493), "Irfan Feratovski", "Production", "Rolling Unit", "19892", 2 },
                    { 62, 19899, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7494), "Igor Krpachovski", "Projects and IT", "Projects, progress and IT", "19899", 2 },
                    { 63, 19911, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7495), "Aleksandar Spasevski", "CEO office", "Health, Safety and Environment", "19911", 2 },
                    { 64, 19916, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7496), "Nevaip Bardi", "Production", "Rolling Unit", "19916", 2 },
                    { 65, 19917, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7498), "Biljana Stoshikj", "Supply chain", "Customer service & Logistics", "19917", 2 },
                    { 66, 19933, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7499), "Svetlana Jovanova", "Finance Department", "Financial Controlling and Reporting", "19933", 2 },
                    { 67, 19960, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7500), "Draganche Taleski", "Production", "Rolling Unit", "19960", 2 },
                    { 68, 19963, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7501), "Toni Naumovski", "Production", "Coating Unit", "19963", 2 },
                    { 69, 19992, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7502), "Metodi Gievski", "Projects and IT", "Projects", "19992", 2 },
                    { 70, 19993, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7504), "Jovica Velkovski", "Supply chain", "Customer service & Logistics", "19993", 2 },
                    { 71, 19997, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7505), "Gordana Astardjieva", "Projects and IT", "High voltage", "19997", 2 },
                    { 72, 20023, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7506), "Zharko Ivanovski", "Production", "Coating Unit", "20023", 2 },
                    { 73, 20024, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7507), "Igorche Janev", "Production", "Rolling Unit", "20024", 2 },
                    { 74, 20033, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7508), "Nikola Panov", "Supply chain", "Quality control", "20033", 2 },
                    { 75, 20034, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7509), "Sasho Mitkovski", "Production", "Coating Unit", "20034", 2 },
                    { 76, 20038, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7511), "Goran Ilievski", "Production", "Rolling Unit", "20038", 2 },
                    { 77, 20041, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7512), "Kircho Merdjanovski", "Projects and IT", "Automation", "20041", 2 },
                    { 78, 20052, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7513), "Davor Zdravkovski", "Supply chain", "Customer service & Logistics", "20052", 2 },
                    { 79, 20072, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7514), "Gorancho Petkovski", "Production", "Quality control", "20072", 2 },
                    { 80, 20076, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7515), "Sashko Cvetanovski", "Production", "Coating Unit", "20076", 2 },
                    { 81, 20095, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7517), "Ilija Tashevski", "Production", "Crane Maintenance & Internal Transport", "20095", 2 },
                    { 82, 20117, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7523), "Kire Stefanoski", "Production", "Rolling Unit", "20117", 2 },
                    { 83, 20125, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7524), "Aleksandar Evremov", "Supply chain", "Coating Unit", "20125", 2 },
                    { 84, 20127, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7525), "Ratko Trajkovski", "Supply chain", "Crane Maintenance & Internal Transport", "20127", 2 },
                    { 85, 20128, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7527), "Goran Miovski", "Projects and IT", "Quality control", "20128", 2 },
                    { 86, 20131, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7551), "Goran Trajkovski", "Production", "Rolling Unit", "20131", 2 },
                    { 87, 20137, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7553), "Gordana Shegmanovikj", "HR", "Facility", "20137", 2 },
                    { 88, 20144, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7554), "Igorche Bogdanovski", "Production", "Coating Unit", "20144", 2 },
                    { 89, 20152, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7555), "Miodrag Petkovikj", "Production", "Maintenance Progress", "20152", 2 },
                    { 90, 20159, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7557), "Gorancho Najdovski", "Projects and IT", "Coating Unit", "20159", 2 },
                    { 91, 20160, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7558), "Dejan Jazadjiev", "Supply chain", "Coating Unit", "20160", 2 },
                    { 92, 20162, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7559), "Sashko Peshov", "Production", "Crane Maintenance & Internal Transport", "20162", 2 },
                    { 93, 20163, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7560), "Kiro Radevski", "Production", "Rolling Unit", "20163", 2 },
                    { 94, 20167, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7561), "Sasho Beroski", "Production", "Coating Unit", "20167", 2 },
                    { 95, 20168, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7563), "Vlatko Changovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20168", 2 },
                    { 96, 20178, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7564), "Stojan Stavreski", "Projects and IT", "Crane Maintenance & Internal Transport", "20178", 2 },
                    { 97, 20182, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7565), "Kjemalj Abazi", "Production", "Coating Unit", "20182", 2 },
                    { 98, 20184, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7566), "Djevat Selimi", "Production", "Rolling Unit", "20184", 2 },
                    { 99, 20191, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7567), "Trajche Dimovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20191", 2 },
                    { 100, 20195, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7569), "Robert Shijakovski", "Production", "Rolling Unit", "20195", 2 },
                    { 101, 20203, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7571), "Slavisha Crnichin", "Production", "Coating Unit", "20203", 2 },
                    { 102, 20210, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7572), "Borche Cvetkovski", "Supply chain", "Customer service & Logistics", "20210", 2 },
                    { 103, 20212, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7573), "Sasha Stefanoski", "Supply chain", "Customer service & Logistics", "20212", 2 },
                    { 104, 20218, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7575), "Josif Slavkovski", "Production", "Central mechanical maintenance", "20218", 2 },
                    { 105, 20225, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7576), "Goce Stojchevski", "Projects and IT", "Customer service & Logistics", "20225", 2 },
                    { 106, 20226, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7577), "Donche Nedelkovski", "Production", "Quality control", "20226", 2 },
                    { 107, 20231, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7578), "Ljupcho Shegmanovikj", "Production", "Coating Unit", "20231", 2 },
                    { 108, 20232, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7579), "Goran Markovski", "Production", "Stores", "20232", 2 },
                    { 109, 20233, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7580), "Dragan Markovski", "Projects and IT", "Customer service & Logistics", "20233", 2 },
                    { 110, 20234, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7582), "Ljupcho Veljanovski", "Production", "Coating Unit", "20234", 2 },
                    { 111, 20235, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7583), "Nikola Angeleski", "Production", "Coating Unit", "20235", 2 },
                    { 112, 20236, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7584), "Aleksandar Bogoev", "Production", "Customer service & Logistics", "20236", 2 },
                    { 113, 20238, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7585), "Stevche Velkovski", "Projects and IT", "Facility", "20238", 2 },
                    { 114, 20245, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7586), "Ljubomir Kochovski", "Projects and IT", "Rolling Unit", "20245", 2 },
                    { 115, 20246, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7587), "Sashko Blazhevski", "Production", "Crane Maintenance & Internal Transport", "20246", 2 },
                    { 116, 20248, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7589), "Zoranche Borizovski", "Production", "Quality control", "20248", 2 },
                    { 117, 20253, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7590), "Nebojsha Stojmanovikj", "Projects and IT", "Rolling Unit", "20253", 2 },
                    { 118, 20255, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7591), "Ljupcho Pashoski", "Production", "Coating Unit", "20255", 2 },
                    { 119, 20261, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7592), "Aleksandar Karajanovski", "Production", "Crane Maintenance & Internal Transport", "20261", 2 },
                    { 120, 20262, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7593), "Dejan Stojanov", "Supply chain", "Rolling Unit", "20262", 2 },
                    { 121, 20263, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7596), "Vladimir Jakimov", "Projects and IT", "Coating Unit", "20263", 2 },
                    { 122, 20267, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7597), "Goranche Ginoski", "Production", "Coating Unit", "20267", 2 },
                    { 123, 20271, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7598), "Avdil Mustafa", "Production", "Customer service & Logistics", "20271", 2 },
                    { 124, 20272, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7600), "Beta Damevska", "Projects and IT", "Crane Maintenance & Internal Transport", "20272", 2 },
                    { 125, 20275, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7601), "Naser Ilazov", "Production", "Rolling Unit", "20275", 2 },
                    { 126, 20280, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7602), "Viktor Boshkovski", "Production", "Planning & Strategy", "20280", 2 },
                    { 127, 20283, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7603), "Zlatko Petrovski", "Supply chain", "Rolling Unit", "20283", 2 },
                    { 128, 20284, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7604), "Aleksandar Stoicev", "Production", "Coating Unit", "20284", 2 },
                    { 129, 20286, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7605), "Aleksandar Jovanovski", "Production", "Coating Unit", "20286", 2 },
                    { 130, 20296, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7607), "Goran Jovanovski", "Production", "Coating Unit", "20296", 2 },
                    { 131, 20297, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7609), "Ivan Maslov", "Projects and IT", "Coating Unit", "20297", 2 },
                    { 132, 20298, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7610), "Ivica Tripunovski", "Supply chain", "Coating Unit", "20298", 2 },
                    { 133, 20299, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7611), "Milisav Boshkovikj", "Production", "Coating Unit", "20299", 2 },
                    { 134, 20300, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7612), "Ilija Pandurski", "Production", "Coating Unit", "20300", 2 },
                    { 135, 20308, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7614), "Ljuben Trajkoski", "Supply chain", "Customer service & Logistics", "20308", 2 },
                    { 136, 20316, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7615), "Igor Petrovski", "Projects and IT", "Purchasing", "20316", 2 },
                    { 137, 20320, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7616), "Brankica Trajanoska", "Supply chain", "Crane Maintenance & Internal Transport", "20320", 2 },
                    { 138, 20323, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7617), "Blazhe Dimov", "Projects and IT", "Customer service & Logistics", "20323", 2 },
                    { 139, 20324, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7618), "Biljana Petrovska", "Supply chain", "Customer service & Logistics", "20324", 2 },
                    { 140, 20325, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7645), "Zoran Trajkov", "Projects and IT", "Central mechanical maintenance", "20325", 2 },
                    { 141, 20328, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7647), "Slavko Spasovski", "HR", "Union", "20328", 2 },
                    { 142, 20332, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7648), "Igorche Gjorgjievski", "Projects and IT", "Crane Maintenance & Internal Transport", "20332", 2 },
                    { 143, 20335, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7649), "Jovica Boshkovski", "Production", "Rolling Unit", "20335", 2 },
                    { 144, 20341, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7651), "Metodija Blazhevski", "Supply chain", "Customer service & Logistics", "20341", 2 },
                    { 145, 20350, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7653), "Sasho Petrushevski", "Supply chain", "Customer service & Logistics", "20350", 2 },
                    { 146, 20351, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7654), "Marjan Stojanovski", "Supply chain", "Customer service & Logistics", "20351", 2 },
                    { 147, 20357, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7655), "Dejan Petrushevski", "Sales", "Sales", "20357", 2 },
                    { 148, 20362, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7656), "Sasho Kiprijanovski", "Supply chain", "Quality control", "20362", 2 },
                    { 149, 20363, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7657), "Zoran Mitevski", "Production", "Coating Unit", "20363", 2 },
                    { 150, 20372, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7659), "Sasho Gjorgjievski", "Production", "Stores", "20372", 2 },
                    { 151, 20380, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7660), "Orce Angelovski", "Projects and IT", "Customer service & Logistics", "20380", 2 },
                    { 152, 20381, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7662), "Dejan Danilovski", "Supply chain", "Coating Unit", "20381", 2 },
                    { 153, 20382, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7664), "Robert Angelovski", "Projects and IT", "Coating Unit", "20382", 2 },
                    { 154, 20385, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7666), "Bratislav Mihajlovikj", "Supply chain", "Customer service & Logistics", "20385", 2 },
                    { 155, 20387, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7667), "Ace Mitevski", "Supply chain", "Facility", "20387", 2 },
                    { 156, 20389, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7668), "Igorche Markovski", "Production", "Rolling Unit", "20389", 2 },
                    { 157, 20390, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7669), "Jovan Markovski", "Supply chain", "Crane Maintenance & Internal Transport", "20390", 2 },
                    { 158, 20393, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7670), "Boban Hristovski", "Supply chain", "Quality control", "20393", 2 },
                    { 159, 20395, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7672), "Dejan Dimishkovski", "Production", "Rolling Unit", "20395", 2 },
                    { 160, 20397, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7673), "Marjan Simonovski", "Production", "Coating Unit", "20397", 2 },
                    { 161, 20402, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7674), "Slavica Mladenovska", "Supply chain", "High voltage", "20402", 2 },
                    { 162, 20431, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7675), "Angelina Rajovska", "HR", "Quality control", "20431", 2 },
                    { 163, 20439, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7676), "Dejan Dilevski", "Production", "Rolling Unit", "20439", 2 },
                    { 164, 20443, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7677), "Aleksandar Zotikj", "Supply chain", "Crane Maintenance & Internal Transport", "20443", 2 },
                    { 165, 20445, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7679), "Jovica Maznevski", "Production", "Quality control", "20445", 2 },
                    { 166, 20447, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7681), "Zvonko Neshkovikj", "Production", "Rolling Unit", "20447", 2 },
                    { 167, 20448, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7682), "Ljupcho Paunkov", "Production", "Coating Unit", "20448", 2 },
                    { 168, 20449, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7683), "Marjan Zdravkovski", "Production", "Crane Maintenance & Internal Transport", "20449", 2 },
                    { 169, 20451, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7684), "Igor Jordanovski", "Production", "Rolling Unit", "20451", 2 },
                    { 170, 20453, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7685), "Aleksandar Stamchevski", "Production", "Coating Unit", "20453", 2 },
                    { 171, 20454, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7687), "Dejan Vasilevski", "Production", "Crane Maintenance & Internal Transport", "20454", 2 },
                    { 172, 20459, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7688), "Zoran Stojanovski", "Production", "Rolling Unit", "20459", 2 },
                    { 173, 20466, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7690), "Iljaz Prekopuca", "Production", "Coating Unit", "20466", 2 },
                    { 174, 20468, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7691), "Fadil Tanalari", "Production", "Crane Maintenance & Internal Transport", "20468", 2 },
                    { 175, 20471, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7692), "Trajche Petrovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20471", 2 },
                    { 176, 20475, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7693), "Boban Mitrovikj", "Production", "Rolling Unit", "20475", 2 },
                    { 177, 20478, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7694), "Dragan Saveski", "Production", "Coating Unit", "20478", 2 },
                    { 178, 20489, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7696), "Ajdin Zulfiovski", "Production", "Coating Unit", "20489", 2 },
                    { 179, 20511, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7697), "Ivan Cibrev", "Supply chain", "Stores", "20511", 2 },
                    { 180, 20518, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7698), "Slavica Jovchevska", "Supply chain", "Customer service & Logistics", "20518", 2 },
                    { 181, 20521, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7699), "Elena Damchevska", "Finance Department", "Accounting & Treasury", "20521", 2 },
                    { 182, 20523, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7700), "Vesna Dimevska", "Supply chain", "Quality control", "20523", 2 },
                    { 183, 20527, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7701), "Kiril Simonoski", "Projects and IT", "Maintenance Progress", "20527", 2 },
                    { 184, 20530, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7703), "Goce Atanasoski", "Projects and IT", "Maintenance Progress", "20530", 2 },
                    { 185, 20603, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7704), "Goran Stojchevski", "Production", "Coating Unit", "20603", 2 },
                    { 186, 20621, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7705), "Todorka Ristovska", "CEO office", "CEO office", "20621", 2 },
                    { 187, 20623, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7707), "Elena Blazeva", "Finance Department", "Finance Department", "20623", 2 },
                    { 188, 20625, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7708), "Darko Najdenov", "Supply chain", "Purchasing", "20625", 2 },
                    { 189, 20632, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7709), "Zoran Mladenovski", "Projects and IT", "Projects, progress and IT", "20632", 2 },
                    { 190, 20636, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7710), "Natalija Nikoloska", "Supply chain", "Quality control", "20636", 2 },
                    { 191, 20637, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7711), "Aleksandar Krstev", "Supply chain", "Planning & Strategy", "20637", 2 },
                    { 192, 20638, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7712), "Elena Kocevska Peceva", "Supply chain", "Quality control", "20638", 2 },
                    { 193, 20640, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7713), "Kiro Risteski", "Production", "Production", "20640", 2 },
                    { 194, 20650, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7738), "Dejana Jovanova Krsteva", "Supply chain", "Supply chain", "20650", 2 },
                    { 195, 20652, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7740), "Toni Pandilovski", "Projects and IT", "Automation", "20652", 2 },
                    { 196, 20662, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7741), "Vladimir Shulevski", "Production", "Coating Unit", "20662", 2 },
                    { 197, 20675, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7742), "Dejan Trajkovski", "HR", "Internal Control", "20675", 2 },
                    { 198, 20678, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7743), "Kire Blagoeski", "Supply chain", "Planning & Strategy", "20678", 2 },
                    { 199, 20685, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7745), "Petar Brashnarov", "Production", "Coating Unit", "20685", 2 },
                    { 200, 20694, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7746), "Zvonimir Manchevski", "Production", "Rolling Unit", "20694", 2 },
                    { 201, 20695, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7747), "Aleksandar Dejanovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20695", 2 },
                    { 202, 20707, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7748), "Selaedin Feratovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20707", 2 },
                    { 203, 20708, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7751), "Slave Manevski", "Projects and IT", "Information technology", "20708", 1 },
                    { 204, 20723, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7752), "Djevat Saliovski", "Production", "Rolling Unit", "20723", 2 },
                    { 205, 20724, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7753), "Vesna Velichkovska", "HR", "Human Resources and Legal Affairs", "20724", 1 },
                    { 206, 20729, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7754), "Vlatko Dimishkovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20729", 2 },
                    { 207, 20734, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7756), "Blage Uroshevski", "Production", "Coating Unit", "20734", 2 },
                    { 208, 20735, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7757), "Stojadin Jankovski", "Production", "Rolling Unit", "20735", 2 },
                    { 209, 20737, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7758), "Zlatko Nikoloski", "Production", "Rolling Unit", "20737", 2 },
                    { 210, 20747, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7759), "Goce Gjorgjievski", "Production", "Rolling Unit", "20747", 2 },
                    { 211, 20751, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7760), "Stefan Tonevski", "Supply chain", "Quality control", "20751", 2 },
                    { 212, 20753, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7761), "Orce Dimovski", "Production", "Rolling Unit", "20753", 2 },
                    { 213, 20758, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7763), "Elena Valkancheva Najdenova", "Sales", "Sales", "20758", 2 },
                    { 214, 20776, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7764), "Igor Dushanovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20776", 2 },
                    { 215, 20779, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7765), "Jagoda Velevska", "CEO office", "Internal Audit", "20779", 2 },
                    { 216, 20781, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7766), "Vladimir Nikolikj", "Supply chain", "Quality control", "20781", 2 },
                    { 217, 20784, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7768), "Dejan Gocevski", "Production", "Coating Unit", "20784", 2 },
                    { 218, 20787, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7769), "Aleksandar Kostovski", "Production", "Rolling Unit", "20787", 2 },
                    { 219, 20797, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7770), "Cane Nikoloski", "Production", "Automation", "20797", 2 },
                    { 220, 20800, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7771), "Viktor Stamenkovski", "Projects and IT", "Quality control", "20800", 2 },
                    { 221, 20801, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7773), "Dragana Petrovikj", "Supply chain", "Quality control", "20801", 2 },
                    { 222, 20802, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7774), "Stefan Despodovski", "Supply chain", "Central mechanical maintenance", "20802", 2 },
                    { 223, 20803, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7775), "Marjan Milanovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20803", 2 },
                    { 224, 20804, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7777), "Dragan Koneski", "Projects and IT", "Coating Unit", "20804", 2 },
                    { 225, 20814, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7778), "Aleksandar Stojanovski", "Production", "Coating Unit", "20814", 2 },
                    { 226, 20815, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7779), "Sashko Miloshevski", "Production", "Crane Maintenance & Internal Transport", "20815", 2 },
                    { 227, 20822, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7780), "Elza Petrovska", "Sales", "Rolling Unit", "20822", 2 },
                    { 228, 20824, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7781), "Darko Zdravkovski", "Production", "Coating Unit", "20824", 2 },
                    { 229, 20825, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7782), "Kiril Chirkov", "Production", "Crane Maintenance & Internal Transport", "20825", 2 },
                    { 230, 20827, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7784), "Igor Cvetanoski", "Production", "Rolling Unit", "20827", 2 },
                    { 231, 20831, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7785), "Martin Nikolovski", "Production", "Coating Unit", "20831", 2 },
                    { 232, 20832, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7786), "Dushko Blazevski", "Supply chain", "Coating Unit", "20832", 2 },
                    { 233, 20834, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7787), "Muammet Sali", "Projects and IT", "Crane Maintenance & Internal Transport", "20834", 2 },
                    { 234, 20835, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7788), "Kristijan Janev", "Projects and IT", "Rolling Unit", "20835", 2 },
                    { 235, 20837, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7790), "Dilaver Sali", "Production", "Coating Unit", "20837", 2 },
                    { 236, 20838, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7791), "Sasho Neshkov", "Production", "Crane Maintenance & Internal Transport", "20838", 2 },
                    { 237, 20839, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7792), "Goran Ilikj", "Production", "Rolling Unit", "20839", 2 },
                    { 238, 20842, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7794), "Zoran Trendevski", "Production", "Customer service & Logistics", "20842", 2 },
                    { 239, 20844, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7795), "Igor Lazevski", "Production", "Coating Unit", "20844", 2 },
                    { 240, 20847, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7796), "Dragan Dragutinovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20847", 2 },
                    { 241, 20848, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7798), "Hristo Jovanovski", "Production", "Rolling Unit", "20848", 2 },
                    { 242, 20851, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7799), "Goran Moskov", "Production", "Coating Unit", "20851", 2 },
                    { 243, 20852, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7800), "Zoran Nikolovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20852", 2 },
                    { 244, 20855, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7801), "Goran Cvetanovski", "Production", "Rolling Unit", "20855", 2 },
                    { 245, 20866, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7804), "Pero Mangarov", "Supply chain", "Customer service & Logistics", "20866", 2 },
                    { 246, 20871, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7805), "Igor Blazevski", "Production", "Coating Unit", "20871", 2 },
                    { 247, 20872, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7807), "Spase Belinski", "Projects and IT", "Crane Maintenance & Internal Transport", "20872", 2 },
                    { 248, 20876, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7824), "Zhivorad Arsenovski", "Production", "Rolling Unit", "20876", 2 },
                    { 249, 20879, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7826), "Ljupcho Pijakovski", "Production", "Coating Unit", "20879", 2 },
                    { 250, 20883, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7827), "Ljupcho Dimitrijeski", "Projects and IT", "Crane Maintenance & Internal Transport", "20883", 2 },
                    { 251, 20889, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7829), "Dushan Jovanoski", "Sales", "Sales", "20889", 2 },
                    { 252, 20893, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7830), "Nikola Nikolovski", "Sales", "Sales", "20893", 2 },
                    { 253, 20894, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7831), "Dimitar Jankovski", "Production", "Rolling Unit", "20894", 2 },
                    { 254, 20896, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7832), "Imer Ljusjani", "Projects and IT", "Crane Maintenance & Internal Transport", "20896", 2 },
                    { 255, 20898, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7834), "Bobi Gjogjievski", "Projects and IT", "Crane Maintenance & Internal Transport", "20898", 2 },
                    { 256, 20899, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7836), "Hristijan Gjorgjevski", "Production", "Rolling Unit", "20899", 2 },
                    { 257, 20903, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7837), "Temelko Sarovski", "Production", "Quality control", "20903", 2 },
                    { 258, 20910, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7838), "Hristijan Simonovski", "Supply chain", "Rolling Unit", "20910", 2 },
                    { 259, 20911, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7839), "Dame Kekenovski", "Production", "Coating Unit", "20911", 2 },
                    { 260, 20914, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7841), "Afrim Jusufi", "Production", "Planning & Strategy", "20914", 2 },
                    { 261, 20915, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7844), "Igor Damjanovski", "Supply chain", "Crane Maintenance & Internal Transport", "20915", 2 },
                    { 262, 20916, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7846), "Besnik Ibraimi", "Projects and IT", "Quality control", "20916", 2 },
                    { 263, 20917, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7847), "Viktor Velichkovski", "Production", "Coating Unit", "20917", 2 },
                    { 264, 20919, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7850), "Robert Jovanovski", "Projects and IT", "Maintenance Progress", "20919", 2 },
                    { 265, 20920, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7851), "Adnan Feratovski", "Projects and IT", "Coating Unit", "20920", 2 },
                    { 266, 20924, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7852), "Biljana Chorobenska", "Supply chain", "Crane Maintenance & Internal Transport", "20924", 2 },
                    { 267, 20927, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7854), "Vladan Trajkovski", "Projects and IT", "Rolling Unit", "20927", 2 },
                    { 268, 20928, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7855), "Vlatko Mitevski", "Production", "Crane Maintenance & Internal Transport", "20928", 2 },
                    { 269, 20935, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7856), "Adis Nezirovski", "Projects and IT", "Coating Unit", "20935", 2 },
                    { 270, 20936, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7857), "Asim Nezirovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20936", 2 },
                    { 271, 20937, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7858), "Goce Spaseski", "Production", "Rolling Unit", "20937", 2 },
                    { 272, 20942, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7860), "Dragi Ickovski", "Projects and IT", "Crane Maintenance & Internal Transport", "20942", 2 },
                    { 273, 20944, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7862), "Ibrahim Mujovikj", "Production", "Rolling Unit", "20944", 2 },
                    { 274, 20948, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7863), "Boban Grozdanovski", "Projects and IT", "Quality control", "20948", 2 },
                    { 275, 20951, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7864), "Robert Stojanovikj", "Projects and IT", "Rolling Unit", "20951", 2 },
                    { 276, 20953, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7865), "Mihajlo Zafirovikj", "Production", "Coating Unit", "20953", 2 },
                    { 277, 20955, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7866), "Aleksandra Trgachevska", "Supply chain", "Central mechanical maintenance", "20955", 2 },
                    { 278, 20958, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7868), "Marjanche Ristovski", "Production", "Customer service & Logistics", "20958", 2 },
                    { 279, 20963, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7869), "Dalibor Cvetkovski", "Production", "Automation", "20963", 2 },
                    { 280, 20964, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7873), "Ivica Stanoeski", "Projects and IT", "Rolling Unit", "20964", 2 },
                    { 281, 20967, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7874), "Gjorgji Velichkovski", "Supply chain", "Customer service & Logistics", "20967", 2 },
                    { 282, 20968, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7875), "Karanfilka Giceva", "Supply chain", "Rolling Unit", "20968", 2 },
                    { 283, 20971, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7877), "Djevat Feratovski", "Production", "Planning & Strategy", "20971", 2 },
                    { 284, 20973, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7878), "Ivan Mitodevski", "Production", "Coating Unit", "20973", 2 },
                    { 285, 20975, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7879), "Robert Ristovski", "Projects and IT", "Maintenance Progress", "20975", 1 },
                    { 286, 20977, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7880), "Vlatko Dimevski", "Supply chain", "Coating Unit", "20977", 2 },
                    { 287, 20979, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7881), "Violeta Vidinska", "HR", "Coating Unit", "20979", 2 },
                    { 288, 20981, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7882), "Aco Jovanovski", "Projects and IT", "Coating Unit", "20981", 2 },
                    { 289, 20982, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7884), "Rade Panovski", "Production", "Coating Unit", "20982", 2 },
                    { 290, 20983, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7886), "Slave Joshovikj", "Production", "Coating Unit", "20983", 2 },
                    { 291, 20988, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7887), "Nenad Petkovikj", "Projects and IT", "Maintenance Progress", "20988", 2 },
                    { 292, 20989, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7888), "Borche Livrinski", "Projects and IT", "Coating Unit", "20989", 2 },
                    { 293, 20994, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7889), "Sanja Lambrinidis", "Supply chain", "Rolling Unit", "20994", 2 },
                    { 294, 20998, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7890), "Ace Jovanovski", "Production", "Rolling Unit", "20998", 2 },
                    { 295, 20999, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7891), "Sashe Smilkovski", "Projects and IT", "Rolling Unit", "20999", 2 },
                    { 296, 21002, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7893), "Leon Danilovski", "Supply chain", "Automation", "21002", 2 },
                    { 297, 21003, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7894), "Enis Zekjiri", "Projects and IT", "Coating Unit", "21003", 2 },
                    { 298, 21006, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7895), "Metodija Malkov", "Production", "Rolling Unit", "21006", 2 },
                    { 299, 21010, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7896), "Stefan Risteski", "Projects and IT", "Customer service & Logistics", "21010", 2 },
                    { 300, 21012, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7930), "Igor Momchilovski", "Production", "Rolling Unit", "21012", 2 },
                    { 301, 21016, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7932), "Oliver Govedarovski", "Projects and IT", "Planning & Strategy", "21016", 2 },
                    { 302, 21017, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7933), "Bobi Nikolovski", "Projects and IT", "Coating Unit", "21017", 2 },
                    { 303, 21020, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7934), "Kristijan Stojanovski", "Supply chain", "Maintenance Progress", "21020", 2 },
                    { 304, 21082, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7935), "Rufat Rufati", "Production", "Coating Unit", "21082", 2 },
                    { 305, 21090, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7937), "Vase Pecevski", "Projects and IT", "Coating Unit", "21090", 2 },
                    { 306, 21094, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7939), "Kristina Karajanovska", "Sales", "Coating Unit", "21094", 2 },
                    { 307, 21095, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7940), "Srechko Vidinski", "Production", "Coating Unit", "21095", 2 },
                    { 308, 21096, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7941), "Nikola Spasevski", "Projects and IT", "Crane Maintenance & Internal Transport", "21096", 2 },
                    { 309, 21097, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7942), "Zvonko Miloshoski", "Supply chain", "Quality control", "21097", 2 },
                    { 310, 21100, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7944), "Ismail Redzepi", "Production", "Rolling Unit", "21100", 2 },
                    { 311, 21104, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7945), "Aleksandar Kekenovski", "Production", "Health, Safety and Environment", "21104", 2 },
                    { 312, 21112, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7946), "Nikola Nikolovski", "Production", "Financial Controlling and Reporting", "21112", 2 },
                    { 313, 21117, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7947), "Jovica Stojanovikj", "Production", "Coating Unit", "21117", 2 },
                    { 314, 21119, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7949), "Vasil Kocevski", "Production", "Rolling Unit", "21119", 2 },
                    { 315, 21121, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7950), "Petre Kushinovski", "Production", "Coating Unit", "21121", 2 },
                    { 316, 21125, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7951), "Mitko Lebamov", "Projects and IT", "Crane Maintenance & Internal Transport", "21125", 2 },
                    { 317, 21126, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7952), "Aleksandar Boshkovski", "Production", "Coating Unit", "21126", 2 },
                    { 318, 21128, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7953), "Gjuner Ismailovski", "Projects and IT", "Rolling Unit", "21128", 2 },
                    { 319, 21131, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7955), "Jovan Markovski", "Production", "Coating Unit", "21131", 2 },
                    { 320, 21133, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7956), "Kjamuran Muaremovski", "Production", "Coating Unit", "21133", 2 },
                    { 321, 21134, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7957), "Nikola Panovski", "Projects and IT", "Accounting & Treasury", "21134", 2 },
                    { 322, 21136, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7958), "Stefan Ristovski", "Production", "Coating Unit", "21136", 2 },
                    { 323, 21139, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7959), "Jasin Ismailovski", "Production", "Facility", "21139", 2 },
                    { 324, 21140, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7961), "Borko Sokolovikj", "Production", "Customer service & Logistics", "21140", 2 },
                    { 325, 21142, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7962), "Stojan Despotoski", "Production", "Coating Unit", "21142", 2 },
                    { 326, 21143, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7964), "Shezair Lazam", "Production", "High voltage", "21143", 2 },
                    { 327, 21149, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7965), "Jovan Stojanovski", "Projects and IT", "Crane Maintenance & Internal Transport", "21149", 2 },
                    { 328, 21151, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7966), "Kire Krusharski", "Production", "Stores", "21151", 2 },
                    { 329, 21152, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7967), "Igorche Kuzmanov", "Production", "Customer service & Logistics", "21152", 2 },
                    { 330, 21154, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7968), "Goce Zdravevski", "Supply chain", "Crane Maintenance & Internal Transport", "21154", 2 },
                    { 331, 21156, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7970), "Goran Vasilevski", "Production", "Crane Maintenance & Internal Transport", "21156", 2 },
                    { 332, 21160, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7971), "Deni Popovski", "Supply chain", "Coating Unit", "21160", 2 },
                    { 333, 21171, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7972), "Jovan Chankulovski", "Production", "High voltage", "21171", 2 },
                    { 334, 21174, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7973), "Dragi Risteski", "Projects and IT", "Quality control", "21174", 2 },
                    { 335, 21175, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7974), "Zoran Urdarevikj", "Production", "Rolling Unit", "21175", 2 },
                    { 336, 21178, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7976), "Miroslav Martinovski", "Production", "Quality control", "21178", 2 },
                    { 337, 21183, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7977), "Emran Iseinov", "Production", "Human Resources", "21183", 2 },
                    { 338, 21184, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7978), "Mirche Milkovski", "Production", "Crane Maintenance & Internal Transport", "21184", 2 },
                    { 339, 21188, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7979), "Aleksandar Kitanovski", "Production", "Rolling Unit", "21188", 2 },
                    { 340, 21189, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7980), "Dejan Stefanovski", "Production", "Coating Unit", "21189", 2 },
                    { 341, 21190, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7981), "Viktor Stojchevski", "Production", "Rolling Unit", "21190", 2 },
                    { 342, 21191, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7983), "Dragan Risteski", "Supply chain", "Crane Maintenance & Internal Transport", "21191", 2 },
                    { 343, 21193, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7984), "Dzemail Ljimani", "Production", "Coating Unit", "21193", 2 },
                    { 344, 21194, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7985), "Biljana Trajkovska", "Supply chain", "Accounting & Treasury", "21194", 2 },
                    { 345, 21196, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7986), "Miroslav Krstikj", "Production", "Planning & Strategy", "21196", 2 },
                    { 346, 21197, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7987), "Violeta Stojanovska", "CEO office", "Facility", "21197", 2 },
                    { 347, 21198, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7989), "Kristina Kolaroska", "Finance Department", "Crane Maintenance & Internal Transport", "21198", 2 },
                    { 348, 21200, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7990), "David Savevski", "Production", "Automation", "21200", 2 },
                    { 349, 21201, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7991), "Emrah Sali", "Production", "Coating Unit", "21201", 2 },
                    { 350, 21204, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7992), "Robert Ristovski", "Production", "Accounting & Treasury", "21204", 2 },
                    { 351, 21206, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7993), "Marjanche Milkovski", "Projects and IT", "Coating Unit", "21206", 2 },
                    { 352, 21209, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7994), "Ice Trajkoski", "Production", "Facility", "21209", 2 },
                    { 353, 21212, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(7996), "Viktor Ilievski", "Production", "Customer service & Logistics", "21212", 2 },
                    { 354, 21218, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8034), "Daniel Slavkovski", "Production", "Coating Unit", "21218", 2 },
                    { 355, 21219, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8036), "Goce Peshevski", "Production", "High voltage", "21219", 2 },
                    { 356, 21224, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8037), "Natasha Mihova", "Finance Department", "Crane Maintenance & Internal Transport", "21224", 2 },
                    { 357, 21225, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8039), "Bujar Zenuli", "Production", "Stores", "21225", 2 },
                    { 358, 21227, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8040), "Tamara Stojchevska", "HR", "Coating Unit", "21227", 2 },
                    { 359, 21229, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8041), "Dragana Velkovikj-Krsteva", "Supply chain", "Customer service & Logistics", "21229", 2 },
                    { 360, 21231, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8042), "Jovica Stojanovski", "Production", "Crane Maintenance & Internal Transport", "21231", 2 },
                    { 361, 21233, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8044), "Mario Trajkovski", "Projects and IT", "Crane Maintenance & Internal Transport", "21233", 2 },
                    { 362, 21240, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8045), "Dancho Kostadinovski", "Projects and IT", "Coating Unit", "21240", 2 },
                    { 363, 21241, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8046), "Konstantin Koneski", "Supply chain", "High voltage", "21241", 2 },
                    { 364, 21243, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8047), "Nenad Mihajloski", "Production", "Quality control", "21243", 2 },
                    { 365, 21247, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8048), "Ilija Andonoski", "Supply chain", "Rolling Unit", "21247", 2 },
                    { 366, 21252, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8049), "Toni Karovchevikj", "Projects and IT", "Quality control", "21252", 2 },
                    { 367, 21254, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8051), "Hristijan Todorovski", "Projects and IT", "Coating Unit", "21254", 2 },
                    { 368, 21257, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8052), "Atanas Boshkov", "Production", "Coating Unit", "21257", 2 },
                    { 369, 21259, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8053), "Damjan Petrovski", "Projects and IT", "Crane Maintenance & Internal Transport", "21259", 2 },
                    { 370, 21260, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8054), "Viktorija Karafiloska", "Supply chain", "Crane Maintenance & Internal Transport", "21260", 2 },
                    { 371, 21261, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8055), "Sashko Janevski", "Production", "Coating Unit", "21261", 2 },
                    { 372, 21262, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8056), "Maja Miloshoska", "Supply chain", "Crane Maintenance & Internal Transport", "21262", 2 },
                    { 373, 21263, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8059), "Elena Stoilkovska", "HR", "Coating Unit", "21263", 2 },
                    { 374, 21268, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8060), "Dragan Najdovski", "Projects and IT", "Crane Maintenance & Internal Transport", "21268", 2 },
                    { 375, 21269, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8061), "Luka Bostandzievski", "Production", "Coating Unit", "21269", 2 },
                    { 376, 21270, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8062), "Sinisha Voinoski", "Production", "Crane Maintenance & Internal Transport", "21270", 2 },
                    { 377, 21271, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8064), "Muhamed Mimin", "Production", "Coating Unit", "21271", 2 },
                    { 378, 21274, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8065), "Nuija Nuijovski", "Projects and IT", "Facility", "21274", 2 },
                    { 379, 21275, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8066), "Svetlana Davkovska", "Finance Department", "Facility", "21275", 2 },
                    { 380, 21277, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8067), "Isa Zenelji", "Production", "Coating Unit", "21277", 2 },
                    { 381, 21280, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8068), "Mario Nikolovski", "Projects and IT", "Quality control", "21280", 2 },
                    { 382, 21281, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8069), "Angel Kostovski", "Production", "Crane Maintenance & Internal Transport", "21281", 2 },
                    { 383, 21282, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8071), "Hristijan Stevkovski", "Supply chain", "Coating Unit", "21282", 2 },
                    { 384, 21283, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8072), "Naim Ajvazi", "Production", "Crane Maintenance & Internal Transport", "21283", 2 },
                    { 385, 21284, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8073), "Miodrag Achkovikj", "Production", "Coating Unit", "21284", 2 },
                    { 386, 21285, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8074), "Andrej Velichkovski", "Projects and IT", "Crane Maintenance & Internal Transport", "21285", 2 },
                    { 387, 21286, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8075), "Dejan Smilevski", "Projects and IT", "Coating Unit", "21286", 2 },
                    { 388, 21288, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8076), "Trajche Trajkovski", "Production", "Crane Maintenance & Internal Transport", "21288", 2 },
                    { 389, 21290, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8077), "Sashko Dimovski", "Projects and IT", "Coating Unit", "21290", 2 },
                    { 390, 21292, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8079), "Dushan Manojlovikj", "Production", "Quality control", "21292", 2 },
                    { 391, 21293, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8081), "Zoran Ilieski", "Projects and IT", "Coating Unit", "21293", 2 },
                    { 392, 21294, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8082), "Antonio Panovski", "Production", "Crane Maintenance & Internal Transport", "21294", 2 },
                    { 393, 21295, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8083), "Violeta Joshovikj", "HR", "Human Resources", "21295", 2 },
                    { 394, 21297, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8084), "Sashka Stojanovska", "HR", "Crane Maintenance & Internal Transport", "21297", 2 },
                    { 395, 21298, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8086), "Ljupcho Emsherijov", "Production", "Rolling Unit", "21298", 2 },
                    { 396, 21299, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8087), "Nikola Risteski", "Supply chain", "Coating Unit", "21299", 2 },
                    { 397, 21300, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8088), "Ljupcho Bogojev", "Projects and IT", "Crane Maintenance & Internal Transport", "21300", 2 },
                    { 398, 21302, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8089), "Erol Idriz", "Projects and IT", "Coating Unit", "21302", 2 },
                    { 399, 21303, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8090), "Blagoja Jovchevski", "Projects and IT", "Crane Maintenance & Internal Transport", "21303", 2 },
                    { 400, 21304, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8091), "Stefan Trajkovikj", "Production", "Coating Unit", "21304", 2 },
                    { 401, 21305, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8092), "Vesna Gjorgjevska", "HR", "Facility", "21305", 2 },
                    { 402, 21306, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8094), "Mihaela Gecheva", "HR", "Facility", "21306", 2 },
                    { 403, 21307, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8095), "Marija Malinova", "Supply chain", "Planning & Strategy", "21307", 2 },
                    { 404, 21308, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8096), "Viktorija Siljanoska", "Projects and IT", "Automation", "21308", 2 },
                    { 405, 21309, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8097), "Aleksandar Paunkovikj", "Projects and IT", "Crane Maintenance & Internal Transport", "21309", 2 },
                    { 406, 21310, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8098), "Stefan Cvetanovski", "Production", "Coating Unit", "21310", 2 },
                    { 407, 21311, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8099), "Valentina Cibreva", "Finance Department", "Accounting & Treasury", "21311", 2 },
                    { 408, 21312, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8127), "Milancho Uroshevski", "Supply chain", "Planning & Strategy", "21312", 2 },
                    { 409, 21313, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8128), "Jashar Ismaili", "HR", "Facility", "21313", 2 },
                    { 410, 21314, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8129), "Daniel Neshkovikj", "Daniel", "Crane Maintenance & Internal Transport", "21314", 2 },
                    { 411, 21315, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8130), "Hristina Jovanovska", "Projects and IT", "Automation", "21315", 2 },
                    { 412, 21316, new DateTime(2025, 10, 11, 19, 45, 26, 390, DateTimeKind.Utc).AddTicks(8132), "Marjan Georgiev", "Production", "Coating Unit", "21316", 2 }
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
                    { 25, true, 9, 1, "Задоволен сум од мојот пакет компензации и бенефиции", 1 },
                    { 26, true, 9, 1, "Моите напори и достигнувања се препознаени и ценети", 1 },
                    { 27, true, 9, 1, "Постојат јасни можности за напредување во кариерата во рамките на компанијата", 1 },
                    { 28, true, 10, 1, "Компанијата ги поттикнува иновациите и креативното размислување", 1 },
                    { 29, true, 10, 1, "Подготвен сум да ги усвојам промените имплементирани во компанијата", 1 },
                    { 30, true, 10, 1, "Идеите и предлозите на вработените се разгледуваат и се спроведуваат кога е соодветно", 1 },
                    { 31, true, 11, 1, "Kолку е веројатно да ја препорачате оваа компанија како работно место на пријател или колега", 1 },
                    { 32, true, 11, 2, "Како ја гледате вашата иднина во оваа компанија во следните 2-3 години?", 1 },
                    { 33, true, 11, 2, "разно", 1 },
                    { 34, true, 12, 2, "Што најмногу ви се допаѓа на вашето сегашно работно место?", 1 },
                    { 35, true, 12, 2, "Кои се најголемите предизвици со кои се соочувате на работа?", 1 },
                    { 36, true, 12, 2, "Какви предлози имате за подобрување на работната средина или процесите на компанијата?", 1 }
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
                name: "IX_Answers_UserId_QuestionId_QuestionFormId",
                table: "Answers",
                columns: new[] { "UserId", "QuestionId", "QuestionFormId" },
                unique: true);

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
