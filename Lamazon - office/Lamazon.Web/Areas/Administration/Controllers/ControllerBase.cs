using Microsoft.AspNetCore.Mvc;

namespace Lamazon.Web.Areas.Administration.Controllers
{
    public class ControllerBase : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
