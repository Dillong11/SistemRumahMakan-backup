using Microsoft.AspNetCore.Mvc;
using SistemRumahMakan.Models;
using SistemRumahMakan.Services;
using SistemRumahMakan.Helpers;

    [SessionAuthorize]
    public class OrderController : Controller
    {
    
    }
namespace SistemRumahMakan.Controllers
{
    public class KategoriController : Controller
    {
        private readonly KategoriService _service;

        public KategoriController(KategoriService service)
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
        public IActionResult Create(KategoriMenu model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.Create(model);

            TempData["Success"] = "Kategori berhasil ditambahkan.";

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
        public IActionResult Edit(KategoriMenu model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.Update(model);

            TempData["Success"] = "Kategori berhasil diperbarui.";

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

            TempData["Success"] = "Kategori berhasil dihapus.";

            return RedirectToAction(nameof(Index));
        }
    }
}