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
UserRepository.cs
public class UserRepository : IUserRepository
{
    public User GetUserByCardNumber(string cardNumber)
    {
        return _db.Users.FirstOrDefault(x => x.CardNumber == cardNumber);
    }
}
RentalRepository.cs
public IEnumerable<Rental> GetRentalsByUserId(int userId)
{
    // Only return not yet returned rentals
    return _db.Rentals.Where(r => r.UserId == userId && r.ReturnedOn == DateTime.MinValue).ToList();
}
IAdminRepository.cs
public interface IAdminRepository
{
    Admin? Login(string username, string password);
    void Create(Movie entity);
    void Update(Movie entity);
    void Delete(int id);
}
🔧 Services
MovieService.cs
•	Uses repository to return movie list and details
•	Converts domain model to DTOs for views
RentalService.cs
•	RentMovie(): Creates a rental, decrements movie quantity
•	MarkAsReturned(): Marks rental returned, updates availability
AdminService.cs
•	Create/update Movie(): Creates a movie, update movie details
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
________________________________________
🖼️ Views (Razor)
Login.cshtml
•	Accepts card number or admin password
•	Displays error if not found
Index.cshtml
•	Displays all movies using MovieDto
•	Enables renting only when logged in
Details.cshtml
•	Displays full movie info
•	Allows renting based on login status
Return.cshtml
•	Shows user’s currently rented movies
•	Allows returning
Register.cshtml
•	Form registration of new user
•	Displays error if not found
Create.cshtml
•	Create movies using MovieDetailsDto
•	Includes all movie metadata fields
Edit.cshtml
•	Pre-populated form for movie details
•	Allows updating all fields
Delete.cshtml
•	Delete movies
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
