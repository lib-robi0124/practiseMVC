Company needs MVC app as Questionnaire for employee satisfactory on different area of workplace. Idea is , Domain.Models to be:
User , Role , Question, QuestionType, QuestionForm, Answer
User are Administrators to be able to add, modify(update), delete Questions as per Roles, Users as Employees are Answer the QuestionForm with Question as Type of Scale from 1 to 10, or fill with Text. Answer has to be stored in database for future reporting, with UserId, QuestionFormId, QuestionId.
Main page will have login screen with CopmanyId and FullName of User
After successfully login, enter to QuestionForm with 3 Questions , like:
QuestionForm Title "Општо задоволство"
QuestionId 1 "Задоволен сум од мојата моментална работа"
Answer is int from Scale 1 2 3 4 5 6 7 8 9 10
QuestionId 2 "Чувствувам дека мојата работа е ценета во рамките на компанијата." 
Answer is int from Scale 1 2 3 4 5 6 7 8 9 10
QuestionId 3 "Се чувствувам мотивиран да одам на работа секој ден."
Answer is int from Scale 1 2 3 4 5 6 7 8 9 10
QuestionForm Title "Отворени прашања"
QuestionId 1 "Што најмногу ви се допаѓа на вашето сегашно работно место? "
Answer is string Text (500)
QuestionId 2 "Кои се најголемите предизвици со кои се соочувате на работа?" 
Answer is string Text (500)
QuestionId 3 "Какви предлози имате за подобрување на работната средина или процесите на компанијата?" 
Answer is string Text (500)
Client Browser → MVC Controllers → Services Layer → DataAccess Layer → Database
     ↓              ↓                  ↓               ↓               ↓
   Views       ViewModels        Business Logic     EF Context     SQL Server
     ↓              ↓                  ↓               ↓               ↓
   HTML         Data Transfer    Validation &      Repository      Tables:
   Forms         Objects         Processing        Pattern        Users, Roles, 
                                                               Questions, Forms, 
                                                               Answers
📋 ARCHITECTURE DIAGRAM

Client Browser 
     ↓
MVC Controllers (UserController, FormController, AnswerController)
     ↓
ViewModels (Data Transfer Objects with Validation)
     ↓
Services Layer (Business Logic & Validation)
     ↓
Repository Pattern (Data Access Abstraction)
     ↓
Entity Framework Context
     ↓
SQL Server Database
✅
Good Separation of Concerns - Clear layers (Domain, DataAccess, Services)

Repository Pattern - Proper abstraction of data access

Dependency Injection - Well implemented with extension methods

Comprehensive Seed Data - Realistic user data with organizational units

Proper Async/Await - Good use of asynchronous programming
📊 Domain Models
namespace Anketa.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; } // Organizational Unit
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<Answer> Answers { get; set; }
    }
}
namespace Anketa.Domain.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
namespace Anketa.Domain.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; } // Admin user who created the question
        public User User { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
        public int QuestionFormId { get; set; }
        public QuestionForm QuestionForm { get; set; }
        public bool IsRequired { get; set; } = true;
        public ICollection<Answer> Answers { get; set; }
    }
}
namespace Anketa.Domain.Models
{
    public class QuestionType
    {
        public int Id { get; set; }
        public string Name { get; set; } // "Scale", "Text"
        public string Description { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
namespace Anketa.Domain.Models
{
    public class QuestionForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
namespace Anketa.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int QuestionFormId { get; set; }
        public QuestionForm QuestionForm { get; set; }
        public int? ScaleValue { get; set; }
        public string TextValue { get; set; }
        public DateTime AnsweredDate { get; set; } = DateTime.UtcNow;
    }
}
using Anketa.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Anketa.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=EmployeeSatisfactionSurvey;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<QuestionForm> QuestionForms { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.QuestionForm)
                .WithMany(f => f.Answers)
                .HasForeignKey(a => a.QuestionFormId);
        }
    }
}
using Anketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Anketa.DataAccess
{
    public static class DataSeedExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrator" },
                new Role { Id = 2, Name = "Employee" }
            );
            // Seed Question Types
            modelBuilder.Entity<QuestionType>().HasData(
                new QuestionType { Id = 1, Name = "Scale", Description = "1-10 Scale" },
                new QuestionType { Id = 2, Name = "Text", Description = "Text Answer" }
            );
            // Seed Question Forms with OU
            modelBuilder.Entity<QuestionForm>().HasData(
                new QuestionForm { Id = 1, Title = "Општо задоволство", Description = "Overall Satisfaction" },
                new QuestionForm { Id = 2, Title = "Обврска кон компанијата", Description = "Commitment to the Company" },
                new QuestionForm { Id = 3, Title = "Професионален развој", Description = "Professional Development" },
                new QuestionForm { Id = 4, Title = "Рамнотежа помеѓу работата и животот", Description = "Work-Life Balance" },
                new QuestionForm { Id = 5, Title = "Комуникација и соработка", Description = "Communication and Collaboration" },
                new QuestionForm { Id = 6, Title = "Лидерство", Description = "Leadership" },
                new QuestionForm { Id = 7, Title = "Организациска култура", Description = "Organizational Culture" },
                new QuestionForm { Id = 8, Title = "Работна средина", Description = "Work Environment" },
                new QuestionForm { Id = 9, Title = "Награди и признанија", Description = "Rewards and Recognition" },
                new QuestionForm { Id = 10, Title = "Иновации и промени", Description = "Innovation and Changes" },
                new QuestionForm { Id = 11, Title = "Конечна евалуација", Description = "Final Evaluation" },
                new QuestionForm { Id = 12, Title = "Отворени прашања", Description = "Open Questions" }
            );
            // Seed Questions (UserId = 1 for Admin user, No OU)
            modelBuilder.Entity<Question>().HasData(
                    // Form 1: Општо задоволство (Scale questions)
                    new Question { Id = 1, Text = "Задоволен сум од мојата моментална работа", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
                    new Question { Id = 2, Text = "Чувствувам дека мојата работа е ценета во рамките на компанијата", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
                    new Question { Id = 3, Text = "Се чувствувам мотивиран да одам на работа секој ден", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },

                    // Form 2: Обврска кон компанијата (Scale questions)
                    new Question { Id = 4, Text = "Се чувствувам горд што работам за оваа компанија", QuestionTypeId = 1, QuestionFormId = 2, UserId = 1 },
                    new Question { Id = 5, Text = "Со задоволство ја препорачувам оваа компанија како работно место на пријателите и семејството", QuestionTypeId = 1, QuestionFormId = 2, UserId = 1 },
                    new Question { Id = 6, Text = "Се гледам себеси како долгорочно работам во оваа компанија", QuestionTypeId = 1, QuestionFormId = 2, UserId = 1 },

                    // Form 3: Професионален развој (Scale questions)
                    new Question { Id = 7, Text = "Имам можности за професионален развој и напредување", QuestionTypeId = 1, QuestionFormId = 3, UserId = 1 },
                    new Question { Id = 8, Text = "Добивам конструктивна повратна информација за мојата работа", QuestionTypeId = 1, QuestionFormId = 3, UserId = 1 },
                    new Question { Id = 9, Text = "Компанијата обезбедува соодветна обука и ресурси за мојот развој", QuestionTypeId = 1, QuestionFormId = 3, UserId = 1 },

                    // Form 4: "Рамнотежа помеѓу работата и животот" (Scale questions)
                    new Question { Id = 10, Text = "Компанијата поддржува здрава рамнотежа помеѓу работата и личниот живот", QuestionTypeId = 1, QuestionFormId = 4, UserId = 1 },
                    new Question { Id = 11, Text = "Можам ефикасно да управувам со стресот поврзан со работата", QuestionTypeId = 1, QuestionFormId = 4, UserId = 1 },
                    new Question { Id = 12, Text = "Мојот работен распоред ми овозможува да ги исполнувам моите лични обврски", QuestionTypeId = 1, QuestionFormId = 4, UserId = 1 },

                    // Form 5: "Комуникација и соработка" (Scale questions)
                    new Question { Id = 13, Text = "Комуникацијата во мојот тим е ефикасна", QuestionTypeId = 1, QuestionFormId = 5, UserId = 1 },
                    new Question { Id = 14, Text = "Се чувствувам удобно да ги искажувам моите идеи и мислења на работа", QuestionTypeId = 1, QuestionFormId = 5, UserId = 1 },
                    new Question { Id = 15, Text = "Соработката помеѓу одделенијата е ефикасна", QuestionTypeId = 1, QuestionFormId = 5, UserId = 1 },

                    // Form 6: "Лидерство" (Scale questions)
                    new Question { Id = 16, Text = "Му верувам на раководството на компанијата", QuestionTypeId = 1, QuestionFormId = 6, UserId = 1 },
                    new Question { Id = 17, Text = "Мојот директен менаџер ме поддржува во остварувањето на моите цели", QuestionTypeId = 1, QuestionFormId = 6, UserId = 1 },
                    new Question { Id = 18, Text = "Важните одлуки на компанијата се пренесуваат транспарентно", QuestionTypeId = 1, QuestionFormId = 6, UserId = 1 },

                    // Form 7: "Организациска култура" (Scale questions)
                    new Question { Id = 19, Text = "Вредностите на компанијата се усогласуваат со моите лични вредности", QuestionTypeId = 1, QuestionFormId = 7, UserId = 1 },
                    new Question { Id = 20, Text = "Се чувствувам вклучено и почитувано на работа", QuestionTypeId = 1, QuestionFormId = 7, UserId = 1 },
                    new Question { Id = 21, Text = "Компанијата промовира различност и инклузија", QuestionTypeId = 1, QuestionFormId = 7, UserId = 1 },

                    // Form 8: "Работна средина" (Scale questions)
                    new Question { Id = 22, Text = "Ги имам сите ресурси потребни за ефикасно извршување на моите задачи", QuestionTypeId = 1, QuestionFormId = 8, UserId = 1 },
                    new Question { Id = 23, Text = "Физичката работна средина е удобна и поволна за продуктивност", QuestionTypeId = 1, QuestionFormId = 8, UserId = 1 },
                    new Question { Id = 24, Text = "Се чувствувам безбедно на работа", QuestionTypeId = 1, QuestionFormId = 8, UserId = 1 },

                    // Form 9: "Награди и признанија" (Scale questions)
                    new Question { Id = 24, Text = "Задоволен сум од мојот пакет компензации и бенефиции", QuestionTypeId = 1, QuestionFormId = 9, UserId = 1 },
                    new Question { Id = 25, Text = "Моите напори и достигнувања се препознаени и ценети", QuestionTypeId = 1, QuestionFormId = 9, UserId = 1 },
                    new Question { Id = 26, Text = "Постојат јасни можности за напредување во кариерата во рамките на компанијата", QuestionTypeId = 1, QuestionFormId = 9, UserId = 1 },

                    // Form 10: "Иновации и промени" (Scale questions)
                    new Question { Id = 27, Text = "Компанијата ги поттикнува иновациите и креативното размислување", QuestionTypeId = 1, QuestionFormId = 10, UserId = 1 },
                    new Question { Id = 28, Text = "Подготвен сум да ги усвојам промените имплементирани во компанијата", QuestionTypeId = 1, QuestionFormId = 10, UserId = 1 },
                    new Question { Id = 29, Text = "Идеите и предлозите на вработените се разгледуваат и се спроведуваат кога е соодветно", QuestionTypeId = 1, QuestionFormId = 10, UserId = 1 },

                    // Form 11: "Конечна евалуација" (Scale and text questions)
                    new Question { Id = 30, Text = "Kолку е веројатно да ја препорачате оваа компанија како работно место на пријател или колега", QuestionTypeId = 1, QuestionFormId = 11, UserId = 1 },
                    new Question { Id = 31, Text = "Како ја гледате вашата иднина во оваа компанија во следните 2-3 години?", QuestionTypeId = 2, QuestionFormId = 11, UserId = 1 },
                    new Question { Id = 32, Text = "разно", QuestionTypeId = 2, QuestionFormId = 11, UserId = 1 },

                    // Form 12: Отворени прашања (Text questions)
                    new Question { Id = 33, Text = "Што најмногу ви се допаѓа на вашето сегашно работно место?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 },
                    new Question { Id = 34, Text = "Кои се најголемите предизвици со кои се соочувате на работа?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 },
                    new Question { Id = 35, Text = "Какви предлози имате за подобрување на работната средина или процесите на компанијата?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 }
                );
            // Seed Users with OU
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, CompanyId = 16130, FullName = "Vasko Gjorgiev", OU = "Production", Password = "16130", RoleId = 2 },
                new User { Id = 2, CompanyId = 16684, FullName = "Zoran Stojanovski", OU = "Production", Password = "16684", RoleId = 2 },
                new User { Id = 3, CompanyId = 16695, FullName = "Pane Naskovski", OU = "Production", Password = "16695", RoleId = 2 },
                new User { Id = 4, CompanyId = 16720, FullName = "Tome Trajanov", OU = "Projects and IT", Password = "16720", RoleId = 2 },
                new User { Id = 5, CompanyId = 16827, FullName = "Zoran Boshkovski", OU = "Production", Password = "16827", RoleId = 2 },
                new User { Id = 6, CompanyId = 16984, FullName = "Dide Petrovski", OU = "Projects and IT", Password = "16984", RoleId = 2 },
                new User { Id = 7, CompanyId = 17011, FullName = "Jovica Gjorgjievski", OU = "Projects and IT", Password = "17011", RoleId = 2 },
                new User { Id = 8, CompanyId = 17055, FullName = "Blagica Besarovska", OU = "Projects and IT", Password = "17055", RoleId = 2 },
                new User { Id = 9, CompanyId = 17064, FullName = "Dragi Naskovski", OU = "Production", Password = "17064", RoleId = 2 },
                new User { Id = 10, CompanyId = 17148, FullName = "Borche Anchevski", OU = "Production", Password = "17148", RoleId = 2 },
                new User { Id = 11, CompanyId = 17772, FullName = "Toni Nacev", OU = "HR", Password = "17772", RoleId = 2 },
                new User { Id = 12, CompanyId = 17884, FullName = "Valentina Kostovska", OU = "HR", Password = "17884", RoleId = 2 },
                new User { Id = 13, CompanyId = 17893, FullName = "Zoran Tripunoski", OU = "Production", Password = "17893", RoleId = 2 },
                new User { Id = 14, CompanyId = 17896, FullName = "Zorancho Taseski", OU = "Projects and IT", Password = "17896", RoleId = 2 },
                new User { Id = 15, CompanyId = 18158, FullName = "Goran Despodovski", OU = "Production", Password = "18158", RoleId = 2 },
                new User { Id = 16, CompanyId = 18162, FullName = "Ljupcho Krstevski", OU = "Production", Password = "18162", RoleId = 2 },
                new User { Id = 17, CompanyId = 18392, FullName = "Sabedin Ljura", OU = "Production", Password = "18392", RoleId = 2 },
                new User { Id = 18, CompanyId = 18412, FullName = "Rade Milenkovski", OU = "Production", Password = "18412", RoleId = 2 },
                new User { Id = 19, CompanyId = 18471, FullName = "Stojka Koneska", OU = "Supply chain", Password = "18471", RoleId = 2 },
                new User { Id = 20, CompanyId = 18529, FullName = "Zharko Nikolovski", OU = "Production", Password = "18529", RoleId = 2 },
                new User { Id = 21, CompanyId = 18533, FullName = "Radica Angelovska", OU = "Projects and IT", Password = "18533", RoleId = 2 },
                new User { Id = 22, CompanyId = 18874, FullName = "Borche Trifunovski", OU = "Production", Password = "18874", RoleId = 2 },
                new User { Id = 23, CompanyId = 18876, FullName = "Pero Stojanovski", OU = "Production", Password = "18876", RoleId = 2 },
                new User { Id = 24, CompanyId = 19370, FullName = "Dragi Petrovski", OU = "Production", Password = "19370", RoleId = 2 },
                new User { Id = 25, CompanyId = 19379, FullName = "Ilo Risteski", OU = "Supply chain", Password = "19379", RoleId = 2 },
                new User { Id = 26, CompanyId = 19767, FullName = "Aleksandar Iliev", OU = "Production", Password = "19767", RoleId = 2 },
                new User { Id = 27, CompanyId = 19775, FullName = "Mile Popovski", OU = "Production", Password = "19775", RoleId = 2 },
                new User { Id = 28, CompanyId = 19776, FullName = "Dragan Hristovski", OU = "Production", Password = "19776", RoleId = 2 },
                new User { Id = 29, CompanyId = 19777, FullName = "Aleksandar Jovchevski", OU = "Production", Password = "19777", RoleId = 2 },
                new User { Id = 30, CompanyId = 19779, FullName = "Ljupcho Andovski", OU = "Supply chain", Password = "19779", RoleId = 2 },
                new User { Id = 31, CompanyId = 19782, FullName = "Ivica Stanchevski", OU = "Production", Password = "19782", RoleId = 2 },
                new User { Id = 32, CompanyId = 19784, FullName = "Biljana Ilievska", OU = "Supply chain", Password = "19784", RoleId = 2 },
                new User { Id = 33, CompanyId = 19787, FullName = "Goran Damjanoski", OU = "Supply chain", Password = "19787", RoleId = 2 },
                new User { Id = 34, CompanyId = 19788, FullName = "Boban Neshovski", OU = "Production", Password = "19788", RoleId = 2 },
                new User { Id = 35, CompanyId = 19795, FullName = "Sashe Taparchevski", OU = "Production", Password = "19795", RoleId = 2 },
                new User { Id = 36, CompanyId = 19796, FullName = "Igor Ristovski", OU = "Production", Password = "19796", RoleId = 2 },
                new User { Id = 37, CompanyId = 19798, FullName = "Ivica Trajkovski", OU = "Production", Password = "19798", RoleId = 2 },
                new User { Id = 38, CompanyId = 19801, FullName = "Vlado Stojanovski", OU = "Production", Password = "19801", RoleId = 2 },
                new User { Id = 39, CompanyId = 19804, FullName = "Goran Spirovski", OU = "Production", Password = "19804", RoleId = 2 },
                new User { Id = 40, CompanyId = 19806, FullName = "Dejan Velkovski", OU = "Production", Password = "19806", RoleId = 2 },
                new User { Id = 41, CompanyId = 19807, FullName = "Stojanche Stefkovski", OU = "Production", Password = "19807", RoleId = 2 },
                new User { Id = 42, CompanyId = 19811, FullName = "Dancho Blazheski", OU = "Production", Password = "19811", RoleId = 2 },
                new User { Id = 43, CompanyId = 19813, FullName = "Ljupcho Lozanovski", OU = "Production", Password = "19813", RoleId = 2 },
                new User { Id = 44, CompanyId = 19818, FullName = "Marjan Nedelkovski", OU = "Projects and IT", Password = "19818", RoleId = 2 },
                new User { Id = 45, CompanyId = 19820, FullName = "Srgjan Stanojevikj", OU = "Production", Password = "19820", RoleId = 2 },
                new User { Id = 46, CompanyId = 19822, FullName = "Dragan Spasevski", OU = "Supply chain", Password = "19822", RoleId = 2 },
                new User { Id = 47, CompanyId = 19823, FullName = "Goran Andonovski", OU = "Projects and IT", Password = "19823", RoleId = 2 },
                new User { Id = 48, CompanyId = 19827, FullName = "Goran Anchovski", OU = "Supply chain", Password = "19827", RoleId = 2 },
                new User { Id = 49, CompanyId = 19833, FullName = "Igor Mircheski", OU = "Supply chain", Password = "19833", RoleId = 2 },
                new User { Id = 50, CompanyId = 19834, FullName = "Goran Nikolovski", OU = "HR", Password = "19834", RoleId = 2 },
                new User { Id = 51, CompanyId = 19838, FullName = "Petar Moskov", OU = "Production", Password = "19838", RoleId = 2 },
                new User { Id = 52, CompanyId = 19840, FullName = "Goran Stojanovski", OU = "Supply chain", Password = "19840", RoleId = 2 },
                new User { Id = 53, CompanyId = 19841, FullName = "Igor Petkovski", OU = "Projects and IT", Password = "19841", RoleId = 2 },
                new User { Id = 54, CompanyId = 19842, FullName = "Nenad Mitrovikj", OU = "Production", Password = "19842", RoleId = 2 },
                new User { Id = 55, CompanyId = 19844, FullName = "Sashko Gjorgjievski", OU = "Supply chain", Password = "19844", RoleId = 2 },
                new User { Id = 56, CompanyId = 19845, FullName = "Nikola Toshevski", OU = "Production", Password = "19845", RoleId = 2 },
                new User { Id = 57, CompanyId = 19848, FullName = "Slobodan Velkovski", OU = "Production", Password = "19848", RoleId = 2 },
                new User { Id = 58, CompanyId = 19849, FullName = "Goce Jankulovski", OU = "Supply chain", Password = "19849", RoleId = 2 },
                new User { Id = 59, CompanyId = 19868, FullName = "Marjan Milovanovikj", OU = "Production", Password = "19868", RoleId = 2 },
                new User { Id = 60, CompanyId = 19877, FullName = "Goran Gavrilovski", OU = "Sales", Password = "19877", RoleId = 2 },
                new User { Id = 61, CompanyId = 19892, FullName = "Irfan Feratovski", OU = "Production", Password = "19892", RoleId = 2 },
                new User { Id = 62, CompanyId = 19899, FullName = "Igor Krpachovski", OU = "Projects and IT", Password = "19899", RoleId = 2 },
                new User { Id = 63, CompanyId = 19911, FullName = "Aleksandar Spasevski", OU = "CEO office", Password = "19911", RoleId = 2 },
                new User { Id = 64, CompanyId = 19916, FullName = "Nevaip Bardi", OU = "Production", Password = "19916", RoleId = 2 },
                new User { Id = 65, CompanyId = 19917, FullName = "Biljana Stoshikj", OU = "Supply chain", Password = "19917", RoleId = 2 },
                new User { Id = 66, CompanyId = 19933, FullName = "Svetlana Jovanova", OU = "Finance Department", Password = "19933", RoleId = 2 },
                new User { Id = 67, CompanyId = 19960, FullName = "Draganche Taleski", OU = "Production", Password = "19960", RoleId = 2 },
                new User { Id = 68, CompanyId = 19963, FullName = "Toni Naumovski", OU = "Production", Password = "19963", RoleId = 2 },
                new User { Id = 69, CompanyId = 19992, FullName = "Metodi Gievski", OU = "Projects and IT", Password = "19992", RoleId = 2 },
                new User { Id = 70, CompanyId = 19993, FullName = "Jovica Velkovski", OU = "Supply chain", Password = "19993", RoleId = 2 },
                new User { Id = 71, CompanyId = 19997, FullName = "Gordana Astardjieva", OU = "Projects and IT", Password = "19997", RoleId = 2 },
                new User { Id = 72, CompanyId = 20023, FullName = "Zharko Ivanovski", OU = "Production", Password = "20023", RoleId = 2 },
                new User { Id = 73, CompanyId = 20024, FullName = "Igorche Janev", OU = "Production", Password = "20024", RoleId = 2 },
                new User { Id = 74, CompanyId = 20033, FullName = "Nikola Panov", OU = "Supply chain", Password = "20033", RoleId = 2 },
                new User { Id = 75, CompanyId = 20034, FullName = "Sasho Mitkovski", OU = "Production", Password = "20034", RoleId = 2 },
                new User { Id = 76, CompanyId = 20038, FullName = "Goran Ilievski", OU = "Production", Password = "20038", RoleId = 2 },
                new User { Id = 77, CompanyId = 20041, FullName = "Kircho Merdjanovski", OU = "Projects and IT", Password = "20041", RoleId = 2 },
                new User { Id = 78, CompanyId = 20052, FullName = "Davor Zdravkovski", OU = "Supply chain", Password = "20052", RoleId = 2 },
                new User { Id = 79, CompanyId = 20072, FullName = "Gorancho Petkovski", OU = "Production", Password = "20072", RoleId = 2 },
                new User { Id = 80, CompanyId = 20076, FullName = "Sashko Cvetanovski", OU = "Production", Password = "20076", RoleId = 2 },
                new User { Id = 81, CompanyId = 20095, FullName = "Ilija Tashevski", OU = "Production", Password = "20095", RoleId = 2 },
                new User { Id = 82, CompanyId = 20117, FullName = "Kire Stefanoski", OU = "Production", Password = "20117", RoleId = 2 },
                new User { Id = 83, CompanyId = 20125, FullName = "Aleksandar Evremov", OU = "Supply chain", Password = "20125", RoleId = 2 },
                new User { Id = 84, CompanyId = 20127, FullName = "Ratko Trajkovski", OU = "Supply chain", Password = "20127", RoleId = 2 },
                new User { Id = 85, CompanyId = 20128, FullName = "Goran Miovski", OU = "Projects and IT", Password = "20128", RoleId = 2 },
                new User { Id = 86, CompanyId = 20131, FullName = "Goran Trajkovski", OU = "Production", Password = "20131", RoleId = 2 },
                new User { Id = 87, CompanyId = 20137, FullName = "Gordana Shegmanovikj", OU = "HR", Password = "20137", RoleId = 2 },
                new User { Id = 88, CompanyId = 20144, FullName = "Igorche Bogdanovski", OU = "Production", Password = "20144", RoleId = 2 },
                new User { Id = 89, CompanyId = 20152, FullName = "Miodrag Petkovikj", OU = "Production", Password = "20152", RoleId = 2 },
                new User { Id = 90, CompanyId = 20159, FullName = "Gorancho Najdovski", OU = "Projects and IT", Password = "20159", RoleId = 2 },
                new User { Id = 91, CompanyId = 20160, FullName = "Dejan Jazadjiev", OU = "Supply chain", Password = "20160", RoleId = 2 },
                new User { Id = 92, CompanyId = 20162, FullName = "Sashko Peshov", OU = "Production", Password = "20162", RoleId = 2 },
                new User { Id = 93, CompanyId = 20163, FullName = "Kiro Radevski", OU = "Production", Password = "20163", RoleId = 2 },
                new User { Id = 94, CompanyId = 20167, FullName = "Sasho Beroski", OU = "Production", Password = "20167", RoleId = 2 },
                new User { Id = 95, CompanyId = 20168, FullName = "Vlatko Changovski", OU = "Projects and IT", Password = "20168", RoleId = 2 },
                new User { Id = 96, CompanyId = 20178, FullName = "Stojan Stavreski", OU = "Projects and IT", Password = "20178", RoleId = 2 },
                new User { Id = 97, CompanyId = 20182, FullName = "Kjemalj Abazi", OU = "Production", Password = "20182", RoleId = 2 },
                new User { Id = 98, CompanyId = 20184, FullName = "Djevat Selimi", OU = "Production", Password = "20184", RoleId = 2 },
                new User { Id = 99, CompanyId = 20191, FullName = "Trajche Dimovski", OU = "Projects and IT", Password = "20191", RoleId = 2 },
                new User { Id = 100, CompanyId = 20195, FullName = "Robert Shijakovski", OU = "Production", Password = "20195", RoleId = 2 },
                new User { Id = 101, CompanyId = 20203, FullName = "Slavisha Crnichin", OU = "Production", Password = "20203", RoleId = 2 },
                new User { Id = 102, CompanyId = 20210, FullName = "Borche Cvetkovski", OU = "Supply chain", Password = "20210", RoleId = 2 },
                new User { Id = 103, CompanyId = 20212, FullName = "Sasha Stefanoski", OU = "Supply chain", Password = "20212", RoleId = 2 },
                new User { Id = 104, CompanyId = 20218, FullName = "Josif Slavkovski", OU = "Production", Password = "20218", RoleId = 2 },
                new User { Id = 105, CompanyId = 20225, FullName = "Goce Stojchevski", OU = "Projects and IT", Password = "20225", RoleId = 2 },
                new User { Id = 106, CompanyId = 20226, FullName = "Donche Nedelkovski", OU = "Production", Password = "20226", RoleId = 2 },
                new User { Id = 107, CompanyId = 20231, FullName = "Ljupcho Shegmanovikj", OU = "Production", Password = "20231", RoleId = 2 },
                new User { Id = 108, CompanyId = 20232, FullName = "Goran Markovski", OU = "Production", Password = "20232", RoleId = 2 },
                new User { Id = 109, CompanyId = 20233, FullName = "Dragan Markovski", OU = "Projects and IT", Password = "20233", RoleId = 2 },
                new User { Id = 110, CompanyId = 20234, FullName = "Ljupcho Veljanovski", OU = "Production", Password = "20234", RoleId = 2 },
                new User { Id = 111, CompanyId = 20235, FullName = "Nikola Angeleski", OU = "Production", Password = "20235", RoleId = 2 },
                new User { Id = 112, CompanyId = 20236, FullName = "Aleksandar Bogoev", OU = "Production", Password = "20236", RoleId = 2 },
                new User { Id = 113, CompanyId = 20238, FullName = "Stevche Velkovski", OU = "Projects and IT", Password = "20238", RoleId = 2 },
                new User { Id = 114, CompanyId = 20245, FullName = "Ljubomir Kochovski", OU = "Projects and IT", Password = "20245", RoleId = 2 },
                new User { Id = 115, CompanyId = 20246, FullName = "Sashko Blazhevski", OU = "Production", Password = "20246", RoleId = 2 },
                new User { Id = 116, CompanyId = 20248, FullName = "Zoranche Borizovski", OU = "Production", Password = "20248", RoleId = 2 },
                new User { Id = 117, CompanyId = 20253, FullName = "Nebojsha Stojmanovikj", OU = "Projects and IT", Password = "20253", RoleId = 2 },
                new User { Id = 118, CompanyId = 20255, FullName = "Ljupcho Pashoski", OU = "Production", Password = "20255", RoleId = 2 },
                new User { Id = 119, CompanyId = 20261, FullName = "Aleksandar Karajanovski", OU = "Production", Password = "20261", RoleId = 2 },
                new User { Id = 120, CompanyId = 20262, FullName = "Dejan Stojanov", OU = "Supply chain", Password = "20262", RoleId = 2 },
                new User { Id = 121, CompanyId = 20263, FullName = "Vladimir Jakimov", OU = "Projects and IT", Password = "20263", RoleId = 2 },
                new User { Id = 122, CompanyId = 20267, FullName = "Goranche Ginoski", OU = "Production", Password = "20267", RoleId = 2 },
                new User { Id = 123, CompanyId = 20271, FullName = "Avdil Mustafa", OU = "Production", Password = "20271", RoleId = 2 },
                new User { Id = 124, CompanyId = 20272, FullName = "Beta Damevska", OU = "Projects and IT", Password = "20272", RoleId = 2 },
                new User { Id = 125, CompanyId = 20275, FullName = "Naser Ilazov", OU = "Production", Password = "20275", RoleId = 2 },
                new User { Id = 126, CompanyId = 20280, FullName = "Viktor Boshkovski", OU = "Production", Password = "20280", RoleId = 2 },
                new User { Id = 127, CompanyId = 20283, FullName = "Zlatko Petrovski", OU = "Supply chain", Password = "20283", RoleId = 2 },
                new User { Id = 128, CompanyId = 20284, FullName = "Aleksandar Stoicev", OU = "Production", Password = "20284", RoleId = 2 },
                new User { Id = 129, CompanyId = 20286, FullName = "Aleksandar Jovanovski", OU = "Production", Password = "20286", RoleId = 2 },
                new User { Id = 130, CompanyId = 20296, FullName = "Goran Jovanovski", OU = "Production", Password = "20296", RoleId = 2 },
                new User { Id = 131, CompanyId = 20297, FullName = "Ivan Maslov", OU = "Projects and IT", Password = "20297", RoleId = 2 },
                new User { Id = 132, CompanyId = 20298, FullName = "Ivica Tripunovski", OU = "Supply chain", Password = "20298", RoleId = 2 },
                new User { Id = 133, CompanyId = 20299, FullName = "Milisav Boshkovikj", OU = "Production", Password = "20299", RoleId = 2 },
                new User { Id = 134, CompanyId = 20300, FullName = "Ilija Pandurski", OU = "Production", Password = "20300", RoleId = 2 },
                new User { Id = 135, CompanyId = 20308, FullName = "Ljuben Trajkoski", OU = "Supply chain", Password = "20308", RoleId = 2 },
                new User { Id = 136, CompanyId = 20316, FullName = "Igor Petrovski", OU = "Projects and IT", Password = "20316", RoleId = 2 },
                new User { Id = 137, CompanyId = 20320, FullName = "Brankica Trajanoska", OU = "Supply chain", Password = "20320", RoleId = 2 },
                new User { Id = 138, CompanyId = 20323, FullName = "Blazhe Dimov", OU = "Projects and IT", Password = "20323", RoleId = 2 },
                new User { Id = 139, CompanyId = 20324, FullName = "Biljana Petrovska", OU = "Supply chain", Password = "20324", RoleId = 2 },
                new User { Id = 140, CompanyId = 20325, FullName = "Zoran Trajkov", OU = "Projects and IT", Password = "20325", RoleId = 2 },
                new User { Id = 141, CompanyId = 20328, FullName = "Slavko Spasovski", OU = "HR", Password = "20328", RoleId = 2 },
                new User { Id = 142, CompanyId = 20332, FullName = "Igorche Gjorgjievski", OU = "Projects and IT", Password = "20332", RoleId = 2 },
                new User { Id = 143, CompanyId = 20335, FullName = "Jovica Boshkovski", OU = "Production", Password = "20335", RoleId = 2 },
                new User { Id = 144, CompanyId = 20341, FullName = "Metodija Blazhevski", OU = "Supply chain", Password = "20341", RoleId = 2 },
                new User { Id = 145, CompanyId = 20350, FullName = "Sasho Petrushevski", OU = "Supply chain", Password = "20350", RoleId = 2 },
                new User { Id = 146, CompanyId = 20351, FullName = "Marjan Stojanovski", OU = "Supply chain", Password = "20351", RoleId = 2 },
                new User { Id = 147, CompanyId = 20357, FullName = "Dejan Petrushevski", OU = "Sales", Password = "20357", RoleId = 2 },
                new User { Id = 148, CompanyId = 20362, FullName = "Sasho Kiprijanovski", OU = "Supply chain", Password = "20362", RoleId = 2 },
                new User { Id = 149, CompanyId = 20363, FullName = "Zoran Mitevski", OU = "Production", Password = "20363", RoleId = 2 },
                new User { Id = 150, CompanyId = 20372, FullName = "Sasho Gjorgjievski", OU = "Production", Password = "20372", RoleId = 2 },
                new User { Id = 151, CompanyId = 20380, FullName = "Orce Angelovski", OU = "Projects and IT", Password = "20380", RoleId = 2 },
                new User { Id = 152, CompanyId = 20381, FullName = "Dejan Danilovski", OU = "Supply chain", Password = "20381", RoleId = 2 },
                new User { Id = 153, CompanyId = 20382, FullName = "Robert Angelovski", OU = "Projects and IT", Password = "20382", RoleId = 2 },
                new User { Id = 154, CompanyId = 20385, FullName = "Bratislav Mihajlovikj", OU = "Supply chain", Password = "20385", RoleId = 2 },
                new User { Id = 155, CompanyId = 20387, FullName = "Ace Mitevski", OU = "Supply chain", Password = "20387", RoleId = 2 },
                new User { Id = 156, CompanyId = 20389, FullName = "Igorche Markovski", OU = "Production", Password = "20389", RoleId = 2 },
                new User { Id = 157, CompanyId = 20390, FullName = "Jovan Markovski", OU = "Supply chain", Password = "20390", RoleId = 2 },
                new User { Id = 158, CompanyId = 20393, FullName = "Boban Hristovski", OU = "Supply chain", Password = "20393", RoleId = 2 },
                new User { Id = 159, CompanyId = 20395, FullName = "Dejan Dimishkovski", OU = "Production", Password = "20395", RoleId = 2 },
                new User { Id = 160, CompanyId = 20397, FullName = "Marjan Simonovski", OU = "Production", Password = "20397", RoleId = 2 },
                new User { Id = 161, CompanyId = 20402, FullName = "Slavica Mladenovska", OU = "Supply chain", Password = "20402", RoleId = 2 },
                new User { Id = 162, CompanyId = 20431, FullName = "Angelina Rajovska", OU = "HR", Password = "20431", RoleId = 2 },
                new User { Id = 163, CompanyId = 20439, FullName = "Dejan Dilevski", OU = "Production", Password = "20439", RoleId = 2 },
                new User { Id = 164, CompanyId = 20443, FullName = "Aleksandar Zotikj", OU = "Supply chain", Password = "20443", RoleId = 2 },
                new User { Id = 165, CompanyId = 20445, FullName = "Jovica Maznevski", OU = "Production", Password = "20445", RoleId = 2 },
                new User { Id = 166, CompanyId = 20447, FullName = "Zvonko Neshkovikj", OU = "Production", Password = "20447", RoleId = 2 },
                new User { Id = 167, CompanyId = 20448, FullName = "Ljupcho Paunkov", OU = "Production", Password = "20448", RoleId = 2 },
                new User { Id = 168, CompanyId = 20449, FullName = "Marjan Zdravkovski", OU = "Production", Password = "20449", RoleId = 2 },
                new User { Id = 169, CompanyId = 20451, FullName = "Igor Jordanovski", OU = "Production", Password = "20451", RoleId = 2 },
                new User { Id = 170, CompanyId = 20453, FullName = "Aleksandar Stamchevski", OU = "Production", Password = "20453", RoleId = 2 },
                new User { Id = 171, CompanyId = 20454, FullName = "Dejan Vasilevski", OU = "Production", Password = "20454", RoleId = 2 },
                new User { Id = 172, CompanyId = 20459, FullName = "Zoran Stojanovski", OU = "Production", Password = "20459", RoleId = 2 },
                new User { Id = 173, CompanyId = 20466, FullName = "Iljaz Prekopuca", OU = "Production", Password = "20466", RoleId = 2 },
                new User { Id = 174, CompanyId = 20468, FullName = "Fadil Tanalari", OU = "Production", Password = "20468", RoleId = 2 },
                new User { Id = 175, CompanyId = 20471, FullName = "Trajche Petrovski", OU = "Projects and IT", Password = "20471", RoleId = 2 },
                new User { Id = 176, CompanyId = 20475, FullName = "Boban Mitrovikj", OU = "Production", Password = "20475", RoleId = 2 },
                new User { Id = 177, CompanyId = 20478, FullName = "Dragan Saveski", OU = "Production", Password = "20478", RoleId = 2 },
                new User { Id = 178, CompanyId = 20489, FullName = "Ajdin Zulfiovski", OU = "Production", Password = "20489", RoleId = 2 },
                new User { Id = 179, CompanyId = 20511, FullName = "Ivan Cibrev", OU = "Supply chain", Password = "20511", RoleId = 2 },
                new User { Id = 180, CompanyId = 20518, FullName = "Slavica Jovchevska", OU = "Supply chain", Password = "20518", RoleId = 2 },
                new User { Id = 181, CompanyId = 20521, FullName = "Elena Damchevska", OU = "Finance Department", Password = "20521", RoleId = 2 },
                new User { Id = 182, CompanyId = 20523, FullName = "Vesna Dimevska", OU = "Supply chain", Password = "20523", RoleId = 2 },
                new User { Id = 183, CompanyId = 20527, FullName = "Kiril Simonoski", OU = "Projects and IT", Password = "20527", RoleId = 2 },
                new User { Id = 184, CompanyId = 20530, FullName = "Goce Atanasoski", OU = "Projects and IT", Password = "20530", RoleId = 2 },
                new User { Id = 185, CompanyId = 20603, FullName = "Goran Stojchevski", OU = "Production", Password = "20603", RoleId = 2 },
                new User { Id = 186, CompanyId = 20621, FullName = "Todorka Ristovska", OU = "CEO office", Password = "20621", RoleId = 2 },
                new User { Id = 187, CompanyId = 20623, FullName = "Elena Blazeva", OU = "Finance Department", Password = "20623", RoleId = 2 },
                new User { Id = 188, CompanyId = 20625, FullName = "Darko Najdenov", OU = "Supply chain", Password = "20625", RoleId = 2 },
                new User { Id = 189, CompanyId = 20632, FullName = "Zoran Mladenovski", OU = "Projects and IT", Password = "20632", RoleId = 2 },
                new User { Id = 190, CompanyId = 20636, FullName = "Natalija Nikoloska", OU = "Supply chain", Password = "20636", RoleId = 2 },
                new User { Id = 191, CompanyId = 20637, FullName = "Aleksandar Krstev", OU = "Supply chain", Password = "20637", RoleId = 2 },
                new User { Id = 192, CompanyId = 20638, FullName = "Elena Kocevska Peceva", OU = "Supply chain", Password = "20638", RoleId = 2 },
                new User { Id = 193, CompanyId = 20640, FullName = "Kiro Risteski", OU = "Production", Password = "20640", RoleId = 2 },
                new User { Id = 194, CompanyId = 20650, FullName = "Dejana Jovanova Krsteva", OU = "Supply chain", Password = "20650", RoleId = 2 },
                new User { Id = 195, CompanyId = 20652, FullName = "Toni Pandilovski", OU = "Projects and IT", Password = "20652", RoleId = 2 },
                new User { Id = 196, CompanyId = 20662, FullName = "Vladimir Shulevski", OU = "Production", Password = "20662", RoleId = 2 },
                new User { Id = 197, CompanyId = 20675, FullName = "Dejan Trajkovski", OU = "HR", Password = "20675", RoleId = 2 },
                new User { Id = 198, CompanyId = 20678, FullName = "Kire Blagoeski", OU = "Supply chain", Password = "20678", RoleId = 2 },
                new User { Id = 199, CompanyId = 20685, FullName = "Petar Brashnarov", OU = "Production", Password = "20685", RoleId = 2 },
                new User { Id = 200, CompanyId = 20694, FullName = "Zvonimir Manchevski", OU = "Production", Password = "20694", RoleId = 2 },
                new User { Id = 201, CompanyId = 20695, FullName = "Aleksandar Dejanovski", OU = "Projects and IT", Password = "20695", RoleId = 2 },
                new User { Id = 202, CompanyId = 20707, FullName = "Selaedin Feratovski", OU = "Projects and IT", Password = "20707", RoleId = 2 },
                new User { Id = 203, CompanyId = 20708, FullName = "Slave Manevski", OU = "Projects and IT", Password = "20708", RoleId = 2 },
                new User { Id = 204, CompanyId = 20723, FullName = "Djevat Saliovski", OU = "Production", Password = "20723", RoleId = 2 },
                new User { Id = 205, CompanyId = 20724, FullName = "Vesna Velichkovska", OU = "HR", Password = "20724", RoleId = 1 },
                new User { Id = 206, CompanyId = 20729, FullName = "Vlatko Dimishkovski", OU = "Projects and IT", Password = "20729", RoleId = 2 },
                new User { Id = 207, CompanyId = 20734, FullName = "Blage Uroshevski", OU = "Production", Password = "20734", RoleId = 2 },
                new User { Id = 208, CompanyId = 20735, FullName = "Stojadin Jankovski", OU = "Production", Password = "20735", RoleId = 2 },
                new User { Id = 209, CompanyId = 20737, FullName = "Zlatko Nikoloski", OU = "Production", Password = "20737", RoleId = 2 },
                new User { Id = 210, CompanyId = 20747, FullName = "Goce Gjorgjievski", OU = "Production", Password = "20747", RoleId = 2 },
                new User { Id = 211, CompanyId = 20751, FullName = "Stefan Tonevski", OU = "Supply chain", Password = "20751", RoleId = 2 },
                new User { Id = 212, CompanyId = 20753, FullName = "Orce Dimovski", OU = "Production", Password = "20753", RoleId = 2 },
                new User { Id = 213, CompanyId = 20758, FullName = "Elena Valkancheva Najdenova", OU = "Sales", Password = "20758", RoleId = 2 },
                new User { Id = 214, CompanyId = 20776, FullName = "Igor Dushanovski", OU = "Projects and IT", Password = "20776", RoleId = 2 },
                new User { Id = 215, CompanyId = 20779, FullName = "Jagoda Velevska", OU = "CEO office", Password = "20779", RoleId = 2 },
                new User { Id = 216, CompanyId = 20781, FullName = "Vladimir Nikolikj", OU = "Supply chain", Password = "20781", RoleId = 2 },
                new User { Id = 217, CompanyId = 20784, FullName = "Dejan Gocevski", OU = "Production", Password = "20784", RoleId = 2 },
                new User { Id = 218, CompanyId = 20787, FullName = "Aleksandar Kostovski", OU = "Production", Password = "20787", RoleId = 2 },
                new User { Id = 219, CompanyId = 20797, FullName = "Cane Nikoloski", OU = "Production", Password = "20797", RoleId = 2 },
                new User { Id = 220, CompanyId = 20800, FullName = "Viktor Stamenkovski", OU = "Projects and IT", Password = "20800", RoleId = 2 },
                new User { Id = 221, CompanyId = 20801, FullName = "Dragana Petrovikj", OU = "Supply chain", Password = "20801", RoleId = 2 },
                new User { Id = 222, CompanyId = 20802, FullName = "Stefan Despodovski", OU = "Supply chain", Password = "20802", RoleId = 2 },
                new User { Id = 223, CompanyId = 20803, FullName = "Marjan Milanovski", OU = "Projects and IT", Password = "20803", RoleId = 2 },
                new User { Id = 224, CompanyId = 20804, FullName = "Dragan Koneski", OU = "Projects and IT", Password = "20804", RoleId = 2 },
                new User { Id = 225, CompanyId = 20814, FullName = "Aleksandar Stojanovski", OU = "Production", Password = "20814", RoleId = 2 },
                new User { Id = 226, CompanyId = 20815, FullName = "Sashko Miloshevski", OU = "Production", Password = "20815", RoleId = 2 },
                new User { Id = 227, CompanyId = 20822, FullName = "Elza Petrovska", OU = "Sales", Password = "20822", RoleId = 2 },
                new User { Id = 228, CompanyId = 20824, FullName = "Darko Zdravkovski", OU = "Production", Password = "20824", RoleId = 2 },
                new User { Id = 229, CompanyId = 20825, FullName = "Kiril Chirkov", OU = "Production", Password = "20825", RoleId = 2 },
                new User { Id = 230, CompanyId = 20827, FullName = "Igor Cvetanoski", OU = "Production", Password = "20827", RoleId = 2 },
                new User { Id = 231, CompanyId = 20831, FullName = "Martin Nikolovski", OU = "Production", Password = "20831", RoleId = 2 },
                new User { Id = 232, CompanyId = 20832, FullName = "Dushko Blazevski", OU = "Supply chain", Password = "20832", RoleId = 2 },
                new User { Id = 233, CompanyId = 20834, FullName = "Muammet Sali", OU = "Projects and IT", Password = "20834", RoleId = 2 },
                new User { Id = 234, CompanyId = 20835, FullName = "Kristijan Janev", OU = "Projects and IT", Password = "20835", RoleId = 2 },
                new User { Id = 235, CompanyId = 20837, FullName = "Dilaver Sali", OU = "Production", Password = "20837", RoleId = 2 },
                new User { Id = 236, CompanyId = 20838, FullName = "Sasho Neshkov", OU = "Production", Password = "20838", RoleId = 2 },
                new User { Id = 237, CompanyId = 20839, FullName = "Goran Ilikj", OU = "Production", Password = "20839", RoleId = 2 },
                new User { Id = 238, CompanyId = 20842, FullName = "Zoran Trendevski", OU = "Production", Password = "20842", RoleId = 2 },
                new User { Id = 239, CompanyId = 20844, FullName = "Igor Lazevski", OU = "Production", Password = "20844", RoleId = 2 },
                new User { Id = 240, CompanyId = 20847, FullName = "Dragan Dragutinovski", OU = "Projects and IT", Password = "20847", RoleId = 2 },
                new User { Id = 241, CompanyId = 20848, FullName = "Hristo Jovanovski", OU = "Production", Password = "20848", RoleId = 2 },
                new User { Id = 242, CompanyId = 20851, FullName = "Goran Moskov", OU = "Production", Password = "20851", RoleId = 2 },
                new User { Id = 243, CompanyId = 20852, FullName = "Zoran Nikolovski", OU = "Projects and IT", Password = "20852", RoleId = 2 },
                new User { Id = 244, CompanyId = 20855, FullName = "Goran Cvetanovski", OU = "Production", Password = "20855", RoleId = 2 },
                new User { Id = 245, CompanyId = 20866, FullName = "Pero Mangarov", OU = "Supply chain", Password = "20866", RoleId = 2 },
                new User { Id = 246, CompanyId = 20871, FullName = "Igor Blazevski", OU = "Production", Password = "20871", RoleId = 2 },
                new User { Id = 247, CompanyId = 20872, FullName = "Spase Belinski", OU = "Projects and IT", Password = "20872", RoleId = 2 },
                new User { Id = 248, CompanyId = 20876, FullName = "Zhivorad Arsenovski", OU = "Production", Password = "20876", RoleId = 2 },
                new User { Id = 249, CompanyId = 20879, FullName = "Ljupcho Pijakovski", OU = "Production", Password = "20879", RoleId = 2 },
                new User { Id = 250, CompanyId = 20883, FullName = "Ljupcho Dimitrijeski", OU = "Projects and IT", Password = "20883", RoleId = 2 },
                new User { Id = 251, CompanyId = 20889, FullName = "Dushan Jovanoski", OU = "Sales", Password = "20889", RoleId = 2 },
                new User { Id = 252, CompanyId = 20893, FullName = "Nikola Nikolovski", OU = "Sales", Password = "20893", RoleId = 2 },
                new User { Id = 253, CompanyId = 20894, FullName = "Dimitar Jankovski", OU = "Production", Password = "20894", RoleId = 2 },
                new User { Id = 254, CompanyId = 20896, FullName = "Imer Ljusjani", OU = "Projects and IT", Password = "20896", RoleId = 2 },
                new User { Id = 255, CompanyId = 20898, FullName = "Bobi Gjogjievski", OU = "Projects and IT", Password = "20898", RoleId = 2 },
                new User { Id = 256, CompanyId = 20899, FullName = "Hristijan Gjorgjevski", OU = "Production", Password = "20899", RoleId = 2 },
                new User { Id = 257, CompanyId = 20903, FullName = "Temelko Sarovski", OU = "Production", Password = "20903", RoleId = 2 },
                new User { Id = 258, CompanyId = 20910, FullName = "Hristijan Simonovski", OU = "Supply chain", Password = "20910", RoleId = 2 },
                new User { Id = 259, CompanyId = 20911, FullName = "Dame Kekenovski", OU = "Production", Password = "20911", RoleId = 2 },
                new User { Id = 260, CompanyId = 20914, FullName = "Afrim Jusufi", OU = "Production", Password = "20914", RoleId = 2 },
                new User { Id = 261, CompanyId = 20915, FullName = "Igor Damjanovski", OU = "Supply chain", Password = "20915", RoleId = 2 },
                new User { Id = 262, CompanyId = 20916, FullName = "Besnik Ibraimi", OU = "Projects and IT", Password = "20916", RoleId = 2 },
                new User { Id = 263, CompanyId = 20917, FullName = "Viktor Velichkovski", OU = "Production", Password = "20917", RoleId = 2 },
                new User { Id = 264, CompanyId = 20919, FullName = "Robert Jovanovski", OU = "Projects and IT", Password = "20919", RoleId = 2 },
                new User { Id = 265, CompanyId = 20920, FullName = "Adnan Feratovski", OU = "Projects and IT", Password = "20920", RoleId = 2 },
                new User { Id = 266, CompanyId = 20924, FullName = "Biljana Chorobenska", OU = "Supply chain", Password = "20924", RoleId = 2 },
                new User { Id = 267, CompanyId = 20927, FullName = "Vladan Trajkovski", OU = "Projects and IT", Password = "20927", RoleId = 2 },
                new User { Id = 268, CompanyId = 20928, FullName = "Vlatko Mitevski", OU = "Production", Password = "20928", RoleId = 2 },
                new User { Id = 269, CompanyId = 20935, FullName = "Adis Nezirovski", OU = "Projects and IT", Password = "20935", RoleId = 2 },
                new User { Id = 270, CompanyId = 20936, FullName = "Asim Nezirovski", OU = "Projects and IT", Password = "20936", RoleId = 2 },
                new User { Id = 271, CompanyId = 20937, FullName = "Goce Spaseski", OU = "Production", Password = "20937", RoleId = 2 },
                new User { Id = 272, CompanyId = 20942, FullName = "Dragi Ickovski", OU = "Projects and IT", Password = "20942", RoleId = 2 },
                new User { Id = 273, CompanyId = 20944, FullName = "Ibrahim Mujovikj", OU = "Production", Password = "20944", RoleId = 2 },
                new User { Id = 274, CompanyId = 20948, FullName = "Boban Grozdanovski", OU = "Projects and IT", Password = "20948", RoleId = 2 },
                new User { Id = 275, CompanyId = 20951, FullName = "Robert Stojanovikj", OU = "Projects and IT", Password = "20951", RoleId = 2 },
                new User { Id = 276, CompanyId = 20953, FullName = "Mihajlo Zafirovikj", OU = "Production", Password = "20953", RoleId = 2 },
                new User { Id = 277, CompanyId = 20955, FullName = "Aleksandra Trgachevska", OU = "Supply chain", Password = "20955", RoleId = 2 },
                new User { Id = 278, CompanyId = 20958, FullName = "Marjanche Ristovski", OU = "Production", Password = "20958", RoleId = 2 },
                new User { Id = 279, CompanyId = 20963, FullName = "Dalibor Cvetkovski", OU = "Production", Password = "20963", RoleId = 2 },
                new User { Id = 280, CompanyId = 20964, FullName = "Ivica Stanoeski", OU = "Projects and IT", Password = "20964", RoleId = 2 },
                new User { Id = 281, CompanyId = 20967, FullName = "Gjorgji Velichkovski", OU = "Supply chain", Password = "20967", RoleId = 2 },
                new User { Id = 282, CompanyId = 20968, FullName = "Karanfilka Giceva", OU = "Supply chain", Password = "20968", RoleId = 2 },
                new User { Id = 283, CompanyId = 20971, FullName = "Djevat Feratovski", OU = "Production", Password = "20971", RoleId = 2 },
                new User { Id = 284, CompanyId = 20973, FullName = "Ivan Mitodevski", OU = "Production", Password = "20973", RoleId = 2 },
                new User { Id = 285, CompanyId = 20975, FullName = "Robert Ristovski", OU = "Projects and IT", Password = "20975", RoleId = 1 },
                new User { Id = 286, CompanyId = 20977, FullName = "Vlatko Dimevski", OU = "Supply chain", Password = "20977", RoleId = 2 },
                new User { Id = 287, CompanyId = 20979, FullName = "Violeta Vidinska", OU = "HR", Password = "20979", RoleId = 2 },
                new User { Id = 288, CompanyId = 20981, FullName = "Aco Jovanovski", OU = "Projects and IT", Password = "20981", RoleId = 2 },
                new User { Id = 289, CompanyId = 20982, FullName = "Rade Panovski", OU = "Production", Password = "20982", RoleId = 2 },
                new User { Id = 290, CompanyId = 20983, FullName = "Slave Joshovikj", OU = "Production", Password = "20983", RoleId = 2 },
                new User { Id = 291, CompanyId = 20988, FullName = "Nenad Petkovikj", OU = "Projects and IT", Password = "20988", RoleId = 2 },
                new User { Id = 292, CompanyId = 20989, FullName = "Borche Livrinski", OU = "Projects and IT", Password = "20989", RoleId = 2 },
                new User { Id = 293, CompanyId = 20994, FullName = "Sanja Lambrinidis", OU = "Supply chain", Password = "20994", RoleId = 2 },
                new User { Id = 294, CompanyId = 20998, FullName = "Ace Jovanovski", OU = "Production", Password = "20998", RoleId = 2 },
                new User { Id = 295, CompanyId = 20999, FullName = "Sashe Smilkovski", OU = "Projects and IT", Password = "20999", RoleId = 2 },
                new User { Id = 296, CompanyId = 21002, FullName = "Leon Danilovski", OU = "Supply chain", Password = "21002", RoleId = 2 },
                new User { Id = 297, CompanyId = 21003, FullName = "Enis Zekjiri", OU = "Projects and IT", Password = "21003", RoleId = 2 },
                new User { Id = 298, CompanyId = 21006, FullName = "Metodija Malkov", OU = "Production", Password = "21006", RoleId = 2 },
                new User { Id = 299, CompanyId = 21010, FullName = "Stefan Risteski", OU = "Projects and IT", Password = "21010", RoleId = 2 },
                new User { Id = 300, CompanyId = 21012, FullName = "Igor Momchilovski", OU = "Production", Password = "21012", RoleId = 2 },
                new User { Id = 301, CompanyId = 21016, FullName = "Oliver Govedarovski", OU = "Projects and IT", Password = "21016", RoleId = 2 },
                new User { Id = 302, CompanyId = 21017, FullName = "Bobi Nikolovski", OU = "Projects and IT", Password = "21017", RoleId = 2 },
                new User { Id = 303, CompanyId = 21020, FullName = "Kristijan Stojanovski", OU = "Supply chain", Password = "21020", RoleId = 2 },
                new User { Id = 304, CompanyId = 21082, FullName = "Rufat Rufati", OU = "Production", Password = "21082", RoleId = 2 },
                new User { Id = 305, CompanyId = 21090, FullName = "Vase Pecevski", OU = "Projects and IT", Password = "21090", RoleId = 2 },
                new User { Id = 306, CompanyId = 21094, FullName = "Kristina Karajanovska", OU = "Sales", Password = "21094", RoleId = 2 },
                new User { Id = 307, CompanyId = 21095, FullName = "Srechko Vidinski", OU = "Production", Password = "21095", RoleId = 2 },
                new User { Id = 308, CompanyId = 21096, FullName = "Nikola Spasevski", OU = "Projects and IT", Password = "21096", RoleId = 2 },
                new User { Id = 309, CompanyId = 21097, FullName = "Zvonko Miloshoski", OU = "Supply chain", Password = "21097", RoleId = 2 },
                new User { Id = 310, CompanyId = 21100, FullName = "Ismail Redzepi", OU = "Production", Password = "21100", RoleId = 2 },
                new User { Id = 311, CompanyId = 21104, FullName = "Aleksandar Kekenovski", OU = "Production", Password = "21104", RoleId = 2 },
                new User { Id = 312, CompanyId = 21112, FullName = "Nikola Nikolovski", OU = "Production", Password = "21112", RoleId = 2 },
                new User { Id = 313, CompanyId = 21117, FullName = "Jovica Stojanovikj", OU = "Production", Password = "21117", RoleId = 2 },
                new User { Id = 314, CompanyId = 21119, FullName = "Vasil Kocevski", OU = "Production", Password = "21119", RoleId = 2 },
                new User { Id = 315, CompanyId = 21121, FullName = "Petre Kushinovski", OU = "Production", Password = "21121", RoleId = 2 },
                new User { Id = 316, CompanyId = 21125, FullName = "Mitko Lebamov", OU = "Projects and IT", Password = "21125", RoleId = 2 },
                new User { Id = 317, CompanyId = 21126, FullName = "Aleksandar Boshkovski", OU = "Production", Password = "21126", RoleId = 2 },
                new User { Id = 318, CompanyId = 21128, FullName = "Gjuner Ismailovski", OU = "Projects and IT", Password = "21128", RoleId = 2 },
                new User { Id = 319, CompanyId = 21131, FullName = "Jovan Markovski", OU = "Production", Password = "21131", RoleId = 2 },
                new User { Id = 320, CompanyId = 21133, FullName = "Kjamuran Muaremovski", OU = "Production", Password = "21133", RoleId = 2 },
                new User { Id = 321, CompanyId = 21134, FullName = "Nikola Panovski", OU = "Projects and IT", Password = "21134", RoleId = 2 },
                new User { Id = 322, CompanyId = 21136, FullName = "Stefan Ristovski", OU = "Production", Password = "21136", RoleId = 2 },
                new User { Id = 323, CompanyId = 21139, FullName = "Jasin Ismailovski", OU = "Production", Password = "21139", RoleId = 2 },
                new User { Id = 324, CompanyId = 21140, FullName = "Borko Sokolovikj", OU = "Production", Password = "21140", RoleId = 2 },
                new User { Id = 325, CompanyId = 21142, FullName = "Stojan Despotoski", OU = "Production", Password = "21142", RoleId = 2 },
                new User { Id = 326, CompanyId = 21143, FullName = "Shezair Lazam", OU = "Production", Password = "21143", RoleId = 2 },
                new User { Id = 327, CompanyId = 21149, FullName = "Jovan Stojanovski", OU = "Projects and IT", Password = "21149", RoleId = 2 },
                new User { Id = 328, CompanyId = 21151, FullName = "Kire Krusharski", OU = "Production", Password = "21151", RoleId = 2 },
                new User { Id = 329, CompanyId = 21152, FullName = "Igorche Kuzmanov", OU = "Production", Password = "21152", RoleId = 2 },
                new User { Id = 330, CompanyId = 21154, FullName = "Goce Zdravevski", OU = "Supply chain", Password = "21154", RoleId = 2 },
                new User { Id = 331, CompanyId = 21156, FullName = "Goran Vasilevski", OU = "Production", Password = "21156", RoleId = 2 },
                new User { Id = 332, CompanyId = 21160, FullName = "Deni Popovski", OU = "Supply chain", Password = "21160", RoleId = 2 },
                new User { Id = 333, CompanyId = 21171, FullName = "Jovan Chankulovski", OU = "Production", Password = "21171", RoleId = 2 },
                new User { Id = 334, CompanyId = 21174, FullName = "Dragi Risteski", OU = "Projects and IT", Password = "21174", RoleId = 2 },
                new User { Id = 335, CompanyId = 21175, FullName = "Zoran Urdarevikj", OU = "Production", Password = "21175", RoleId = 2 },
                new User { Id = 336, CompanyId = 21178, FullName = "Miroslav Martinovski", OU = "Production", Password = "21178", RoleId = 2 },
                new User { Id = 337, CompanyId = 21183, FullName = "Emran Iseinov", OU = "Production", Password = "21183", RoleId = 2 },
                new User { Id = 338, CompanyId = 21184, FullName = "Mirche Milkovski", OU = "Production", Password = "21184", RoleId = 2 },
                new User { Id = 339, CompanyId = 21188, FullName = "Aleksandar Kitanovski", OU = "Production", Password = "21188", RoleId = 2 },
                new User { Id = 340, CompanyId = 21189, FullName = "Dejan Stefanovski", OU = "Production", Password = "21189", RoleId = 2 },
                new User { Id = 341, CompanyId = 21190, FullName = "Viktor Stojchevski", OU = "Production", Password = "21190", RoleId = 2 },
                new User { Id = 342, CompanyId = 21191, FullName = "Dragan Risteski", OU = "Supply chain", Password = "21191", RoleId = 2 },
                new User { Id = 343, CompanyId = 21193, FullName = "Dzemail Ljimani", OU = "Production", Password = "21193", RoleId = 2 },
                new User { Id = 344, CompanyId = 21194, FullName = "Biljana Trajkovska", OU = "Supply chain", Password = "21194", RoleId = 2 },
                new User { Id = 345, CompanyId = 21196, FullName = "Miroslav Krstikj", OU = "Production", Password = "21196", RoleId = 2 },
                new User { Id = 346, CompanyId = 21197, FullName = "Violeta Stojanovska", OU = "CEO office", Password = "21197", RoleId = 2 },
                new User { Id = 347, CompanyId = 21198, FullName = "Kristina Kolaroska", OU = "Finance Department", Password = "21198", RoleId = 2 },
                new User { Id = 348, CompanyId = 21200, FullName = "David Savevski", OU = "Production", Password = "21200", RoleId = 2 },
                new User { Id = 349, CompanyId = 21201, FullName = "Emrah Sali", OU = "Production", Password = "21201", RoleId = 2 },
                new User { Id = 350, CompanyId = 21204, FullName = "Robert Ristovski", OU = "Production", Password = "21204", RoleId = 2 },
                new User { Id = 351, CompanyId = 21206, FullName = "Marjanche Milkovski", OU = "Projects and IT", Password = "21206", RoleId = 2 },
                new User { Id = 352, CompanyId = 21209, FullName = "Ice Trajkoski", OU = "Production", Password = "21209", RoleId = 2 },
                new User { Id = 353, CompanyId = 21212, FullName = "Viktor Ilievski", OU = "Production", Password = "21212", RoleId = 2 },
                new User { Id = 354, CompanyId = 21218, FullName = "Daniel Slavkovski", OU = "Production", Password = "21218", RoleId = 2 },
                new User { Id = 355, CompanyId = 21219, FullName = "Goce Peshevski", OU = "Production", Password = "21219", RoleId = 2 },
                new User { Id = 356, CompanyId = 21224, FullName = "Natasha Mihova", OU = "Finance Department", Password = "21224", RoleId = 2 },
                new User { Id = 357, CompanyId = 21225, FullName = "Bujar Zenuli", OU = "Production", Password = "21225", RoleId = 2 },
                new User { Id = 358, CompanyId = 21227, FullName = "Tamara Stojchevska", OU = "HR", Password = "21227", RoleId = 2 },
                new User { Id = 359, CompanyId = 21229, FullName = "Dragana Velkovikj-Krsteva", OU = "Supply chain", Password = "21229", RoleId = 2 },
                new User { Id = 360, CompanyId = 21231, FullName = "Jovica Stojanovski", OU = "Production", Password = "21231", RoleId = 2 },
                new User { Id = 361, CompanyId = 21233, FullName = "Mario Trajkovski", OU = "Projects and IT", Password = "21233", RoleId = 2 },
                new User { Id = 362, CompanyId = 21240, FullName = "Dancho Kostadinovski", OU = "Projects and IT", Password = "21240", RoleId = 2 },
                new User { Id = 363, CompanyId = 21241, FullName = "Konstantin Koneski", OU = "Supply chain", Password = "21241", RoleId = 2 },
                new User { Id = 364, CompanyId = 21243, FullName = "Nenad Mihajloski", OU = "Production", Password = "21243", RoleId = 2 },
                new User { Id = 365, CompanyId = 21247, FullName = "Ilija Andonoski", OU = "Supply chain", Password = "21247", RoleId = 2 },
                new User { Id = 366, CompanyId = 21252, FullName = "Toni Karovchevikj", OU = "Projects and IT", Password = "21252", RoleId = 2 },
                new User { Id = 367, CompanyId = 21254, FullName = "Hristijan Todorovski", OU = "Projects and IT", Password = "21254", RoleId = 2 },
                new User { Id = 368, CompanyId = 21257, FullName = "Atanas Boshkov", OU = "Production", Password = "21257", RoleId = 2 },
                new User { Id = 369, CompanyId = 21259, FullName = "Damjan Petrovski", OU = "Projects and IT", Password = "21259", RoleId = 2 },
                new User { Id = 370, CompanyId = 21260, FullName = "Viktorija Karafiloska", OU = "Supply chain", Password = "21260", RoleId = 2 },
                new User { Id = 371, CompanyId = 21261, FullName = "Sashko Janevski", OU = "Production", Password = "21261", RoleId = 2 },
                new User { Id = 372, CompanyId = 21262, FullName = "Maja Miloshoska", OU = "Supply chain", Password = "21262", RoleId = 2 },
                new User { Id = 373, CompanyId = 21263, FullName = "Elena Stoilkovska", OU = "HR", Password = "21263", RoleId = 2 },
                new User { Id = 374, CompanyId = 21268, FullName = "Dragan Najdovski", OU = "Projects and IT", Password = "21268", RoleId = 2 },
                new User { Id = 375, CompanyId = 21269, FullName = "Luka Bostandzievski", OU = "Production", Password = "21269", RoleId = 2 },
                new User { Id = 376, CompanyId = 21270, FullName = "Sinisha Voinoski", OU = "Production", Password = "21270", RoleId = 2 },
                new User { Id = 377, CompanyId = 21271, FullName = "Muhamed Mimin", OU = "Production", Password = "21271", RoleId = 2 },
                new User { Id = 378, CompanyId = 21274, FullName = "Nuija Nuijovski", OU = "Projects and IT", Password = "21274", RoleId = 2 },
                new User { Id = 379, CompanyId = 21275, FullName = "Svetlana Davkovska", OU = "Finance Department", Password = "21275", RoleId = 2 },
                new User { Id = 380, CompanyId = 21277, FullName = "Isa Zenelji", OU = "Production", Password = "21277", RoleId = 2 },
                new User { Id = 381, CompanyId = 21280, FullName = "Mario Nikolovski", OU = "Projects and IT", Password = "21280", RoleId = 2 },
                new User { Id = 382, CompanyId = 21281, FullName = "Angel Kostovski", OU = "Production", Password = "21281", RoleId = 2 },
                new User { Id = 383, CompanyId = 21282, FullName = "Hristijan Stevkovski", OU = "Supply chain", Password = "21282", RoleId = 2 },
                new User { Id = 384, CompanyId = 21283, FullName = "Naim Ajvazi", OU = "Production", Password = "21283", RoleId = 2 },
                new User { Id = 385, CompanyId = 21284, FullName = "Miodrag Achkovikj", OU = "Production", Password = "21284", RoleId = 2 },
                new User { Id = 386, CompanyId = 21285, FullName = "Andrej Velichkovski", OU = "Projects and IT", Password = "21285", RoleId = 2 },
                new User { Id = 387, CompanyId = 21286, FullName = "Dejan Smilevski", OU = "Projects and IT", Password = "21286", RoleId = 2 },
                new User { Id = 388, CompanyId = 21288, FullName = "Trajche Trajkovski", OU = "Production", Password = "21288", RoleId = 2 },
                new User { Id = 389, CompanyId = 21290, FullName = "Sashko Dimovski", OU = "Projects and IT", Password = "21290", RoleId = 2 },
                new User { Id = 390, CompanyId = 21292, FullName = "Dushan Manojlovikj", OU = "Production", Password = "21292", RoleId = 2 },
                new User { Id = 391, CompanyId = 21293, FullName = "Zoran Ilieski", OU = "Projects and IT", Password = "21293", RoleId = 2 },
                new User { Id = 392, CompanyId = 21294, FullName = "Antonio Panovski", OU = "Production", Password = "21294", RoleId = 2 },
                new User { Id = 393, CompanyId = 21295, FullName = "Violeta Joshovikj", OU = "HR", Password = "21295", RoleId = 2 },
                new User { Id = 394, CompanyId = 21297, FullName = "Sashka Stojanovska", OU = "HR", Password = "21297", RoleId = 2 },
                new User { Id = 395, CompanyId = 21298, FullName = "Ljupcho Emsherijov", OU = "Production", Password = "21298", RoleId = 2 },
                new User { Id = 396, CompanyId = 21299, FullName = "Nikola Risteski", OU = "Supply chain", Password = "21299", RoleId = 2 },
                new User { Id = 397, CompanyId = 21300, FullName = "Ljupcho Bogojev", OU = "Projects and IT", Password = "21300", RoleId = 2 },
                new User { Id = 398, CompanyId = 21302, FullName = "Erol Idriz", OU = "Projects and IT", Password = "21302", RoleId = 2 },
                new User { Id = 399, CompanyId = 21303, FullName = "Blagoja Jovchevski", OU = "Projects and IT", Password = "21303", RoleId = 2 },
                new User { Id = 400, CompanyId = 21304, FullName = "Stefan Trajkovikj", OU = "Production", Password = "21304", RoleId = 2 },
                new User { Id = 401, CompanyId = 21305, FullName = "Vesna Gjorgjevska", OU = "HR", Password = "21305", RoleId = 2 },
                new User { Id = 402, CompanyId = 21306, FullName = "Mihaela Gecheva", OU = "HR", Password = "21306", RoleId = 2 },
                new User { Id = 403, CompanyId = 21307, FullName = "Marija Malinova", OU = "Supply chain", Password = "21307", RoleId = 2 },
                new User { Id = 404, CompanyId = 21308, FullName = "Viktorija Siljanoska", OU = "Projects and IT", Password = "21308", RoleId = 2 },
                new User { Id = 405, CompanyId = 21309, FullName = "Aleksandar Paunkovikj", OU = "Projects and IT", Password = "21309", RoleId = 2 },
                new User { Id = 406, CompanyId = 21310, FullName = "Stefan Cvetanovski", OU = "Production", Password = "21310", RoleId = 2 },
                new User { Id = 407, CompanyId = 21311, FullName = "Valentina Cibreva", OU = "Finance Department", Password = "21311", RoleId = 2 },
                new User { Id = 408, CompanyId = 21312, FullName = "Milancho Uroshevski", OU = "Supply chain", Password = "21312", RoleId = 2 },
                new User { Id = 409, CompanyId = 21313, FullName = "Jashar Ismaili", OU = "HR", Password = "21313", RoleId = 2 },
                new User { Id = 410, CompanyId = 21314, FullName = "Daniel Neshkovikj", OU = "Daniel", Password = "21314", RoleId = 2 },
                new User { Id = 411, CompanyId = 21315, FullName = "Hristina Jovanovska", OU = "Projects and IT", Password = "21315", RoleId = 2 },
                new User { Id = 412, CompanyId = 21316, FullName = "Marjan Georgiev", OU = "Production", Password = "21316", RoleId = 2 }
                );
        }
    }
}
using System.Linq.Expressions;

namespace Anketa.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
using Anketa.Domain.Models;

namespace Anketa.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByCompanyIdAsync(int companyId);
        Task<User> GetUserWithRoleAsync(int userId);
        Task<User> AuthenticateAsync(int companyId, string fullName, string password);
        Task<string> GetUserOUAsync(int userId);
    }
}
using Anketa.Domain.Models;

namespace Anketa.DataAccess.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<QuestionForm>> GetActiveFormsAsync();
        Task<QuestionForm> GetFormWithQuestionsAsync(int formId);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        // Admin methods
        Task<Question> CreateQuestionAsync(Question question);
        Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId);
    }
}
using Anketa.Domain.Models;

namespace Anketa.DataAccess.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
        Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId);
        Task<bool> HasUserCompletedFormAsync(int userId, int formId);
        Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId);
    }
}

using Anketa.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Anketa.DataAccess.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.SingleOrDefaultAsync(predicate);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }
        public virtual void Update(T entity)
        {
            _entities.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }
    }
}
using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Anketa.DataAccess.Implementations
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuestionForm>> GetActiveFormsAsync()
        {
            return await _context.QuestionForms
                .Where(f => f.IsActive)
                .Include(f => f.Questions)
                .ThenInclude(q => q.QuestionType)
                .ToListAsync();
        }
       
        public async Task<QuestionForm> GetFormWithQuestionsAsync(int formId)
        {
            return await _context.QuestionForms
                .Include(f => f.Questions)
                .ThenInclude(q => q.QuestionType)
                .FirstOrDefaultAsync(f => f.Id == formId);
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions
                .Include(q => q.QuestionType)
                .Include(q => q.QuestionForm)
                .Include(q => q.User)
                .OrderBy(q => q.QuestionFormId)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _context.Questions
                .Include(q => q.QuestionType)
                .Include(q => q.User)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }
        // New method for admin to create questions
        public async Task<Question> CreateQuestionAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        // New method to get questions by form
        public async Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId)
        {
            return await _context.Questions
                .Where(q => q.QuestionFormId == formId)
                .Include(q => q.QuestionType)
                .ToListAsync();
        }
    }
}
using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Anketa.DataAccess.Implementations
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly AppDbContext _context;

        public AnswerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
            using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var answer in answers)
                {
                    var question = await _context.Questions
                        .Include(q => q.QuestionType)
                        .FirstOrDefaultAsync(q => q.Id == answer.Key);

                    if (question == null) continue;

                    var existingAnswer = await _context.Answers
                        .FirstOrDefaultAsync(a => a.UserId == userId &&
                                                a.QuestionId == answer.Key &&
                                                a.QuestionFormId == formId);

                    if (existingAnswer != null)
                    {
                        // Update existing answer
                        UpdateAnswerValue(existingAnswer, question.QuestionType.Name, answer.Value);
                        Update(existingAnswer);
                    }
                    else
                    {
                        // Create new answer
                        var newAnswer = new Answer
                        {
                            UserId = userId,
                            QuestionId = answer.Key,
                            QuestionFormId = formId,
                            AnsweredDate = DateTime.UtcNow
                        };
                        UpdateAnswerValue(newAnswer, question.QuestionType.Name, answer.Value);
                        await AddAsync(newAnswer);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
     private void UpdateAnswerValue(Answer answer, string questionType, object value)
        {
            if (questionType == "Scale" && value is int scaleValue)
            {
                answer.ScaleValue = scaleValue;
                answer.TextValue = null;
            }
            else if (questionType == "Text" && value is string textValue)
            {
                answer.TextValue = textValue;
                answer.ScaleValue = null;
            }
        }

        public async Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId)
        {
            return await _context.Answers
                .Where(a => a.UserId == userId && a.QuestionFormId == formId)
                .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
                .ToListAsync();
        }

        public async Task<bool> HasUserCompletedFormAsync(int userId, int formId)
        {
            return await _context.Answers
                .AnyAsync(a => a.UserId == userId && a.QuestionFormId == formId);
        }
        public async Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId)
        {
            return await _context.Answers
                .Where(a => a.QuestionFormId == formId)
                .Include(a => a.Question)
                .Include(a => a.User)
                .ToListAsync();
        }
    }
}
using Anketa.Domain.Models;

namespace Anketa.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(int companyId, string fullName, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<string> GetUserOUAsync(int userId);
    }
}
using Anketa.Domain.Models;

namespace Anketa.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionForm>> GetActiveFormsAsync();
        Task<QuestionForm> GetFormWithQuestionsAsync(int formId);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        // Admin methods
        Task<Question> CreateQuestionAsync(Question question);
        Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId);
    }
}
using Anketa.Domain.Models;

namespace Anketa.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
        Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId);
        Task<bool> HasUserCompletedFormAsync(int userId, int formId);
        Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId);
    }
}
using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Anketa.Services.Interfaces;

namespace Anketa.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(int companyId, string fullName, string password)
        {
            return await _userRepository.AuthenticateAsync(companyId, fullName, password);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserWithRoleAsync(id);
        }

        public async Task<string> GetUserOUAsync(int userId)
        {
            return await _userRepository.GetUserOUAsync(userId);
        }
    }
}
using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Anketa.Services.Interfaces;

namespace Anketa.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Question> CreateQuestionAsync(Question question)
        {
            return await _questionRepository.CreateQuestionAsync(question);
        }

        public async Task<IEnumerable<QuestionForm>> GetActiveFormsAsync()
        {
            return await _questionRepository.GetActiveFormsAsync();
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllQuestionsAsync();
        }

        public async Task<QuestionForm> GetFormWithQuestionsAsync(int formId)
        {
            return await _questionRepository.GetFormWithQuestionsAsync(formId);
        }
               
        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _questionRepository.GetQuestionByIdAsync(questionId);
        }

        public async Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId)
        {
            return await _questionRepository.GetQuestionsByFormAsync(formId);
        }
    }
}
using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Anketa.Services.Interfaces;

namespace Anketa.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
            return await _answerRepository.SubmitAnswersAsync(userId, formId, answers);
        }

        public async Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId)
        {
            return await _answerRepository.GetUserAnswersAsync(userId, formId);
        }
        public async Task<bool> HasUserCompletedFormAsync(int userId, int formId)
        {
            return await _answerRepository.HasUserCompletedFormAsync(userId, formId);
        }

        public async Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId)
        {
            return await _answerRepository.GetFormAnswersAsync(formId);
        }
    }
}
using Anketa.DataAccess;
using Anketa.DataAccess.Implementations;
using Anketa.DataAccess.Interfaces;
using Anketa.Services.Implementations;
using Anketa.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Anketa.Services.Extensions
{
    public static class InjectionExtensions
    {
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString));
        }
        public static void InjectRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
        }
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuestionService, QuestionService>();

        }
    }
}
** in Progam.cs

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.InjectDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.InjectRepositories();
builder.Services.InjectServices();
