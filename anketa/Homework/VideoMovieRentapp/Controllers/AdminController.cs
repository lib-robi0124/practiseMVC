using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoMovieRent.Domain;
using VideoMovieRent.Filters;
using VideoMovieRent.Services.Dtos;
using VideoMovieRent.Services.Interfaces;
using VideoMovieRent.Services.Services.Interfaces;

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
