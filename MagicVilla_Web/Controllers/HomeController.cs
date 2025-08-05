using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        
            public IActionResult Index()
            {
                return View();
            }

            public IActionResult Privacy()
            {
                return View();
            }
            public IActionResult IndexVilla()
            {
                return View();
            }

    }
}
