using System.Diagnostics;
using Lamazon.Services.Interfaces;
using Lamazon.Web.Models;
using Microsoft.AspNetCore.Mvc;

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
