using Microsoft.AspNetCore.Mvc;

namespace SistemRumahMakan.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login","Account");
        }
    }
}