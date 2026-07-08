using Microsoft.AspNetCore.Mvc;
using SistemRumahMakan.Services;
using SistemRumahMakan.Helpers;

namespace SistemRumahMakan.Controllers
{
    [SessionAuthorize]
    public class DashboardController : Controller
    {
        private readonly DashboardService _service;

        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var model = _service.GetDashboard();

            return View(model);
        }
    }
}