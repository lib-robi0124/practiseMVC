Company needs MVC app as Questionnaire for employee satisfactory on different area of workplace. Idea is , Domain.Models to be:
User , Role , Question, QuestionType, QuestionForm, Answer
User are Administrators to be able to add, modify(update), delete Questions as per Roles, Users as Employees are Answer the QuestionForm with Question as Type of Scale from 1 to 10, or fill with Text. Answer has to be stored in database for future reporting, with UserId, QuestionFormId, QuestionId.
Main page will have login screen with CopmanyId and FullName of User
After successfully login, enter to QuestionForm with 3 Questions , like:
QuestionForm Title “ Општо задоволство “
QuestionId 1 Задоволен сум од мојата моментална работа
Answer is int from Scale 1 2 3 4 5 6 7 8 9 10
QuestionId 2 Чувствувам дека мојата работа е ценета во рамките на компанијата. 
Answer is int from Scale 1 2 3 4 5 6 7 8 9 10
QuestionId 3 Се чувствувам мотивиран да одам на работа секој ден. (1-10)
Answer is int from Scale 1 2 3 4 5 6 7 8 9 10
QuestionForm Title “ Отворени прашања” 
QuestionId 1 Што најмногу ви се допаѓа на вашето сегашно работно место? 
Answer is string Text (500)
QuestionId 2 Кои се најголемите предизвици со кои се соочувате на работа? 
Answer is string Text (500)
QuestionId 3 Какви предлози имате за подобрување на работната средина или процесите на компанијата? 
Answer is string Text (500)

Error on Add-Migration is :Could not load assembly 'Questionnaire.DataAccess'. Ensure it is referenced by the startup project 'Questionnaire.Skopje'.

1. Review the code, suggest soluton for error on creation database,
2. make suggestions for repository, services, controllers, view.models

public abstract class BaseEntity
{
    public int Id { get; set; }
}
public class User : BaseEntity
{
    public int CompanyId { get; set; }
    public string FullName { get; set; }
    public string OU { get; set; } // Organizational Unit
    public string RoleKey { get; set; }
    public Role Role { get; set; }
    public virtual ICollection<QuestionForm> QuestionForms { get; set; }
    public User()
    {
        QuestionForms = new HashSet<QuestionForm>();
    }
}
 public class Role
 {
     public string Key { get; set; }
     public string Name { get; set; }
     public virtual ICollection<User> Users { get; set; }
     public Role()
     {
         Users = new HashSet<User>();
     }
 }
public class Question : BaseEntity
{
    public string Name { get; set; }
    public string QuestionText { get; set; }
    public int QuestionTypeId { get; set; }
    public QuestionType QuestionType { get; set; }
    public virtual ICollection<Answer> Answers { get; set; }

    public Question()
    {
        Answers = new HashSet<Answer>();
    }
}
 public class QuestionType : BaseEntity
 {
     public string Name { get; set; }
     public virtual ICollection<Question> Questions { get; set; }
     public QuestionType()
     {
         Questions = new HashSet<Question>();
     }
 }
  public class QuestionForm : BaseEntity
 {
     public string Title { get; set; }
     public int UserId { get; set; }
     public User User { get; set; }
     public virtual ICollection<Answer> Answers { get; set; }
     public virtual ICollection<Question> Questions { get; set; }
     public QuestionForm()
     {
         Answers = new HashSet<Answer>();
         Questions = new HashSet<Question>();
     }
 }
 public class Answer : BaseEntity
 {
     public int QuestionFormId { get; set; }
     public QuestionForm QuestionForm { get; set; }
     public int QuestionId { get; set; }
     public Question Question { get; set; }
     public string Response { get; set; }
     public int UserId { get; set; }
     public User User { get; set; }
     public Answer()
     {
         Response = string.Empty;
     }

 }
public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CompanyId).IsRequired();
        builder.Property(x => x.FullName).IsRequired().HasMaxLength(128);
        builder.Property(x => x.OU).HasMaxLength(128);

        builder.HasIndex(x => x.CompanyId).IsUnique();
        //foreign key to UserRole
        builder.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleKey)
            .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise role, da ne se izbriseat i user-ite
            .HasConstraintName("FK_User_UserRole");
    }
}
 public class RoleConfig : IEntityTypeConfiguration<Role>
 {
     public void Configure(EntityTypeBuilder<Role> builder)
     {
         builder.ToTable("Role");
         builder.HasKey(x => x.Key);
         builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
     }
 }
  public class QuestionTypeConfig : IEntityTypeConfiguration<QuestionType>
 {
     public void Configure(EntityTypeBuilder<QuestionType> builder)
     {
         builder.ToTable("QuestionTypes");
         builder.HasKey(x => x.Id);
         builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
     }
 }
  public class QuestionFormConfig : IEntityTypeConfiguration<QuestionForm>
 {
     public void Configure(EntityTypeBuilder<QuestionForm> builder)
     {
         builder.ToTable("QuestionForms");
         builder.HasKey(x => x.Id);
         builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
         // foreign key to User
         builder.HasOne(x => x.User)
             .WithMany(x => x.QuestionForms)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise user, da ne se izbriseat i questionForms
             .HasConstraintName("FK_QuestionForm_User");
     }
 }
  public class AnswerConfig : IEntityTypeConfiguration<Answer>
 {
     public void Configure(EntityTypeBuilder<Answer> builder)
     {
         builder.ToTable("Answers");
         builder.HasKey(x => x.Id);
         builder.Property(x => x.Response).HasMaxLength(500);
        
         // foreign key to Question
         builder.HasOne(x => x.Question)
             .WithMany(x => x.Answers)
             .HasForeignKey(x => x.QuestionId)
             .OnDelete(DeleteBehavior.NoAction) 
             .HasConstraintName("FK_Answer_Question");
         // foreign key to QuestionForm
         builder.HasOne(x => x.QuestionForm)
             .WithMany(x => x.Answers)
             .HasForeignKey(x => x.QuestionFormId)
             .OnDelete(DeleteBehavior.NoAction) 
             .HasConstraintName("FK_Answer_QuestionForm");
     }

 }
  public static class DataSeedExtensions
 {
     public static ModelBuilder SeedUsers(this ModelBuilder modelBuilder)
     {
         modelBuilder.Entity<User>().HasData(
            new User { Id = 1, CompanyId = 16130, FullName = "Vasko Gjorgiev", OU = "Production", RoleKey = "user" },
             new User { Id = 2, CompanyId = 16684, FullName = "Zoran Stojanovski", OU = "Production", RoleKey = "user" },
             new User { Id = 3, CompanyId = 16695, FullName = "Pane Naskovski", OU = "Production", RoleKey = "user" },
             new User { Id = 4, CompanyId = 16720, FullName = "Tome Trajanov", OU = "Projects and IT", RoleKey = "user" },
             new User { Id = 5, CompanyId = 16827, FullName = "Zoran Boshkovski", OU = "Production", RoleKey = "user" },
             new User { Id = 6, CompanyId = 16984, FullName = "Dide Petrovski", OU = "Projects and IT", RoleKey = "user" },
             new User { Id = 7, CompanyId = 17011, FullName = "Jovica Gjorgjievski", OU = "Projects and IT", RoleKey = "user" },
             new User { Id = 8, CompanyId = 17055, FullName = "Blagica Besarovska", OU = "Projects and IT", RoleKey = "user" },
             new User { Id = 9, CompanyId = 17064, FullName = "Dragi Naskovski", OU = "Production", RoleKey = "user" },
             new User { Id = 10, CompanyId = 17148, FullName = "Borche Anchevski", OU = "Production", RoleKey = "user" },
             new User { Id = 411, CompanyId = 21315, FullName = "Hristina Jovanovska", OU = "Projects and IT", RoleKey = "user" },
             new User { Id = 412, CompanyId = 21316, FullName = "Marjan Georgiev", OU = "Production", RoleKey = "user" });

         return modelBuilder;
     }
     public static ModelBuilder SeedRoles(this ModelBuilder modelBuilder)
     {
         modelBuilder.Entity<Role>().HasData(
             new Role { Key = "admin", Name = "Administrator" },
             new Role { Key = "user", Name = "User" }
         );

         return modelBuilder;
     }
     public static ModelBuilder SeedQuestionType(this ModelBuilder modelBuilder)
     {
         modelBuilder.Entity<QuestionType>().HasData(
             new QuestionType { Id = 1, Name = "Scale" },
             new QuestionType { Id = 2, Name = "Text" }
         );
         return modelBuilder;
     }
     public static ModelBuilder SeedQuestion(this ModelBuilder modelBuilder)
     {
         modelBuilder.Entity<Question>().HasData(
             new Question { Id = 1, QuestionText = "Задоволен сум од мојата моментална работа", QuestionTypeId = 1 },
             new Question { Id = 2, QuestionText = "Чувствувам дека мојата работа е ценета во рамките на компанијата", QuestionTypeId = 1 },
             new Question { Id = 3, QuestionText = "Се чувствувам мотивиран да одам на работа секој ден", QuestionTypeId = 1 },
             new Question { Id = 4, QuestionText = "Се чувствувам горд што работам за оваа компанија", QuestionTypeId = 1 },
             new Question { Id = 5, QuestionText = "Со задоволство ја препорачувам оваа компанија како работно место на пријателите и семејството", QuestionTypeId = 1 },
             new Question { Id = 6, QuestionText = "Се гледам себеси како долгорочно работам во оваа компанија", QuestionTypeId = 1 },
             new Question { Id = 7, QuestionText = "Имам можности за професионален развој во рамките на компанијата", QuestionTypeId = 1 },
             new Question { Id = 8, QuestionText = "Добивам конструктивен фидбек кој ми помага да се подобрам", QuestionTypeId = 1 },
             new Question { Id = 9, QuestionText = "Компанијата обезбедува соодветна обука и ресурси за мојот развој", QuestionTypeId = 1 },
             new Question { Id = 10, QuestionText = "Компанијата поддржува здрава рамнотежа помеѓу работата и личниот живот", QuestionTypeId = 1 },
             new Question { Id = 11, QuestionText = "Можам ефикасно да управувам со стресот поврзан со работата", QuestionTypeId = 1 },
             new Question { Id = 12, QuestionText = "Мојот работен распоред ми овозможува да ги исполнувам моите лични обврски", QuestionTypeId = 1 },
             new Question { Id = 13, QuestionText = "Комуникацијата во мојот тим е ефикасна", QuestionTypeId = 1 },
             new Question { Id = 14, QuestionText = "Се чувствувам удобно да ги искажувам моите идеи и мислења на работа. ", QuestionTypeId = 1 },
             new Question { Id = 15, QuestionText = "Соработката помеѓу одделенијата е ефикасна", QuestionTypeId = 1 },
             new Question { Id = 16, QuestionText = "Му верувам на раководството на компанијата", QuestionTypeId = 1 },
             new Question { Id = 17, QuestionText = "Мојот директен менаџер ме поддржува во остварувањето на моите цели", QuestionTypeId = 1 },
             new Question { Id = 18, QuestionText = "Важните одлуки на компанијата се пренесуваат транспарентно", QuestionTypeId = 1 },
             new Question { Id = 19, QuestionText = "Вредностите на компанијата се усогласуваат со моите лични вредности", QuestionTypeId = 1 },
             new Question { Id = 20, QuestionText = "Се чувствувам вклучено и почитувано на работа", QuestionTypeId = 1 },
             new Question { Id = 21, QuestionText = "Компанијата промовира различност и инклузија", QuestionTypeId = 1 },
             new Question { Id = 22, QuestionText = "Ги имам сите ресурси потребни за ефикасно извршување на моите задачи", QuestionTypeId = 1 },
             new Question { Id = 23, QuestionText = "Физичката работна средина е удобна и поволна за продуктивност", QuestionTypeId = 1 },
             new Question { Id = 24, QuestionText = "Се чувствувам безбедно на работа", QuestionTypeId = 1 },
             new Question { Id = 25, QuestionText = "Задоволен сум од мојот пакет компензации и бенефиции", QuestionTypeId = 1 },
             new Question { Id = 26, QuestionText = "Моите напори и достигнувања се препознаени и ценети", QuestionTypeId = 1 },
             new Question { Id = 27, QuestionText = "Постојат јасни можности за напредување во кариерата во рамките на компанијата", QuestionTypeId = 1 },
             new Question { Id = 28, QuestionText = "Компанијата ги поттикнува иновациите и креативното размислување", QuestionTypeId = 1 },
             new Question { Id = 29, QuestionText = "Подготвен сум да ги усвојам промените имплементирани во компанијата", QuestionTypeId = 1 },
             new Question { Id = 30, QuestionText = "Идеите и предлозите на вработените се разгледуваат и се спроведуваат кога е соодветно", QuestionTypeId = 1 },
             new Question { Id = 31, QuestionText = "Kолку е веројатно да ја препорачате оваа компанија како работно место на пријател или колега", QuestionTypeId = 1 },
             new Question { Id = 32, QuestionText = "Што најмногу ви се допаѓа на вашето сегашно работно место?", QuestionTypeId = 2 },
             new Question { Id = 33, QuestionText = "Кои се најголемите предизвици со кои се соочувате на работа?", QuestionTypeId = 2 },
             new Question { Id = 34, QuestionText = "Какви предлози имате за подобрување на работната средина или процесите на компанијата?", QuestionTypeId = 2 },
             new Question { Id = 35, QuestionText = "разно", QuestionTypeId = 2 }

         );
         return modelBuilder;
     }
     public static ModelBuilder SeedQuestionForm(this ModelBuilder modelBuilder)
     {
         modelBuilder.Entity<QuestionForm>().HasData(
             new QuestionForm { Id = 1, Title = "Општо задоволство" },
             new QuestionForm { Id = 2, Title = "Обврска кон компанијата" },
             new QuestionForm { Id = 3, Title = "Професионален развој" },
             new QuestionForm { Id = 4, Title = "Рамнотежа помеѓу работата и животот" },
             new QuestionForm { Id = 5, Title = "Комуникација и соработка" },
             new QuestionForm { Id = 6, Title = "Лидерство" },
             new QuestionForm { Id = 7, Title = "Организациска култура" },
             new QuestionForm { Id = 8, Title = "Работна средина" },
             new QuestionForm { Id = 9, Title = "Награди и признанија" },
             new QuestionForm { Id = 10, Title = "Иновации и промени" },
             new QuestionForm { Id = 11, Title = "Конечна евалуација" },
             new QuestionForm { Id = 12, Title = "Отворени прашања" }
         );
         return modelBuilder;
     }
 
 }
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=AnketaDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.SeedUsers()
                    .SeedRoles()
                    .SeedQuestionType()
                    .SeedQuestion()
                    .SeedQuestionForm();
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionForm> QuestionForms { get; set; }
    public DbSet<Answer> Answers { get; set; }
}
public static class InjectionExtensions
{
    public static void InjectDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
    }
}

