namespace Lamazon.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}
namespace Lamazon.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleKey { get; set; }
        public string PasswordHash { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public User()
        {
            Orders = new HashSet<Order>();
            Invoices = new HashSet<Invoice>();
        }
    }
}
namespace Lamazon.Domain.Entities
{
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
}
namespace Lamazon.Domain.Entities
{
    public class ProductStatus : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public ProductStatus()
        {
            Products = new HashSet<Product>();
        }
    
    }
}
namespace Lamazon.Domain.Entities
{
    public class ProductCategoryStatus : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public ProductCategoryStatus()
        {
            ProductCategories = new HashSet<ProductCategory>();
        }
    }
}
namespace Lamazon.Domain.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; }
        public int ProductCategoryStatusId { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ProductCategoryStatus ProductCategoryStatus { get; set; }
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }
    }
}
namespace Lamazon.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int ProductCategoryId { get; set; }
        public decimal Price { get; set; }
        public int ProductStatusId { get; set; }
        public int DiscountPercentage { get; set; }
        public bool IsFeatured { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; }
        public virtual ICollection<OrderLineItem> OrderLineItems { get; set; }
        public Product()
        {
            InvoiceLineItems = new HashSet<InvoiceLineItem>();
            OrderLineItems = new HashSet<OrderLineItem>();
        }


    }
}
namespace Lamazon.Domain.Entities
{
    public class OrderStatus : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }
    }
}
namespace Lamazon.Domain.Entities
{
    public class OrderLineItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int DiscountProcentage { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; }
        public OrderLineItem()
        {
            InvoiceLineItems = new HashSet<InvoiceLineItem>();
        }
    }
}
namespace Lamazon.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
        public int OrderStatusId { get; set; }
        public string? IpAddress { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryFlagUrl { get; set; }
        public virtual User User { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<OrderLineItem> OrderLineItems { get; set; }
        public Order()
        {
            Invoices = new HashSet<Invoice>();
            OrderLineItems = new HashSet<OrderLineItem>();
        }

    }
}
namespace Lamazon.Domain.Entities
{
    public class InvoiceStatus : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public InvoiceStatus()
        {
            Invoices = new HashSet<Invoice>();
        }
    }
}
namespace Lamazon.Domain.Entities
{
    public class InvoiceLineItem : BaseEntity
    {
        public int InvoiceId { get; set; }
        public int OrderLineItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int DiscountProcentage { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual OrderLineItem OrderLineItem { get; set; }
        public virtual Product Product { get; set; }
    }
}
namespace Lamazon.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int InvoiceStatusId { get; set; }
        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
        public virtual InvoiceStatus InvoiceStatus { get; set; }
        public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; }
        public Invoice()
        {
            InvoiceLineItems = new HashSet<InvoiceLineItem>();
        }
    }
}
namespace Lamazon.Domain.Enums
{
    public enum ProductStatusEnum
    {
        Active = 1,
        Deleted = 255
    }
}
namespace Lamazon.DataAccess.DataContext
{
    public class ApplicationDbContext : DbContext //IdentityDbContext<User, Role, int> usually for Identity
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=LamazonDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder
                 .SeedProductCategoryStatus()
                 .SeedProductStatus()
                 .SeedProductCategory()
                 .SeedProducts()
                 .SeedRoles()
                 .SeedUsers();

        }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
        public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLineItem> OrderLineItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductCategoryStatus> ProductCategoryStatuses { get; set; }
        public DbSet<ProductStatus> ProductStatuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(100);

            builder.HasIndex(x => x.Email).IsUnique(); //email da bide unikaten
            //foreign key to UserRole
            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleKey)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise role, da ne se izbriseat i user-ite
                .HasConstraintName("FK_User_UserRole");

        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Key);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class ProductStatusConfig : IEntityTypeConfiguration<ProductStatus>
    {
        public void Configure(EntityTypeBuilder<ProductStatus> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).IsRequired().HasPrecision(10, 2);
            builder.Property(x => x.ImageUrl).IsRequired();
            builder.Property(x => x.IsFeatured).HasDefaultValue(false);
            builder.Property(x => x.DiscountPercentage).HasDefaultValue(0);
            //foreign key to Category
            builder.HasOne(x => x.ProductStatus)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductStatusId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise category, da ne se izbriseat i product-ite
                .HasConstraintName("FK_Product_Category");
            //foreign key to ProductCategory
            builder.HasOne(x => x.ProductCategory)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductCategoryId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise category, da ne se izbriseat i product-ite
                .HasConstraintName("FK_Product_ProductCategory");
        }

    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class ProductCategoryStatusConfig : IEntityTypeConfiguration<ProductCategoryStatus>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryStatus> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.HasOne(x => x.ProductCategoryStatus)
                .WithMany(x => x.ProductCategories)
                .HasForeignKey(x => x.ProductCategoryStatusId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise parent category, da ne se izbriseat i subcategory-ite
                .HasConstraintName("FK_ProductCategory_ProductCategoryStatus");
        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class OrderStatusConfig : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class OrderLineItemConfig : IEntityTypeConfiguration<OrderLineItem>
    {
        public void Configure(EntityTypeBuilder<OrderLineItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.ProductPrice).IsRequired().HasPrecision(10, 2);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.TotalPrice).IsRequired().HasPrecision(10, 2);
            //foreign key to Order
            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderLineItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise order, da se ne izbriseat i line item-ite
                .HasConstraintName("FK_OrderLineItem_Order");
            //foreign key to Product
            builder.HasOne(x => x.Product)
                .WithMany(x => x.OrderLineItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise product, da ne se izbriseat i line item-ite
                .HasConstraintName("FK_OrderLineItem_Product");
        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(50);
            builder.Property(x => x.CountryCode).HasMaxLength(5);
            builder.Property(x => x.IpAddress).HasMaxLength(50);
            builder.Property(x => x.TotalAmount).IsRequired().HasPrecision(10, 2);
            //foreign key to OrderStatus
            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise user, da ne se izbriseat i order-ite
                .HasConstraintName("FK_Order_User");
            //foreign key to OrderStatus
            builder.HasOne(x => x.OrderStatus)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.OrderStatusId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise status, da ne se izbriseat i order-ite
                .HasConstraintName("FK_Order_OrderStatus");
        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class InvoiceStatusConfig : IEntityTypeConfiguration<InvoiceStatus>
    {
        public void Configure(EntityTypeBuilder<InvoiceStatus> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class InvoiceLineItemConfig : IEntityTypeConfiguration<InvoiceLineItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.ProductPrice).IsRequired().HasPrecision(10, 2);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.TotalPrice).IsRequired().HasPrecision(10, 2);

            builder.HasOne(x => x.OrderLineItem)
                .WithMany(x => x.InvoiceLineItems)
                .HasForeignKey(x => x.OrderLineItemId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise order line item, da ne se izbrise i invoice line items
                .HasConstraintName("FK_InvoiceLineItem_OrderLineItem");
            builder.HasOne(x => x.Invoice)
                .WithMany(x => x.InvoiceLineItems)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise invoice, da ne se izbrise i line items
                .HasConstraintName("FK_InvoiceLineItem_Invoice");
            builder.HasOne(x => x.Product)
                .WithMany(x => x.InvoiceLineItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise product, da ne se izbrise i line items
                .HasConstraintName("FK_InvoiceLineItem_Product");
        }
    }
}
namespace Lamazon.DataAccess.EntitiesConfig
{
    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(50);

            builder.Property(x => x.TotalAmount).IsRequired().HasPrecision(10, 2);

            builder.HasOne(x => x.User)
                .WithMany(u => u.Invoices)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise invoice, da ne se izbrise i userot
                .HasConstraintName("FK_Invoice_User");

            builder.HasOne(x => x.Order)
                .WithMany(x => x.Invoices)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise invoice, da ne se izbrise i orderot
                .HasConstraintName("FK_Invoice_Order");

            builder.HasOne(x => x.InvoiceStatus)
                .WithMany(x => x.Invoices)
                .HasForeignKey(x => x.InvoiceStatusId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise invoice, da ne se izbrise i statusot
                .HasConstraintName("FK_Invoice_InvoiceStatus");
        }
    }
}
namespace Lamazon.DataAccess.DataContext
{
    public static class DataSeedExtension
    {
        public static ModelBuilder SeedProductCategoryStatus(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategoryStatus>().HasData(
                new ProductCategoryStatus { Id = 1, Name = "Active" },
                new ProductCategoryStatus { Id = 255, Name = "Deleted" }
            );
            return modelBuilder;
        }
        public static ModelBuilder SeedProductStatus(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductStatus>().HasData(
                new ProductStatus { Id = 1, Name = "Active" },
                new ProductStatus { Id = 255, Name = "Deleted" }
            );
            return modelBuilder;
        }
        public static ModelBuilder SeedProductCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { Id = 1, Name = "Foods", ProductCategoryStatusId = 1 },
                new ProductCategory { Id = 2, Name = "Drinks", ProductCategoryStatusId = 1 },
                new ProductCategory { Id = 3, Name = "Books", ProductCategoryStatusId = 1 },
                new ProductCategory { Id = 4, Name = "Software", ProductCategoryStatusId = 1 },
                new ProductCategory { Id = 5, Name = "Computers", ProductCategoryStatusId = 1 }
            );
            return modelBuilder;
        }
        public static ModelBuilder SeedProducts(this ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<Product>().HasData(
                // Books
                new Product
                {
                    Id = 1,
                    Name = "Clean Code",
                    Description = "Even bad code can function. But if code isn’t clean, it can bring a development organization to its knees. Every year, countless hours and significant resources are lost because of poorly written code. But it doesn’t have to be that way.\r\n\r\nNoted software expert Robert C. Martin, presents a revolutionary paradigm with Clean Code: A Handbook of Agile Software Craftsmanship. Martin, who has helped bring agile principles from a practitioner’s point of view to tens of thousands of programmers, has teamed up with his colleagues from Object Mentor to distill their best agile practice of cleaning code “on the fly” into a book that will instill within you the values of software craftsman, and make you a better programmer―but only if you work at it.\r\n\r\nWhat kind of work will you be doing? You’ll be reading code―lots of code. And you will be challenged to think about what’s right about that code, and what’s wrong with it. More importantly you will be challenged to reassess your professional values and your commitment to your craft.\r\n\r\nClean Code is divided into three parts. The first describes the principles, patterns, and practices of writing clean code. The second part consists of several case studies of increasing complexity. Each case study is an exercise in cleaning up code―of transforming a code base that has some problems into one that is sound and efficient. The third part is the payoff: a single chapter containing a list of heuristics and “smells” gathered while creating the case studies. The result is a knowledge base that describes the way we think when we write, read, and clean code.\r\n",
                    ImageUrl = "https://m.media-amazon.com/images/I/51E2055ZGUL._SY522_.jpg",
                    ProductCategoryId = 3, // Books
                    ProductStatusId = 1, // Active
                    Price = 55m,
                    IsFeatured = true,
                    DiscountPercentage = 10
                },
                new Product
                {
                    Id = 2,
                    Name = "The Pragmatic Programmer",
                    Description = "Ward Cunningham Straight from the programming trenches, The Pragmatic Programmer cuts through the increasing specialization and technicalities of modern software development to examine the core process--taking a requirement and producing working, maintainable code that delights its users. It covers topics ranging from personal responsibility and career development to architectural techniques for keeping your code flexible and easy to adapt and reuse. Read this book, and you’ll learn how to Fight software rot; Avoid the trap of duplicating knowledge; Write flexible, dynamic, and adaptable code; Avoid programming by coincidence; Bullet-proof your code with contracts, assertions, and exceptions; Capture real requirements; Test ruthlessly and effectively; Delight your users; Build teams of pragmatic programmers; and Make your developments more precise with automation.",
                    ImageUrl = "https://m.media-amazon.com/images/I/61ztlXgCmpL._SY522_.jpg",
                    ProductCategoryId = 3,
                    ProductStatusId = 1,
                    Price = 50.99m,
                    IsFeatured = false,
                    DiscountPercentage = 15
                },
                new Product
                {
                    Id = 3,
                    Name = "Introduction to Algorithms",
                    Description = "A comprehensive update of the leading algorithms text, with new material on matchings in bipartite graphs, online algorithms, machine learning, and other topics.",
                    ImageUrl = "https://m.media-amazon.com/images/I/61Mw06x2XcL._AC_UY327_FMwebp_QL65_.jpg",
                    ProductCategoryId = 3,
                    ProductStatusId = 1,
                    Price = 60.99m,
                    IsFeatured = false,
                    DiscountPercentage = 0
                },
                // Software
                new Product
                {
                    Id = 4,
                    Name = "Windоws 11 Home",
                    Description = "OEM IS TO BE INSTALLED ON A NEW PC with no prior version of Windows installed and cannot be transferred to another machine.",
                    ImageUrl = "https://m.media-amazon.com/images/I/61JfosHunyL._AC_SX679_.jpg",
                    ProductCategoryId = 4,
                    ProductStatusId = 1,
                    Price = 119.99m,
                    IsFeatured = false,
                    DiscountPercentage = 0
                },
                // Computers
                new Product
                {
                    Id = 5,
                    Name = "Lenovo ThinkPad T14s Gen 6-2024",
                    Description = "AHEAD OF THE PACK- Breathtakingly light and insanely powerful, the ThinkPad T14s Gen 6 sets the new industry-leading standard for AI PCs.\r\nBETTER THAN THE BEST - The Snapdragon X Elite processor is engineered to fulfill the needs of today’s IT teams and hybrid workforce. Meet one of the most powerful, intuitive, and efficient Windows platforms.\r\nGLITTERING GRAPHICS - Enjoy vibrant visuals and sharp details in the sunniest of spots with the Eyesafe Certified, anti-glare WUXGA IPS panel. The energy-efficient display delivers vivid hues and stark contrasts.\r\nSEAMLESS CONNECTION - For relentless on-the-go productivity, Qualcomm Wi-Fi 7 ensures faster bandwidth usage.",
                    ImageUrl = "https://m.media-amazon.com/images/I/71EYq-mmleL._AC_SX679_.jpg",
                    ProductCategoryId = 5,
                    ProductStatusId = 1,
                    Price = 1711.99m,
                    IsFeatured = true,
                    DiscountPercentage = 0
                },
                new Product
                {
                    Id = 6,
                    Name = "ASUS Chromebook Flip CX1 Convertible Laptop, 14\" FHD",
                    Description = "SMOOTH MULTITASKING — Powered by the Intel Pentium N6000 4-Core Processor, enabling decent multitasking experience\r\nTOUCHSCREEN VERSATILITY — 14-inch FHD 1920x1080 NanoEdge 360-degree flippable touchscreen display\r\nWORK AND PLAY FROM ANY ANGLE — Convertible 2-in-1 design with four different work modes: traditional clamshell, tent, stand, tablet mode\r\nLIGHTWEIGHT YET DURABLE — Durable and built to US Military Grade standard MIL- STD 810H weighing just 3.59 lbs",
                    ImageUrl = "https://m.media-amazon.com/images/I/61UucaPmVtL._AC_SX679_.jpg",
                    ProductCategoryId = 5,
                    ProductStatusId = 1,
                    Price = 350,
                    IsFeatured = false,
                    DiscountPercentage = 7
                }
            );
            return modelBuilder;
        }
        #region Users/Roles Seed
        // To be implemented in the future
        public static ModelBuilder SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Key = "admin", Name = "Administrator" },
                new Role { Key = "user", Name = "User" }
            );

            return modelBuilder;
        }
        public static ModelBuilder SeedUsers(this ModelBuilder modelBuilder)
        {
            // Passwords should be hashed and salted in a real application
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Admin User",
                    RoleKey = "admin",
                    Email = "admin@admin.com",
                    PasswordHash = "AQAAAAEAACcQAAAAECJCSH7Y7+DSAD+UKEnb6fjgOROzppnUpop5/kVMcBDjzOVaLz0vts978iw4ooBhhQ==" // (Admin123)Placeholder for hashed password
                },
                new User
                {
                    Id = 2,
                    FullName = "User",
                    RoleKey = "user",
                    Email = "user@user.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEH2PV/R1HciXgHqwrYcEp/32IrxaQ44wcbBnM6EHK2FXA5wZRYXN6pddtVKNqTpTxg=="
                }); // (User123) Placeholder for hashed password

            return modelBuilder;
        }
        #endregion
    }
}
namespace Lamazon.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByEmail(string email);
        int Insert(User user);
    }
}
namespace Lamazon.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        List<Product> GetAllFeaturedProducts();
        Product GetById(int id);

    }
}
namespace Lamazon.DataAccess.Implementacija
{
    public class BaseRepository
    {
        protected readonly ApplicationDbContext _applicationDbContext;
        protected BaseRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
namespace Lamazon.DataAccess.Implementacija
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public User GetByEmail(string email)
        {
            return _applicationDbContext.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Email.ToLower() == email.ToLower());

        }

        public User GetById(int id)
        {
            return _applicationDbContext.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Id == id);
        }

        public int Insert(User user)
        {
            _applicationDbContext.Users.Add(user);
            _applicationDbContext.SaveChanges();
            return user.Id;
        }
    }
}
namespace Lamazon.DataAccess.Implementacija
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public List<Product> GetAllFeaturedProducts()
        {
            return _applicationDbContext.Products.Include(x => x.ProductCategory)
                .Where(x => x.IsFeatured && x.ProductStatusId != (int)ProductStatusEnum.Deleted).ToList();
        }

        public List<Product> GetAll()
        {
            return _applicationDbContext.Products.Include(x => x.ProductCategory)
                .Where(x => x.ProductStatusId != (int)ProductStatusEnum.Deleted).ToList();
        }

        public Product GetById(int id)
        {
            return _applicationDbContext.Products.Include(x => x.ProductCategory)
                .FirstOrDefault(x => x.Id == id & x.ProductStatusId != (int)ProductStatusEnum.Deleted);
        }
    }
}
//kreirame proekt ViewModels / folderi /Models / Enums
namespace Lamazon.ViewModels.Enums
{
    public enum ProductStatusEnum
    {
        Active = 1,
        Deleted = 255
    }
}
namespace Lamazon.ViewModels.Enums
{
    public enum ProductCategoryStatusEnum
    {
        Active = 1,
        Deleted = 255
    }
}
namespace Lamazon.ViewModels.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleKey { get; set; }
        public RoleViewModel Roles { get; set; }
    }
}
namespace Lamazon.ViewModels.Models
{
    public class UserCredentialsViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
namespace Lamazon.ViewModels.Models
{
    public class RoleViewModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
namespace Lamazon.ViewModels.Models
{
    public class RegisterUserViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
namespace Lamazon.ViewModels.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Info { get; set; }
        public string ImageUrl { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductStatusEnum ProductStatus { get; set; }
        public decimal Price { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsAddedToCart { get; set; }
        public int DiscountPercentage { get; set; }
        public decimal DiscountedPrice { get { return Math.Round((1 - (decimal)DiscountPercentage / 100) * Price, decimals: 2); } }

    }
}
namespace Lamazon.ViewModels.Models
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductCategoryStatusEnum ProductCategoryStatus { get; set; }
    }
}
//next e mapiranje na Domain / Models with ViewModels / Models
//mapiranje e na nivo proekt Services, moze so helper vo nov proekt Mapping ili so Nuget paket AutoMapper
namespace Lamazon.Services.AutoMapperProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<RegisterUserViewModel, User>().ReverseMap();
        }

    }
}
namespace Lamazon.Services.AutoMapperProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            // Define your mappings here
            CreateMap<Role, RoleViewModel>().ReverseMap();

        }
    }
}
namespace Lamazon.Services.AutoMapperProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.ProductStatus, opt => opt.Ignore())
                .ForMember(x => x.ProductStatus, opt => opt.MapFrom(s => s.ProductCategoryId))
                .ForMember(x => x.Info, opt =>
                opt.MapFrom(s => $"{s.Id.ToString("000")} - {s.Name} ({s.ProductCategory.Name})"))
                .ReverseMap()
                .ForMember(x => x.ProductStatus, opt => opt.Ignore())
                .ForMember(x => x.ProductStatusId, opt => opt.MapFrom(s => s.ProductStatus));
        }
    }
}
namespace Lamazon.Services.AutoMapperProfiles
{
    public class ProductCategorMappingProfile : Profile
    {
        public ProductCategorMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>()
                .ForMember(x => x.ProductCategoryStatus, opt => opt.Ignore())
                .ForMember(x => x.ProductCategoryStatus, opt => opt.MapFrom(s => s.ProductCategoryStatusId))
                .ReverseMap()
                .ForMember(x => x.ProductCategoryStatus, opt => opt.Ignore())
                .ForMember(x => x.ProductCategoryStatusId, opt => opt.MapFrom(s => s.ProductCategoryStatus));
        }
    }
}
// to project Services add new folder Interfaces / Implementations
namespace Lamazon.Services.Interfaces
{
    public interface IUserService
    {
        UserViewModel RegisterUser(RegisterUserViewModel registerUserViewModel);

        UserViewModel GetUserById(int id);

        UserViewModel ValidateUser(UserCredentialsViewModel userCredentialsViewModel);
    }
}
//site products, nema da vraka product od entitet tuku od viewmodels
namespace Lamazon.Services.Interfaces
{
    public interface IProductService
    {
        List<ProductViewModel> GetAllProducts();
        List<ProductViewModel> GetAllFeaturedProducts();
        ProductViewModel GetProductById(int id);
    }
}
namespace Lamazon.Services.Implementations
{
    public class ProductService : IProductService
    {
        //inject repository and automapper

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public List<ProductViewModel> GetAllFeaturedProducts()
        {
            var featuredProducts = _productRepository.GetAllFeaturedProducts();
            var mappedProducts = _mapper.Map<List<ProductViewModel>>(featuredProducts);
            return mappedProducts;
        }

        public List<ProductViewModel> GetAllProducts()
        {
            var products = _productRepository.GetAll();
            var mappedProducts = _mapper.Map<List<ProductViewModel>>(products);
            return mappedProducts;
        }

        public ProductViewModel GetProductById(int id)
        {
            var product = _productRepository.GetById(id);
            return _mapper.Map<ProductViewModel>(product);
        }
    }
}
namespace Lamazon.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public UserViewModel GetUserById(int id)
        {
            var user = _userRepository.GetById(id);

            return _mapper.Map<UserViewModel>(user);
        }

        public UserViewModel RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            var user = _mapper.Map<User>(registerUserViewModel);
            PasswordHasherHelper.HashPassword(user, registerUserViewModel.Password);
            user.RoleKey = Roles.User;
            var userId = _userRepository.Insert(user);
            if (userId <= 0)
            {
                throw new Exception("Error registering user");
            }
            return GetUserById(userId);
        }

        public UserViewModel ValidateUser(UserCredentialsViewModel userCredentialsViewModel)
        {
            var user = _userRepository.GetByEmail(userCredentialsViewModel.Email);
            if (user is null)
            {
                return null;
            }
            var result = PasswordHasherHelper.VerifyHashedPassword(user, userCredentialsViewModel.Password);
            if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
            {
                return new UserViewModel();
            }
            return _mapper.Map<UserViewModel>(user);
        }

    }
}
namespace Lamazon.Services.Extensions
{
    public static class InjectionExtensions
    {
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            // Here you can add your service injections, for example:
            // services.AddScoped<IYourService, YourServiceImplementation>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
        public static void InjectRepositories(this IServiceCollection services)
        {
            // Here you can add your repository injections, for example:
            // services.AddScoped<IYourRepository, YourRepositoryImplementation>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
        public static void InjectServices(this IServiceCollection services)
        {
            // Here you can add your service injections, for example:
            // services.AddScoped<IYourService, YourServiceImplementation>();
            // services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
        }
        public static void InjectAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
// now we are creating Views cshtml in Main App
//in Shared folder Layout and partial views
<footer class="border-top footer text-muted">
    <div class="container">
        <nav class="float-left">
            <ul class="footer-links">
                <li>
                    <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                </li>
              
            </ul>
        </nav>
        <div class="copyrigth float-rigth">
            &copy;@DateTime.Now.Year.ToString() - Lamazon
        </ div >
</ div >

</ footer >
//Layout - no footer
<!DOCTYPE html>
<html lang="en">

    <head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>@ViewData["Title"] - Lamazon</title>
    <link rel="stylesheet" href="~/css/site.css" />
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons" />
    </head>
    <body>
        <header>
        @(await Html.PartialAsync("_Navbar"))
        </header>
        <div class="container">
            <main role="main" class="pb-3"> 
             @RenderBody()
            </main>
        </div>
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true" ></ script >
    @RenderSection("Scripts", required: false)
    </ body >
</ html >
//nav-bar
<nav class="navbar navbar-expand-sm navbar-ligth bg-white border-bottom box-shadow navbar-fixed mb-3 navbar-toggleable-sm">
    <div class="container">
        <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">
            <img src="~/images/lamazon.svg" class="navbar-logo" alt="Lamazon" style="height:30px;" />
        </a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" 
        aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
          <div class="collapse navbar-collapse justify-content-between">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-controller="Products" asp-action="Index">Products</a>
                </li>
            </ul>
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a class="nav-link notification-container">
                        <i class="material-icons">shopping_cart</i>
                    </a>
                </li>
                @if(User.Identity.IsAuthenticated)
                {
                    <li class="nav-item dropdown">
                        <a class="nav-link header-nav" href="javascript:;" data-bs-toggle="dropdown" ></a>
                     
                        <i class="fas fa-user mr-1"></i>@User.DisplayName()
                        <div class="dropdown-menu dropdown-menu-right">
                            @if(User.IsAdmin())
                            {
                                <a class="dropdown-item" >Lamazon CMS</a>
                            }
                      <a class="dropdown-item" asp-area="" asp-controller="Users" asp-action="Logout">Logout</a>
                        </div>
                    </li>
                }
                else
                {
                    <li class="nav-item">
                        <a class="nav-link text-dark" asp-controller="Users" asp-action="Login">Login</a>
                    </li>
                }
            </ ul >
        </ div >
    </ div >
</ nav >
//add @usings to ViewImports.cshtml
@using Lamazon.Web
@using Lamazon.Web.Models
@using Lamazon.ViewModels.Models
@using Lamazon.ViewModels.Enums
@using Lamazon.Web.Extensions
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
----
@model ProductViewModel

<div class="col-md-4">
    <div class="product-container">
        <img class="product-image" src="@Model.ImageUrl" />
        <div class="product-content">
            <a asp-controller="Products" asp-action="ProductDetails" asp-route-id="@Model.Id">
                <p class="product-name">@Model.Name</p>
            </a>
            <p class="product-category">@Model.ProductCategory</p>
            @if(Model.DiscountedPrice != Model.Price)
            {
                <p class="product-price">
                    <del>$@Model.Price.ToString("N2")</del>
                    $@Model.DiscountedPrice.ToString("N2")
                </p>
            }
            else
            {
            <p class="product_price">
                $@Model.Price.ToString("N2")
            </p>
            }
            <div style="clear:both"></div>
            <hr />
            <div class="product-description">
                <p>@Model.Description</p>
            </div>
        </div>
            <div class="product-actions">
                @if (Model.IsAddedToCart)
                {
                    <button data-id="@Model.Id" class="btn btn-remove-from-cart btn-danger">Remove from Cart</button>
                    <button data-id="@Model.Id" class="btn btn-add-to-cart btn-success hidden">Add to Cart</button>
                }
                else
                {
                    <button data-id="@Model.Id" class="btn btn-remove-from-cart btn-danger hidden">Remove from Cart</button>
                    <button data-id="@Model.Id" class="btn btn-add-to-cart btn-success ">Add to Cart</button>
                }
            </ div >
    </ div >
</ div >
//edit view.cshtml
//add builder.Services to Programm.cs
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.InjectDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.InjectRepositories();
builder.Services.InjectServices();
builder.Services.InjectAutoMapper();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/User/Login";
        option.ExpireTimeSpan = TimeSpan.FromHours(1);
        option.SlidingExpiration = true; // Renew - reset the cookie on each request
        
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Enable authentication middleware - flow should be: UseRouting -> UseAuthentication -> UseAuthorization -> UseEndpoints
// This order is important
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
// Home / Index.cshtml
@model IEnumerable<ProductViewModel>

@{
    Layout = "~/Views/Shared/_LayoutProducts.cshtml";
}

<h2>Products</h2>

<div class="row">
    @foreach(var product in Model)
    {
        @(await Html.PartialAsync("_ProductItem", product))
    }

</ div >
// Privacy.cshtml
@{
    ViewData["Title"] = "Privacy Policy";
    Layout = "~/Views/Shared/_LayoutNoFooter.cshtml";
}
<h1>@ViewData["Title"]</h1>

<p>Use this page to detail your site's privacy policy.</p>
//views / products
@model IEnumerable<ProductViewModel>

@{
    Layout = "~/Views/Shared/_LayoutProducts.cshtml";
}

<h2>Products</h2>
<div class="row">
    @foreach(var product in Model)
    {
        @(await Html.PartialAsync("_ProductItem", product))
    }

</div>
//productDetails
@model ProductViewModel

<div class="row">
    <div class="col-md-12">
        <a asp-controller="Products" asp-action="Index">Back to products</a>
    </div>
    <hr />
    <div class="row">
        <div class="col-md-6">
            <img class="product-details-image" src="@Model.ImageUrl" />
        </div>
        <div class="col-md-6">
            <p class="product-details-name">@Model.Name</p>
            <p class="product-details-category">Category: @Model.ProductCategory.Name</p>
            <p class="product-details-price">Price: $@Model.Price</p>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <p class="product-details-description" > @Model.Description </ p >
        </ div >
    </ div >
</ div >
// next is edit HomeController
namespace Lamazon.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductService _productsService;
        public HomeController(IProductService productService)
        {
            _productsService = productService;
        }

        public IActionResult Index()
        {
            var featuredProducts = _productsService.GetAllFeaturedProducts();
            return View(featuredProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
namespace Lamazon.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            var allProducts = _productService.GetAllProducts();
            return View(allProducts);
        }
        public IActionResult ProductDetails(int? id)
        {
            if (id.HasValue)
            {
                var product = _productService.GetProductById(id.Value);
                return View(product);
            }
            return new EmptyResult();
        }
    }
}
namespace Lamazon.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) 
        { 
            _userService = userService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsViewModel userCredentialsViewModel, string returnUrl)
        {
            if(string.IsNullOrEmpty(userCredentialsViewModel.Email))
            {
                ModelState.AddModelError("UserLoginError", "Email is invalid and required");
                return View(userCredentialsViewModel);
            }
            var userValidationResult = _userService.ValidateUser(userCredentialsViewModel);
            if(userValidationResult is null)
            {
                ModelState.AddModelError("UserLoginError", "User not found");
                return View(userCredentialsViewModel);
            }
            //sign in user
            await AuthHelper.SignInUser(userValidationResult, HttpContext);
            if(string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("/");
            }
            return Redirect(returnUrl);

        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserViewModel)
        {
            
            var registeredUser = _userService.RegisterUser(registerUserViewModel);
            if(registeredUser != null)
            {
                await AuthHelper.SignInUser(registeredUser, HttpContext);
                return Redirect("/");
            }
            return View(registerUserViewModel);
        }
        public async Task<IActionResult> Logout()
        {
            await AuthHelper.SignOutUser(HttpContext);
            return Redirect("/"); //redirect to home page (root page)
        }

    }
}
