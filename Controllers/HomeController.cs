using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    public class HomeController : Controller
    {
        // GET: / or /Home or /Home/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}
