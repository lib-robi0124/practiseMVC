using Microsoft.EntityFrameworkCore;
using VideoMovieRent.Domain;
using VideoMovieRent.Domain.Enums;

namespace VideoMovieRent.DataAccess
{
    public class VideoMovieRentDbContext : DbContext
    {
        public VideoMovieRentDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Admin> Admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                 .HasData(
                 new User { Id = 1, FullName = "Emma Watson", Age = 25, CardNumber = "1111-2222", CreatedOn = DateTime.Now.AddMonths(-6), IsSubscriptionExpired = false, SubscriptionType = SubscriptionType.Monthly },
                 new User { Id = 2, FullName = "James Wilson", Age = 30, CardNumber = "2222-3333", CreatedOn = DateTime.Now.AddMonths(-8), IsSubscriptionExpired = true, SubscriptionType = SubscriptionType.Yearly },
                 new User { Id = 3, FullName = "Olivia Martinez", Age = 22, CardNumber = "3333-4444", CreatedOn = DateTime.Now.AddYears(-1), IsSubscriptionExpired = false, SubscriptionType = SubscriptionType.Monthly },
                 new User { Id = 4, FullName = "Liam Anderson", Age = 28, CardNumber = "4444-5555", CreatedOn = DateTime.Now.AddMonths(-3), IsSubscriptionExpired = false, SubscriptionType = SubscriptionType.Yearly },
                 new User { Id = 5, FullName = "Sophia Garcia", Age = 35, CardNumber = "5555-6666", CreatedOn = DateTime.Now.AddYears(-2), IsSubscriptionExpired = true, SubscriptionType = SubscriptionType.Monthly },
                 new User { Id = 6, FullName = "Noah Taylor", Age = 40, CardNumber = "6666-7777", CreatedOn = DateTime.Now.AddMonths(-10), IsSubscriptionExpired = false, SubscriptionType = SubscriptionType.Yearly },
                 new User { Id = 7, FullName = "Ava Lopez", Age = 29, CardNumber = "7777-8888", CreatedOn = DateTime.Now.AddMonths(-5), IsSubscriptionExpired = false, SubscriptionType = SubscriptionType.Monthly },
                 new User { Id = 8, FullName = "William Hernandez", Age = 33, CardNumber = "8888-9999", CreatedOn = DateTime.Now.AddMonths(-1), IsSubscriptionExpired = true, SubscriptionType = SubscriptionType.Yearly },
                 new User { Id = 9, FullName = "Mia Gonzalez", Age = 21, CardNumber = "9999-0000", CreatedOn = DateTime.Now.AddMonths(-7), IsSubscriptionExpired = false, SubscriptionType = SubscriptionType.Monthly },
                 new User { Id = 10, FullName = "Benjamin Clark", Age = 27, CardNumber = "0000-1111", CreatedOn = DateTime.Now.AddMonths(-4), IsSubscriptionExpired = true, SubscriptionType = SubscriptionType.Yearly });

            modelBuilder.Entity<Movie>()
                .HasData(
                new Movie { Id = 1, Title = "Neon Shadows", Genre = Genre.SciFi, Language = Language.Japanese, IsAvailable = true, ReleaseDate = new DateTime(2012, 10, 23), Length = new TimeSpan(2, 23, 0), AgeRestriction = 13, Quantity = 5, ImagePath = "/images/movie/NeonShados.jpg" },
                new Movie { Id = 2, Title = "The Silent Forest", Genre = Genre.Thriller, Language = Language.Korean, IsAvailable = true, ReleaseDate = new DateTime(2019, 5, 30), Length = new TimeSpan(2, 12, 0), AgeRestriction = 18, Quantity = 3, ImagePath = "/images/movie/TheSilentForest.jpg" },
                new Movie { Id = 3, Title = "Quantum Paradox", Genre = Genre.SciFi, Language = Language.English, IsAvailable = false, ReleaseDate = new DateTime(2012, 4, 11), Length = new TimeSpan(2, 23, 0), AgeRestriction = 13, Quantity = 0, ImagePath = "/images/movie/Paradox.jpg" },
                new Movie { Id = 4, Title = "Celestial Voyage", Genre = Genre.Adventure, Language = Language.French, IsAvailable = true, ReleaseDate = new DateTime(2014, 11, 7), Length = new TimeSpan(2, 49, 0), AgeRestriction = 10, Quantity = 4, ImagePath = "/images/movie/voyage.jpg" },
                new Movie { Id = 5, Title = "Whispers in the Dark", Genre = Genre.Horror, Language = Language.Spanish, IsAvailable = true, ReleaseDate = new DateTime(1980, 5, 23), Length = new TimeSpan(2, 26, 0), AgeRestriction = 18, Quantity = 2, ImagePath = "/images/movie/whispers.jpg" },
                new Movie { Id = 6, Title = "Midnight in Paris", Genre = Genre.Romance, Language = Language.French, IsAvailable = true, ReleaseDate = new DateTime(2001, 4, 25), Length = new TimeSpan(2, 2, 0), AgeRestriction = 12, Quantity = 3, ImagePath = "/images/movie/paris.jpg" },
                new Movie { Id = 7, Title = "The Forgotten Letter", Genre = Genre.Mystery, Language = Language.German, IsAvailable = false, ReleaseDate = new DateTime(1997, 12, 20), Length = new TimeSpan(1, 56, 0), AgeRestriction = 13, Quantity = 0, ImagePath = "/images/movie/letters.jpg" },
                new Movie { Id = 8, Title = "Crimson Peak", Genre = Genre.Horror, Language = Language.English, IsAvailable = true, ReleaseDate = new DateTime(2017, 2, 24), Length = new TimeSpan(1, 44, 0), AgeRestriction = 17, Quantity = 6, ImagePath = "/images/movie/peak.jpg" },
                new Movie { Id = 9, Title = "Rogue Agent", Genre = Genre.Action, Language = Language.Mandarin, IsAvailable = true, ReleaseDate = new DateTime(2010, 7, 16), Length = new TimeSpan(2, 28, 0), AgeRestriction = 13, Quantity = 7, ImagePath = "/images/movie/agent.jpg" },
                new Movie { Id = 10, Title = "The Alchemist's Daughter", Genre = Genre.Fantasy, Language = Language.Spanish, IsAvailable = true, ReleaseDate = new DateTime(2006, 10, 11), Length = new TimeSpan(1, 58, 0), AgeRestriction = 15, Quantity = 2, ImagePath = "/images/movie/daugther.jpg" });

            modelBuilder.Entity<Rental>()
                .HasData(
                new Rental { Id = 1, MovieId = 1, UserId = 1, RentedOn = DateTime.Now.AddDays(-10), ReturnedOn = DateTime.Now.AddDays(-5), Title = "Neon Shadows"},
                new Rental { Id = 2, MovieId = 2, UserId = 2, RentedOn = DateTime.Now.AddDays(-8), ReturnedOn = DateTime.Now.AddDays(-2), Title = "The Silent Forest"},
                new Rental { Id = 3, MovieId = 3, UserId = 3, RentedOn = DateTime.Now.AddDays(-7), ReturnedOn = DateTime.Now.AddDays(-1), Title = "Quantum Paradox"},
                new Rental { Id = 4, MovieId = 4, UserId = 4, RentedOn = DateTime.Now.AddDays(-5), ReturnedOn = DateTime.Now, Title = "Celestial Voyage"},
                new Rental { Id = 5, MovieId = 5, UserId = 5, RentedOn = DateTime.Now.AddDays(-12), ReturnedOn = DateTime.Now.AddDays(-7), Title = "Whispers in the Dark" },
                new Rental { Id = 6, MovieId = 6, UserId = 6, RentedOn = DateTime.Now.AddDays(-3), ReturnedOn = DateTime.Now, Title = "Midnight in Paris" },
                new Rental { Id = 7, MovieId = 7, UserId = 7, RentedOn = DateTime.Now.AddDays(-4), ReturnedOn = DateTime.Now.AddDays(-1), Title = "The Forgotten Letter" },
                new Rental { Id = 8, MovieId = 8, UserId = 8, RentedOn = DateTime.Now.AddDays(-2), ReturnedOn = DateTime.Now, Title = "Crimson Peak" },
                new Rental { Id = 9, MovieId = 9, UserId = 9, RentedOn = DateTime.Now.AddDays(-6), ReturnedOn = DateTime.Now.AddDays(-2), Title = "Rogue Agent" },
                new Rental { Id = 10, MovieId = 10, UserId = 10, RentedOn = DateTime.Now.AddDays(-1), ReturnedOn = DateTime.Now, Title = "The Alchemist's Daughter" });

            modelBuilder.Entity<Cast>()
                .HasData(
                new Cast { Id = 1, MovieId = 1, Name = "Ken Watanabe", Part = Part.Actor },
                new Cast { Id = 2, MovieId = 2, Name = "Park Chan-wook", Part = Part.Director },
                new Cast { Id = 3, MovieId = 2, Name = "Lee Byung-hun", Part = Part.Actor },
                new Cast { Id = 4, MovieId = 3, Name = "Michael B. Jordan", Part = Part.Camera },
                new Cast { Id = 5, MovieId = 4, Name = "Marion Cotillard", Part = Part.Actor },
                new Cast { Id = 6, MovieId = 7, Name = "Tom Hardy", Part = Part.Actor },
                new Cast { Id = 7, MovieId = 8, Name = "Jessica Chastain", Part = Part.Director },
                new Cast { Id = 8, MovieId = 9, Name = "Donnie Yen", Part = Part.Actor },
                new Cast { Id = 9, MovieId = 3, Name = "Zoe Saldana", Part = Part.Camera },
                new Cast { Id = 10, MovieId = 6, Name = "Owen Wilson", Part = Part.Actor });

            modelBuilder.Entity<Admin>()
                .HasData(
                new Admin { Id = 1, Username = "peroperov", Password = "12233445" },
                new Admin { Id = 2, Username = "admin", Password = "admin123" }
                );
        }

    }
}
    
