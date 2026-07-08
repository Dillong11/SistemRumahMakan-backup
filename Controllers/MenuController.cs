using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemRumahMakan.Models;
using SistemRumahMakan.Services;
using Microsoft.AspNetCore.Hosting;

namespace SistemRumahMakan.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuService _service;

        public MenuController(MenuService service,
                      IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        public IActionResult Index(string? keyword)
        {
            return View(_service.GetAll(keyword));
        }

        public IActionResult Create()
        {
            ViewBag.Kategori = new SelectList(
                _service.GetKategori(),
                "Id",
                "NamaKategori");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Menu model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategori = new SelectList(
                    _service.GetKategori(),
                    "Id",
                    "NamaKategori");

                return View(model);
            }

            if (model.FotoFile != null)
            {
                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "menu");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName =
                    Guid.NewGuid() +
                    Path.GetExtension(model.FotoFile.FileName);

                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream =
                    new FileStream(filePath, FileMode.Create))
                {
                    model.FotoFile.CopyTo(stream);
                }

                model.Foto = fileName;
            }
            _service.Create(model);

            TempData["Success"] = "Menu berhasil ditambahkan.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            var data = _service.GetById(id);

            if (data == null)
                return NotFound();

            ViewBag.Kategori = new SelectList(
                _service.GetKategori(),
                "Id",
                "NamaKategori",
                data.KategoriMenuId);

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Menu model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategori = new SelectList(
                    _service.GetKategori(),
                    "Id",
                    "NamaKategori",
                    model.KategoriMenuId);

                return View(model);
            }

            _service.Update(model);

            TempData["Success"] = "Menu berhasil diubah.";

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

            TempData["Success"] = "Menu berhasil dihapus.";

            return RedirectToAction(nameof(Index));
        }

        private readonly IWebHostEnvironment _environment;
    }
}