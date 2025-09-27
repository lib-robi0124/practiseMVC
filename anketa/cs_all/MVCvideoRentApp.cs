🎬 Video Movie Rental System – ASP.NET Core MVC
This project is a web-based movie rental system built with ASP.NET Core MVC and Entity Framework Core. It allows users to browse, rent, and return movies, admin to manage movie inventory, and registration new user.
Overview
This is an MVC application for a video movie rental system that allows users / admins to:
👤 User Features:
•	Browse available movies
•	Login using a card number
•	Rent available movies
•	View and return their rented movies
•	Register a new user/customer
👨‍💼 Admin Features:
•	Secure admin login with credentials
•	Create new movie entries
•	Update existing movie details
•	Delete movies from inventory
•	Manage movie images and metadata
________________________________________
🧱 Application Layers
1. Domain Layer
Contains core models and enums:
•	User, Movie, Rental, Cast, Admin
•	Shared base entity: BaseEntity (adds Id)
•	Enumerations: Genre, Language, Part, SubscriptionType
2. Database Layer
•	Uses Entity Framework Core
•	VideoMovieRentDbContext: Holds DbSet<T> for all models.
•	Seed data: Populates users, movies, casts, and rentals and admins.
3. Repository and implementation Layer
Defines and implements data access contracts:
•	IRepository<T>: Generic CRUD
•	IMovieRepository, IUserRepository, IRentalRepository, IAdminRepository
•	MovieRepository, UserRepository, RentalRepository, AdminRepository
4. Filter Layer
•	Business logic AdminAuthorizeAttribute
5. Service Layer
•	Business logic abstraction
•	Interfaces: IMovieService, IUserService, IRentalService, IAdminService
•	DTOs: MovieDto, MovieDetailsDto, RentalDto, DeletoDto, RegisterDto
6. Controller Layer
•	MovieController: Handles login, browse, rent, return, registerNewUser, view logic.
•	AdminController: Handles login, browse, create, update, delete, view logic.
7. Views
•	Razor Views for:
o	Movie - Login, Index (Movie list), Details, Return, CheckCard, Register
o	Admin - Login, Index (Movie list), Edit, Create, Delete
📁 Domain Models
BaseEntity.cs
public abstract class BaseEntity
{
    public int Id { get; set; } // Shared Id for all domain objects
}
User.cs
public class User : BaseEntity
{
    public string FullName { get; set; } = null!;
    public int Age { get; set; }
    [Required] // Ensures CardNumber is provided
    public string CardNumber { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public bool IsSubscriptionExpired { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
}
Movie.cs
public class Movie : BaseEntity
{
    public string Title { get; set; } = null!;
    public Genre Genre { get; set; }           // Enum-based genre classification
    public Language Language { get; set; }     // Enum-based language
    public bool IsAvailable { get; set; }      // Availability status
    public DateTime ReleaseDate { get; set; }
    public TimeSpan Length { get; set; }
    public int AgeRestriction { get; set; }
    public int Quantity { get; set; }
    public string? ImagePath { get; set; }     // Relative image path for UI
}
Admin.cs
public class Admin : BaseEntity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!; // Store hashed in production!
}
namespace VideoMovieRent.Domain
{
    public class Rental : BaseEntity
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public DateTime RentedOn { get; set; }
        public DateTime? ReturnedOn { get; set; } = null;
    }
}
namespace VideoMovieRent.Domain
{
    public class Cast : BaseEntity
    {
        public int MovieId { get; set; }
        public string Name { get; set; } = null!;
        public Part Part { get; set; }
    }
}
# Enum
namespace VideoMovieRent.Domain.Enums
{
    public enum Genre
    {
        Action = 1,
        Adventure = 2,
        Thriller = 3,
        Horror = 4,
        SciFi = 5,
        Romance = 6,
        Fantasy = 7,
        Mystery = 8
    }
}
namespace VideoMovieRent.Domain.Enums
{
    public enum Language
    {
        English = 1,
        Spanish = 2,
        French = 3,
        German = 4,
        Japanese = 5,
        Korean = 6,
        Mandarin = 7
    }
}
namespace VideoMovieRent.Domain.Enums
{
    public enum Part
    {
        Actor = 1,
        Director = 2,
        Camera = 3
    }
}
namespace VideoMovieRent.Domain.Enums
{
    public enum SubscriptionType
    {
        Monthly = 1,
        Yearly = 2
    }
}
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
                new Rental { Id = 1, MovieId = 1, UserId = 1, RentedOn = DateTime.Now.AddDays(-10), ReturnedOn = DateTime.Now.AddDays(-5), Title = "Neon Shadows" },
                new Rental { Id = 2, MovieId = 2, UserId = 2, RentedOn = DateTime.Now.AddDays(-8), ReturnedOn = DateTime.Now.AddDays(-2), Title = "The Silent Forest" },
                new Rental { Id = 3, MovieId = 3, UserId = 3, RentedOn = DateTime.Now.AddDays(-7), ReturnedOn = DateTime.Now.AddDays(-1), Title = "Quantum Paradox" },
                new Rental { Id = 4, MovieId = 4, UserId = 4, RentedOn = DateTime.Now.AddDays(-5), ReturnedOn = DateTime.Now, Title = "Celestial Voyage" },
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
📂 Repositories
IRepository<T>
Generic CRUD contract:
public interface IRepository<T> where T : BaseEntity
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Create(T entity);
    void Update(T entity);
    void Delete(int id);
}
IAdminRepository.cs
public interface IAdminRepository
{
    Admin? Login(string username, string password);
    void Create(Movie entity);
    void Update(Movie entity);
    void Delete(int id);
}
namespace VideoMovieRent.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        User GetUserByCardNumber(string cardNumber);
        void Login(string cardNumber);
    }
}
namespace VideoMovieRent.DataAccess.Interfaces
{
    public interface IRentalRepository
    {
        IEnumerable<Rental> GetRentalsByUserId(int userId);
        public bool MarkAsReturned(int rentalId, int userId);
        bool Rent(int movieId, int userId);
    }
}
** DbContext inject to Repository
** AdminRepository.cs
namespace VideoMovieRent.DataAccess.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private readonly VideoMovieRentDbContext _db;
** Domain model in Repositories
        public AdminRepository(VideoMovieRentDbContext db)
        {
            _db = db;
        }
        public Admin? Login(string username, string password)
        {
            return _db.Admins.FirstOrDefault(a =>
                a.Username == username && a.Password == password);
        }
        public void Create(Movie entity)
        {
            _db.Movies.Add(entity);
            _db.SaveChanges();
        }

        public void Update(Movie entity)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            var movie = _db.Movies.FirstOrDefault(x => x.Id == id);
            _db.Movies.Remove(movie);
            _db.SaveChanges();
        }
    }
}
UserRepository.cs
namespace VideoMovieRent.DataAccess.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly VideoMovieRentDbContext _db;

        public UserRepository(VideoMovieRentDbContext db)
        {
            _db = db;
        }
        public User GetUserByCardNumber(string cardNumber)
        {
            return _db.Users.FirstOrDefault(x => x.CardNumber == cardNumber);
        }

        public void Login(string cardNumber)
        {
            var user = _db.Users.FirstOrDefault(x => x.CardNumber == cardNumber);
        }

        public void CreateUser(User user)
        {
            _db.Users.Add(user);
        }
    }
}
RentalRepository.cs
namespace VideoMovieRent.DataAccess.Implementation
{
    public class RentalRepository : IRentalRepository
    {
        private readonly VideoMovieRentDbContext _db;
        public RentalRepository(VideoMovieRentDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Rental> GetRentalsByUserId(int userId)
        {
            return _db.Rentals.Where(r => r.UserId == userId && r.ReturnedOn == DateTime.MinValue).ToList();
        }

        public bool MarkAsReturned(int rentalId, int userId)
        {
            var rental = _db.Rentals.FirstOrDefault(r => r.Id == rentalId && r.UserId == userId && r.ReturnedOn == DateTime.MinValue);
            if (rental == null)
            {
                return false; // Rental not found or already returned
            }
            rental.ReturnedOn = DateTime.UtcNow;
            var movie = _db.Movies.FirstOrDefault(m => m.Id == rental.MovieId);
            if (movie != null)
            {
                movie.Quantity++; // Increment the quantity of the movie
                if (movie.Quantity > 0)
                {
                    movie.IsAvailable = true; // Mark the movie as available
                }

            }
            _db.SaveChanges();
            return true; // Rental marked as returned successfully
        }

        public bool Rent(int movieId, int userId)
        {
            var movie = _db.Movies.SingleOrDefault(x => x.Id == movieId);

            if (movie == null || movie.Quantity <= 0)
            {
                return false; // Movie not available for rent
            }
            var rental = new Rental
            {
                MovieId = movieId,
                Title = movie.Title,
                UserId = userId,
                RentedOn = DateTime.UtcNow,
                ReturnedOn = DateTime.MinValue // Not returned yet
            };
            _db.Rentals.Add(rental);
            movie.Quantity--; // Decrement the quantity of the movie
            if (movie.Quantity == 0)
            {
                movie.IsAvailable = false; // Mark the movie as not available
            }
            _db.SaveChanges();
            return true; // Rental created successfully
        }

    }
}
namespace VideoMovieRent.DataAccess.Implementation
{
    public class MovieRepository : IRepository<Movie>
    {
        private readonly VideoMovieRentDbContext _db;

        public MovieRepository(VideoMovieRentDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Movie> GetAll()
        {
            return _db.Movies.ToList();
        } 
        public Movie GetById(int id)
        {
            return _db.Movies.SingleOrDefault(x => x.Id == id);
        }
        public void Create(Movie entity)
        {
            _db.Movies.Add(entity);
            _db.SaveChanges();
        }
        public void Delete(int id)
        {
            var movie = _db.Movies.SingleOrDefault(x => x.Id == id);

            if (movie != null)
            {
                _db.Movies.Remove(movie);
                _db.SaveChanges();
            }
        }
        public void Update(Movie entity)
        {
            _db.Movies.Update(entity);
            _db.SaveChanges();
        }
      
    }
}
** DbContext in Repository and Repositories in Services
🔧 Services
MovieService.cs
•	Uses repository to return movie list and details
•	Converts domain model to DTOs for views
namespace VideoMovieRent.Services.Interfaces
{
    public interface IMovieService
    {
        IEnumerable<MovieDto> GetAllMovies();
        MovieDetailsDto GetMovieDetails(int id);
        Movie GetById(int id);
        IEnumerable<MovieDto> SearchMovies(string? title, Genre? genre, Language? language);
    }
}
namespace VideoMovieRent.Services.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;
        public MovieService(IRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }
** Converts domain model to DTOs for views
        public IEnumerable<MovieDto> GetAllMovies()
        {
            var movieDto = new List<MovieDto>();
            List<Movie> movies = _movieRepository.GetAll().ToList();
            if (movies != null && movies.Count > 0)
            {
                foreach (var movie in movies)
                {
                    movieDto.Add(new MovieDto
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Genre = movie.Genre,
                        IsAvailable = movie.Quantity > 0,
                        ImagePath = movie.ImagePath
                    });
                }
                return movieDto;
            }
            return movieDto;
        }

        // Replace the GetById method with the correct return type and logic
        public Movie GetById(int id)
        {
            return _movieRepository.GetById(id);
          
        }
        public MovieDetailsDto GetMovieDetails(int id)
        {
            var movie = _movieRepository.GetById(id);
            if (movie != null)
            {
                var movieDetails = new MovieDetailsDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Genre = movie.Genre,
                    Length = movie.Length,
                    AgeRestriction = movie.AgeRestriction,
                    Quantity = movie.Quantity,
                    ReleaseDate = movie.ReleaseDate,
                    Language = movie.Language,
                    ImagePath = movie.ImagePath
                };
                return movieDetails;
            }
            else
            {
                throw new KeyNotFoundException($"Movie with ID {id} not found.");
            }
        }
        public IEnumerable<MovieDto> SearchMovies(string? title, Genre? genre, Language? language)
        {
            var query = _movieRepository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(m => m.Title.Contains(title));
            if (genre.HasValue)
                query = query.Where(m => m.Genre == genre.Value);
            if (language.HasValue)
                query = query.Where(m => m.Language == language.Value);

            return query.Select(m => new MovieDto 
            { 
                Id = m.Id,
                Title = m.Title,
                Genre = m.Genre,
                Language = m.Language,
                IsAvailable = m.Quantity > 0,
                ImagePath = m.ImagePath,
            }).ToList();
        }
        
    }
}
RentalService.cs
•	RentMovie(): Creates a rental, decrements movie quantity
•	MarkAsReturned(): Marks rental returned, updates availability
namespace VideoMovieRent.Services.Interfaces
{
    public interface IRentalService
    {
        IEnumerable<Rental> GetRentalsByUserId(int userId);
        public bool MarkAsReturned(int rentalId, int userId);
        bool RentMovie(int movieId, int userId);
    }
}
namespace VideoMovieRent.Services.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        public RentalService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }
        public IEnumerable<Rental> GetRentalsByUserId(int userId)
        {
            return _rentalRepository.GetRentalsByUserId(userId);
        }

        public bool RentMovie(int userId, int movieId)
        {
            return _rentalRepository.Rent(userId, movieId);
        }

        public bool MarkAsReturned(int userId, int movieId)
        {
            return _rentalRepository.MarkAsReturned(userId, movieId);
        }
    }
}
AdminService.cs
•	Create/update Movie(): Creates a movie, update movie details
namespace VideoMovieRent.Services.Services.Interfaces
{
    public interface IAdminService
    {
        Admin Login(string username, string password);
        void CreateMovie(MovieDetailsDto dto);
        void UpdateMovie(MovieDetailsDto dto);
        void DeleteMovie(int id);
    }
}
namespace VideoMovieRent.Services.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public void CreateMovie(MovieDetailsDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Genre = dto.Genre,
                Language = dto.Language,
                Quantity = 1, // Default quantity for new movies
                ImagePath = dto.ImagePath
            };
            _adminRepository.Create(movie);
        }
        public void DeleteMovie(int id)
        {
            _adminRepository.Delete(id);
        }
        public Admin? Login(string username, string password)
        {
            return _adminRepository.Login(username, password);
        }
        public void UpdateMovie(MovieDetailsDto entity)
        {
            var movie = new Movie
            {
                Id = entity.Id,
                Title = entity.Title,
                Genre = entity.Genre,
                Length = entity.Length,
                AgeRestriction = entity.AgeRestriction,
                Quantity = entity.Quantity,
                ReleaseDate = entity.ReleaseDate,
                Language = entity.Language,
                ImagePath = entity.ImagePath
            };
        }
    }
}
namespace VideoMovieRent.Services.Interfaces
{
    public interface IUserService
    {
        User GetUserByCardNumber(string cardNumber);
        void CreateUser(User user);
        void Login(string cardNumber);
    }
}
namespace VideoMovieRent.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateUser(User user)
        {
            _userRepository.CreateUser(user);
        }

        public User GetUserByCardNumber(string cardNumber)
        {
            return _userRepository.GetUserByCardNumber(cardNumber);
        }
        public void Login(string cardNumber)
        {
            _userRepository.Login(cardNumber);
        }
    }
}
namespace VideoMovieRent.Filters
{
    // This attribute restricts access to admin-only actions
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAdmin = context.HttpContext.Session.GetString("IsAdminLoggedIn");

            if (string.IsNullOrEmpty(isAdmin) || isAdmin != "true")
            {
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
        }
    }
}
namespace VideoMovieRent.Services.Dtos
{
    public class RegisterDto
    {
        [Required] public string FullName { get; set; }
        [Required] public string CardNumber { get; set; }
        [Required]
        [Range(18, 80, ErrorMessage = "Age must be between 18 and 80.")]
        public int Age { get; set; }
    }
}
namespace VideoMovieRent.Services.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Genre Genre { get; set; }
        public Language Language { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImagePath { get; set; }
    }
}
namespace VideoMovieRent.Services.Dtos
{
    public class RentalDto
    {
        public required string MovieTitle { get; set; }
        public DateTime RentedOn { get; set; }
        public required string cardNumber { get; set; }
    }
}
namespace VideoMovieRent.Services.Dtos
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Genre Genre { get; set; }
        public TimeSpan Length { get; set; }
        public int AgeRestriction { get; set; }
        public int Quantity { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Language Language { get; set; }
        public string? ImagePath { get; set; }
    }
}
namespace VideoMovieRent.Services.Dtos
{
    public class DeleteDto : BaseEntity
    {
        public string Title { get; set; }
        public Genre Genre { get; set; }
        public int Quantity { get; set; }
    }
}
Controllers
// Main controller for movie operations
public class MovieController : Controller
{
    // Actions for:
    // - Listing movies (Index)
    // - User login (Login)
    // - Registration (Register)
    // - Movie details (Details)
    // - Renting movies (Rent)
    // - Returning movies (Return, ReturnMovie)
    // - Logout (Logout)
}
namespace VideoMovieRentapp.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;           // Handles movie-related operations
        private readonly IRentalService _rentalService;         // Handles renting and returning of movies
        private readonly IUserService _userService;             // Used to find users by card number
        private readonly IWebHostEnvironment _webHostEnvironment; // Used to get the wwwroot path for saving images
        public MovieController(
            IMovieService movieService,
            IRentalService rentalService,
            IUserService userService,
            IWebHostEnvironment webHostEnvironment)
        {
            _movieService = movieService;
            _rentalService = rentalService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }
        // Display list of movies
        public IActionResult Index(int userId, int managerId)
        {
            List<MovieDto> movies = _movieService.GetAllMovies().ToList();
            ViewBag.UserId = userId;           // Used in view to know if a user is logged in
            return View(movies);               // Pass movies to the view
        }
        // GET: Show user login form
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        // POST: Handle user login by card number
        [HttpPost]
        public IActionResult Login(string cardNumber)
        {
            var user = _userService.GetUserByCardNumber(cardNumber);
            if (user == null)
            {
                ViewBag.Error = "Invalid card number.";
                return View(); // Show error if card number not found
            }
            return RedirectToAction("Index", new { userId = user.Id }); // Login success
        }
        // POST: Logout user or manager and reset state
        [HttpPost]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", new { userId = 0 }); // Return to base state
        }
        // Step 1: Show card number input form
        [HttpGet]
        public IActionResult CheckCard()
        {
            return View(); // CheckCard.cshtml
        }

        // Step 1: Validate card number
        [HttpPost]
        public IActionResult CheckCard(string cardNumber)
        {
            var existingUser = _userService.GetUserByCardNumber(cardNumber);
            if (existingUser != null)
            {
                ViewBag.Error = "Card number already exists. Try logging in.";
                return View(); // back to CheckCard.cshtml
            }

            // Card is new – pass it to the register form
            return RedirectToAction("Register", new { cardNumber = cardNumber });
        }

        // Step 2: Show register form (for new card)
        [HttpGet]
        public IActionResult Register(string cardNumber)
        {
            var dto = new RegisterDto { CardNumber = cardNumber };
            return View(dto); // Register.cshtml
        }

        // Step 2: Save user
        [HttpPost]
        public IActionResult Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var existingUser = _userService.GetUserByCardNumber(dto.CardNumber);
            if (existingUser != null)
            {
                ViewBag.Error = "Card number already exists.";
                return View(dto);
            }

            var newUser = new User
            {
                FullName = dto.FullName,
                Age = dto.Age,
                CardNumber = dto.CardNumber,
                CreatedOn = DateTime.Now,
                IsSubscriptionExpired = false,
                SubscriptionType = SubscriptionType.Yearly // Default to yearly subscription
            };

            _userService.CreateUser(newUser);

            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Search(string? title, Genre? genre, Language? language)
        {
            var results = _movieService.SearchMovies(title, genre, language);
            return View("Index", results);
        }

        // Show movie details page for user
        public IActionResult Details(int id, int userId)
        {
            MovieDetailsDto movieDets = _movieService.GetMovieDetails(id);
            ViewBag.UserId = userId;
            return View(movieDets); // Show detailed info
        }
        // POST: Rent a movie for the user
        [HttpPost]
        public IActionResult Rent(int movieId, int userId)
        {
            var success = _rentalService.RentMovie(movieId, userId);
            if (!success)
            {
                TempData["Error"] = "Movie is not available for rent."; // Show error
            }
            return RedirectToAction("Details", new { id = movieId, userId });
        }
        // Show all active rentals for a user
        public IActionResult Return(int userId)
        {
            var rentals = _rentalService.GetRentalsByUserId(userId);
            ViewBag.UserId = userId;
            return View(rentals); // Show list of rented movies to return
        }
        // POST: Return a movie
        [HttpPost]
        public IActionResult ReturnMovie(int rentalId, int userId)
        {
            _rentalService.MarkAsReturned(rentalId, userId); // Mark rental as returned
            return RedirectToAction("Return", new { userId }); // Refresh page
        }
    }
}
// Admin Controler: Handles admin login and movie management
public class AdminController : Controller
{
    // Actions for:
    // - Listing movies (Index)
    // - Admin login (Login)
    // - Create Movie (Create)
    // - Update movies (Edit)
    // - Delete movies (Delete)
    // - Logout (Logout)
}
namespace VideoMovieRentapp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IMovieService _movieService;
        private readonly IWebHostEnvironment _webHostEnvironment; // Used to get the wwwroot path for saving images


        public AdminController(IMovieService movieService, IAdminService adminService, IWebHostEnvironment webHostEnvironment)
        {
            _movieService = movieService;
            _adminService = adminService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var admin = _adminService.Login(username, password);
            if (admin == null)
            {
                ViewBag.Error = "Invalid admin credentials.";
                return View();
            }

            HttpContext.Session.SetString("IsAdminLoggedIn", "true");
            HttpContext.Session.SetString("AdminUsername", admin.Username);
            return RedirectToAction("Index");
        }
        [AdminAuthorize]
        public IActionResult Index()
        {
            var movies = _movieService.GetAllMovies();
            return View(movies);
        }
        [HttpPost]
        [AdminAuthorize]
        public IActionResult Logout()
        {
            // Clear the session to log out the admin
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [AdminAuthorize]
        public IActionResult Create()
        {
            return View(new MovieDetailsDto());
        }

        [HttpPost]
        [AdminAuthorize]
        public IActionResult Create(MovieDetailsDto dto, IFormFile ImageFile)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string moviePath = Path.Combine(wwwRootPath, "images", "movies");
                if (!Directory.Exists(moviePath))
                {
                    Directory.CreateDirectory(moviePath);
                }
                using (var fileStream = new FileStream(Path.Combine(moviePath, fileName), FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }
                dto.ImagePath = "/images/movies/" + fileName;
            }
            else
            {
                dto.ImagePath = null; // Handle the case where no image is uploaded
            }
            _adminService.CreateMovie(dto);
            return RedirectToAction("Create");
        }

        [HttpGet]
        [AdminAuthorize]
        public IActionResult Edit(int id)
        {
            var movie = _movieService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound();
            }
            var dto = new MovieDetailsDto
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                AgeRestriction = movie.AgeRestriction,
                Genre = movie.Genre,
                Language = movie.Language,
                Length = movie.Length,
                Quantity = movie.Quantity,
                ImagePath = movie.ImagePath
            };
            ViewBag.MovieId = id;
            return View("Edit", dto);
        }

        [HttpPost]
        [AdminAuthorize]
        public IActionResult Edit(MovieDetailsDto dto)
        {
            _adminService.UpdateMovie(dto);
            return RedirectToAction("Edit");
        }
        [HttpGet]
        [AdminAuthorize]
        public IActionResult DeleteMovie(int id)
        {
            var movie = _movieService.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }
            var dto = new DeleteDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Quantity = movie.Quantity
            };
            ViewBag.MovieId = id;
            return View(dto);
        }
        [HttpPost]
        [AdminAuthorize]
        public IActionResult Delete(int id)
        {
            _adminService.DeleteMovie(id);
            return RedirectToAction("Index");
        }
    }
}
________________________________________
🖼️ Views (Razor)
Login.cshtml
•	Accepts card number or admin password
•	Displays error if not found
** views - admin -index , login, create, edit, delete
@model List<MovieDto>
@{
    // Ensure adminId is available in the view. 
    // If it's passed via ViewData, ViewBag, or as part of the model, assign it here.
    // Example using ViewBag:
    var adminId = ViewBag.AdminId;
}

<div class="container py-5">
    <h2 class="mb-4">Available Movies</h2>

    <form asp-controller="Admin" asp-action="Logout" method="post" class="d-inline mb-5">
        <button type="submit" class="btn btn-danger">Logout</button>
    </form>
   
        <a asp-controller="Admin" asp-action="Create" asp-route-adminId="@adminId" class="btn btn-success mb-3">Create Movie</a>
   
    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
        @foreach (var movie in Model)
        {
            <div class="col">
                <div class="card h-100 shadow-sm border-0">
                    <img src="@movie.ImagePath" class="card-img-top" alt="@movie.Title" style="height: 400px; object-fit: cover;" />
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title">@movie.Title</h5>
                        <p class="card-text mb-1"><strong>Genre:</strong> @movie.Genre</p>
                        <p class="card-text mb-3">
                            <strong>Status:</strong>
                            @if (movie.IsAvailable)
                            {
                                <span class="badge bg-success">Available</span>
                            }
                            else
                            {
                                <span class="badge bg-secondary">Unavailable</span>
                            }
                        </p>
                        <div class="mt-auto d-flex gap-2">
                            <a asp-controller="Admin" asp-action="Details" asp-route-id="@movie.Id" asp-route-adminId="@adminId" class="btn btn-outline-secondary w-50">
                                Details
                            </a>
                           
                                <button class="btn btn-secondary w-50" disabled>Rent</button>
                            
                        </div>
                      
                            <a asp-controller="Admin" asp-action="DeleteMovie" asp-route-id="@movie.Id" asp-route-adminId="@adminId" class="btn btn-danger w-100 mt-2">
                                Delete
                            </a>
                            <a asp-controller="Admin" asp-action="Edit" asp-route-id="@movie.Id" asp-route-adminId="@adminId" class="btn btn-warning w-100 mt-2">
                                Update
                            </a>
                       
                    </div>
                </div>
            </div>
        }
    </div>
</div>

@{
    ViewData["Title"] = "Admin Login";
}

<h2>Admin Login</h2>

<form asp-action="Login" method="post">
    <div class="mb-3">
        <label>Username</label>
        <input type="text" name="username" class="form-control" required />
    </div>
    <div class="mb-3">
        <label>Password</label>
        <input type="password" name="password" class="form-control" required />
    </div>

    <button type="submit" class="btn btn-primary" > Login </ button >
</ form >
@if (ViewBag.Error != null)
{
    <p class="text-danger mt-2">@ViewBag.Error</p>
}

@model MovieDetailsDto

@{
    ViewData["Title"] = "Create Movie";
    var adminId = ViewBag.AdminId; // Fix: define adminId from ViewBag or ViewData
}
Create.cshtml
•	Create movies using MovieDetailsDto
•	Includes all movie metadata fields
<h2>Create New Movie</h2>
<form asp-action="Create" asp-route-adminId="@adminId" method="post" enctype="multipart/form-data">
    <div class="mb-3">
        <label class="form-label">Title</label>
        <input type="text" class="form-control" name="Title" required />
        <span asp-validation-for="Title" class="text-danger"></span>
    </div>
    <div class="mb-3">
        <label class="form-label">Genre</label>
        <select class="form-control" name="Genre">
            @foreach (var genre in Enum.GetValues(typeof(VideoMovieRent.Domain.Enums.Genre)))
            {
                <option value="@genre">@genre</option>
            }
        </select>
    </div>
    <div class="mb-3">
        <label class="form-label">Language</label>
        <select class="form-control" name="Language">
            @foreach (var lang in Enum.GetValues(typeof(VideoMovieRent.Domain.Enums.Language)))
            {
                <option value="@lang">@lang</option>
            }
        </select>
    </div>

    <div class="mb-3">
        <label class="form-label">Release Date</label>
        <input type="date" class="form-control" name="ReleaseDate" required />
    </div>
    <div class="mb-3">
        <label class="form-label">Length (hh:mm)</label>
        <input type="text" class="form-control" name="Length" placeholder="e.g. 02:30" required />
    </div>
    <div class="mb-3">
        <label class="form-label">Age Restriction</label>
        <input type="number" class="form-control" name="AgeRestriction" required />
    </div>
    <div class="mb-3">
        <label class="form-label">Quantity</label>
        <input type="number" class="form-control" name="Quantity" required />
    </div>
    <div class="mb-3">
        <label class="form-label">Movie Image</label>
        <input type="file" class="form-control" name="ImageFile" accept="image/*" />
    </div>
    <button type="submit" class="btn btn-success">Create</button>

    <button type="submit" class="btn btn-success">Save</button>
    <a asp-action="Index" class="btn btn-secondary">Cancel</a>
</form>

@model MovieDetailsDto

@{
    ViewData["Title"] = "Edit Movie";
    var adminId = ViewBag.AdminId; // Fix: define adminId from ViewBag or ViewData

}
Edit.cshtml
•	Pre-populated form for movie details
•	Allows updating all fields
<h2>Edit Movie</h2>

<form asp-action="Edit" asp-route-adminId="@adminId" method="post" enctype="multipart/form-data">
    <div class="mb-3">
        <label class="form-label">Title</label>
        <input type="text" class="form-control" name="Title" required />
        <span asp-validation-for="Title" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label class="form-label">Genre</label>
        <select class="form-control" name="Genre">
            @foreach (var genre in Enum.GetValues(typeof(VideoMovieRent.Domain.Enums.Genre)))
            {
                <option value="@genre">@genre</option>
            }
        </select>
    </div>

    <div class="mb-3">
        <label class="form-label">Language</label>
        <select class="form-control" name="Language">
            @foreach (var lang in Enum.GetValues(typeof(VideoMovieRent.Domain.Enums.Language)))
            {
                <option value="@lang">@lang</option>
            }
        </select>
    </div>

    <div class="mb-3">
        <label class="form-label">Release Date</label>
        <input type="date" class="form-control" name="ReleaseDate" required />
    </div>
    <div class="mb-3">
        <label class="form-label">Length (hh:mm)</label>
        <input type="text" class="form-control" name="Length" placeholder="e.g. 02:30" required />
    </div>
    <div class="mb-3">
        <label class="form-label">Age Restriction</label>
        <input type="number" class="form-control" name="AgeRestriction" required />
    </div>
    <div class="mb-3">
        <label class="form-label">Quantity</label>
        <input type="number" class="form-control" name="Quantity" required />
    </div>
    <div class="mb-3">
        <label class="form-label">Movie Image</label>
        <input type="file" class="form-control" name="ImageFile" accept="image/*" />
    </div>
    <button type="submit" class="btn btn-success">Update</button>

    <button type="submit" class="btn btn-success">Save</button>
    <a asp-action="Index" class="btn btn-secondary">Cancel</a>
</form>
@model DeleteDto
@{
    ViewData["Title"] = "Delete Movie";
    var adminId = ViewBag.AdminId; // Fix: define adminId from ViewBag or ViewData
}
<h2>Delete Movie</h2>
<form asp-action="Delete" asp-route-adminId="@adminId" method="post" id="deleteForm">
    <div class="mb-3">
        <label class="form-label">Title</label>
        <input type="text" class="form-control" value="@Model.Title" disabled />
    </div>
    <div class="mb-3">
        <label class="form-label">Genre</label>
        <input type="text" class="form-control" value="@Model.Genre" disabled />
    </div>
    <div class="mb-3">
        <label class="form-label">Quantity</label>
        <input type="number" class="form-control" value="@Model.Quantity" disabled />
    </div>
    <input type="hidden" name="id" value="@ViewBag.MovieId" />
    <button type="button" class="btn btn-danger" onclick="showModal()">Delete</button>
     <a asp-action="Index" class="btn btn-secondary" > Cancel </ a >
</ form >
** views - movie - index - login - register - Details - Return - check card
Index.cshtml
•	Displays all movies using MovieDto
•	Enables renting only when logged in
@model List<MovieDto>
@{
    var userIdStr = Context.Request.Query["userId"].ToString();
    int userId = 0;
    int.TryParse(userIdStr, out userId);
    bool isLoggedIn = userId > 0;
}
<div class="container py-5">
    <h2 class="mb-4">Available Movies</h2>
    @if (userId > 0)
    {
        <form asp-controller="Movie" asp-action="Logout" method="post" class="d-inline mb-5">
            <button type="submit" class="btn btn-danger">Logout</button>
        </form>
        <a asp-controller="Movie" asp-action="Return" asp-route-userId="@userId" class="btn btn-info mb-3 ms-2">My Rentals</a>
    }
       <form method="get" asp-action="Search">
        <input type="text" name="title" placeholder="Title" />
        <select name="genre">
            <option value="">All Genres</option>
            <option value="Action">Action</option>
            <option value="Comedy">Comedy</option>
            <option value="Drama">Drama</option>
            <option value="Horror">Horror</option>
            <option value="Sci-Fi">Sci-Fi</option>
        </select>
        <select name="language">
            <option value="">All Languages</option>
            <option value="English">English</option>
            <option value="Spanish">Spanish</option>
            <option value="French">French</option>
            <option value="German">German</option>
            <option value="Japanese">Japanese</option>
        </select>
        <button type="submit">Search</button>
    </form>
    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
        @foreach (var movie in Model)
        {
            <div class="col">
                <div class="card h-100 shadow-sm border-0">
                    <img src="@movie.ImagePath" class="card-img-top" alt="@movie.Title" style="height: 400px; object-fit: cover;" />
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title">@movie.Title</h5>
                        <p class="card-text mb-1"><strong>Genre:</strong> @movie.Genre</p>
                        <p class="card-text mb-3">
                            <strong>Status:</strong>
                            @if (movie.IsAvailable)
                            {
                                <span class="badge bg-success">Available</span>
                            }
                            else
                            {
                                <span class="badge bg-secondary">Unavailable</span>
                            }
                        </p>
                        <div class="mt-auto d-flex gap-2">
                            <a asp-controller="Movie" asp-action="Details" asp-route-id="@movie.Id" asp-route-userId="@userId" class="btn btn-outline-secondary w-50">
                                Details
                            </a>
                            @if (!isLoggedIn || !movie.IsAvailable)
                            {
                                <button class="btn btn-secondary w-50" disabled>Rent</button>
                            }
                            else
                            {
                                <form asp-controller="Movie" asp-action="Rent" method="post" class="w-50">
                                    <input type="hidden" name="movieId" value="@movie.Id" />
                                    <input type="hidden" name="userId" value="@userId" />
                                    <button type="submit" class="btn btn-primary w-100">Rent</button>
                                </form>
                            }
                        </div>
                        
                    </div>
                </div>
            </div>
        }
    </div>
</div>
login.cshtml
@{
    ViewData["Title"] = "Login";
    var error = ViewBag.Error as string;
}

<h2>Login</h2>
@if (!string.IsNullOrEmpty(error))
{
    <div class="alert alert-danger">@error</div>
}
<form asp-action="Login" method="post">
    <div class="mb-3">
        <label for="cardNumber" class="form-label">Card Number</label>
        <input type="text" class="form-control" id="cardNumber" name="cardNumber" required />
    </div>
    <button type="submit" class="btn btn-primary">Login</button>
</form>
Details.cshtml
•	Displays full movie info
•	Allows renting based on login status
@model MovieDetailsDto
@{
    var userIdObj = ViewBag.UserId ?? Context.Request.Query["userId"];
    var userIdStr = userIdObj != null ? userIdObj.ToString() : "";
    int userId = 0;
    int.TryParse(userIdStr, out userId);
    bool isLoggedIn = userId > 0;
}
<div class="container py-5">
    <div class="row justify-content-center">
        <div class="col-md-10 col-lg-8">
            <div class="card shadow-lg border-0 rounded-4">
                <div class="row g-0">
                    <div class="col-md-5">
                        <img src="@Model.ImagePath" alt="@Model.Title" class="img-fluid h-100 w-100 rounded-start" style="object-fit: cover;" />
                    </div>
                    <div class="col-md-7">
                        <div class="card-body p-4 d-flex flex-column h-100">
                            <h2 class="card-title mb-3">@Model.Title</h2>
                            <ul class="list-unstyled mb-4">
                                <li><strong>Genre:</strong> @Model.Genre</li>
                                <li><strong>Length:</strong> @Model.Length.Hours @("hr") @Model.Length.Minutes @("min")</li>
                                <li><strong>Age Restriction:</strong> @Model.AgeRestriction+</li>
                                <li><strong>Release Date:</strong> @Model.ReleaseDate</li>
                                <li><strong>Quantity Available:</strong> @Model.Quantity</li>
                                <li><strong>Language:</strong> @Model.Language</li>
                            </ul>

                            <div class="mt-auto d-flex gap-2">
                                <a asp-action="Index" asp-controller="Movie" asp-route-userId="@userId" class="btn btn-outline-secondary w-50">
                                    ← Back to List
                                </a>
                                @if (!isLoggedIn || Model.Quantity <= 0)
                                {
                                    <button class="btn btn-secondary w-50" disabled>Rent This Movie</button>
                                }
                                else
                                {
                                    <form asp-action="Rent" asp-controller="Movie" method="post" class="w-50">
                                        <input type="hidden" name="movieId" value="@Model.Id" />
                                        <input type="hidden" name="userId" value="@userId" />
                                        <button type="submit" class="btn btn-primary w-100">Rent This Movie</button>
                                    </form>
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
Return.cshtml
•	Shows user’s currently rented movies
•	Allows returning
@model IEnumerable<Rental>
@{
    var userIdObj = ViewBag.UserId ?? Context.Request.Query["userId"];
    var userIdStr = userIdObj != null ? userIdObj.ToString() : "";
    int userId = 0;
    int.TryParse(userIdStr, out userId);
}
<div class="container py-5">
    <h2 class="mb-4">Your Rented Movies</h2>
    @if (!Model.Any())
    {
        <div class="alert alert-info">You have no active rentals.</div>
    }
    else
    {
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Rental Id</th>
                    <th>Movie Id</th>
                    <th>Title</th>
                    <th>Rented On</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var rental in Model)
                {
                    <tr>
                        <td>@rental.Id</td>
                        <td>@rental.MovieId</td>
                        <td>@rental.Title</td>
                        <td>@rental.RentedOn</td>
                        <td>
                            <form asp-action="ReturnMovie" asp-controller="Movie" method="post">
                                <input type="hidden" name="rentalId" value="@rental.Id" />
                                <input type="hidden" name="userId" value="@userId" />
                                <button type="submit" class="btn btn-success">Return</button>
                            </form>
                        </td>
                    </tr>
                }

            </tbody>
        </table>
    }
    <a asp-action="Index" asp-controller="Movie" asp-route-userId="@userId" class="btn btn-outline-secondary w-50">
        ← Back to List
    </a>
</div>
Register.cshtml
•	Form registration of new user
•	Displays error if not found
@model RegisterDto
@{
    ViewData["Title"] = "Register new User";
}
@if (ViewBag.Error != null)
{
    <div class="alert alert-danger">@ViewBag.Error</div>
}
<h2>User Registration</h2>

<form asp-action="Register" method="post">
    <div class="form-group">
        <label asp-for="FullName"></label>
        <input asp-for="FullName" class="form-control" />
    </div>

    <div class="form-group">
        <label asp-for="Age"></label>
        <input asp-for="Age" type="number" class="form-control" />
    </div>

    <div class="form-group">
        <label asp-for="CardNumber"></label>
        <input asp-for="CardNumber" class="form-control" />
    </div>

    <button type="submit" class="btn btn-primary">Register</button>
    <a asp-action="Index" class="btn btn-secondary" > Cancel </ a >
</ form >

@model MovieDetailsDto
@{
    var userIdObj = ViewBag.UserId ?? Context.Request.Query["userId"];
    var userIdStr = userIdObj != null ? userIdObj.ToString() : "";
    int userId = 0;
    int.TryParse(userIdStr, out userId);
    bool isLoggedIn = userId > 0;
}

<div class="container py-5">
    <div class="row justify-content-center">
        <div class="col-md-10 col-lg-8">
            <div class="card shadow-lg border-0 rounded-4">
                <div class="row g-0">
                    <div class="col-md-5">
                        <img src="@Model.ImagePath" alt="@Model.Title" class="img-fluid h-100 w-100 rounded-start" style="object-fit: cover;" />
                    </div>
                    <div class="col-md-7">
                        <div class="card-body p-4 d-flex flex-column h-100">
                            <h2 class="card-title mb-3">@Model.Title</h2>
                            <ul class="list-unstyled mb-4">
                                <li><strong>Genre:</strong> @Model.Genre</li>
                                <li><strong>Length:</strong> @Model.Length.Hours @("hr") @Model.Length.Minutes @("min")</li>
                                <li><strong>Age Restriction:</strong> @Model.AgeRestriction+</li>
                                <li><strong>Release Date:</strong> @Model.ReleaseDate</li>
                                <li><strong>Quantity Available:</strong> @Model.Quantity</li>
                                <li><strong>Language:</strong> @Model.Language</li>
                            </ul>

                            <div class="mt-auto d-flex gap-2">
                                <a asp-action="Index" asp-controller="Movie" asp-route-userId="@userId" class="btn btn-outline-secondary w-50">
                                    ← Back to List
                                </a>
                                @if (!isLoggedIn || Model.Quantity <= 0)
                                {
                                    <button class="btn btn-secondary w-50" disabled>Rent This Movie</button>
                                }
                                else
                                {
                                    <form asp-action="Rent" asp-controller="Movie" method="post" class="w-50">
                                        <input type="hidden" name="movieId" value="@Model.Id" />
                                        <input type="hidden" name="userId" value="@userId" />
                                        <button type="submit" class="btn btn-primary w-100">Rent This Movie</button>
                                    </form>
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
@{
    ViewData["Title"] = "Check Card Number";
}

<h2>Check Card Number</h2>

@if (ViewBag.Error != null)
{
    <div class="alert alert-danger">@ViewBag.Error</div>
}

<form asp-action="CheckCard" method="post">
    <div class="form-group">
        <label for="cardNumber">Card Number</label>
        <input name="cardNumber" class="form-control" required />
    </div>
    <button type="submit" class="btn btn-primary">Continue</button>
</form>
** Shared _Layout.cshtml
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - VideoMovieRentapp</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/VideoMovieRentapp.styles.css" asp-append-version="true" />
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container-fluid">
                <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">VideoMovieRentapp</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Movie" asp-action="Login">User Login</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Admin" asp-action="Login">Admin Login</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Movie" asp-action="CheckCard">Register new User</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2025 - VideoMovieRentapp - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
        </div>
    </footer>
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
** Program.cs
🔁 Dependency Injection (in Program.cs)
// Configure maximum file size for form uploads (image upload readiness)
builder.Services.Configure<FormOptions>(options => {
    options.MultipartBodyLengthLimit = 104857600; // 100 MB});
// Inject EF Core with SQL Server
builder.Services.AddDbContext<VideoMovieRentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnString")));
// Repositories
builder.Services.AddScoped<IRepository<Movie>, MovieRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
// Services
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IAdminService, AdminService>();
// Enable Session for login simulation
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movie}/{action=Index}/{id?}");

app.Run();


🎯 Functionality Flow
•	🔐 Login Flow
Login View → MovieController.Login() → UserService.GetUserByCardNumber()
→ If success → Redirect to Index (with userId)
•	🎬 Browse & Rent
MovieController.Index() → MovieService.GetAllMovies() → index.cshtml
MovieController.Details() → MovieService.GetMovieDetails() → details.cshtml
MovieController.Rent() → RentalService.RentMovie()
•	📦 Return
MovieController.Return() → RentalService.GetRentalsByUserId() → return.cshtml
MovieController.ReturnMovie() → RentalService.MarkAsReturned()
🎯 Admin Functionality Flow
Login Flow:
Admin/Login → AdminController.Login() → AdminService.Login() → Redirect to Index
Create Movie:
Admin/Create → Handles form post → Saves image → AdminService.CreateMovie()
Update Movie:
Admin/Edit/{id} → Loads current data → AdminService.UpdateMovie()
Delete Movie:
Admin/Delete/{id} → Confirmation → AdminService.DeleteMovie()
Logout:
Clears session → Redirects to login page
🧪 Sample Seed Data (DbContext)
•	10 Users
•	10 Movies
•	10 Rentals
•	10 Cast entries
🔍 Notes
•	Login is card number only (mock authentication).
•	Session-based login optional — you can extend with HttpContext.Session.
•	All admin routes protected by [AdminAuthorize]
•	Image paths reference wwwroot/images/movie.
