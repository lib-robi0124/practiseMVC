using Lamazon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            if(id has )
    }
}
