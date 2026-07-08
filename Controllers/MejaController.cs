using Microsoft.AspNetCore.Mvc;
using SistemRumahMakan.Models;
using SistemRumahMakan.Services;
using SistemRumahMakan.Helpers;

[SessionAuthorize]
public class DashboardController : Controller
{

}

namespace SistemRumahMakan.Controllers
{
    public class MejaController : Controller
    {
        private readonly MejaService _service;

        public MejaController(MejaService service)
        {
            _service = service;
        }

        public IActionResult Index(string? keyword)
        {
            var data = _service.GetAll(keyword);
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Meja model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.Create(model);

            TempData["Success"] = "Meja berhasil ditambahkan.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            var data = _service.GetById(id);

            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Meja model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.Update(model);

            TempData["Success"] = "Meja berhasil diubah.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            var data = _service.GetById(id);

            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.Delete(id);

            TempData["Success"] = "Meja berhasil dihapus.";

            return RedirectToAction(nameof(Index));
        }
    }
}