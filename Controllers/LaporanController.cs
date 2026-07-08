using Microsoft.AspNetCore.Mvc;
using SistemRumahMakan.Services;

namespace SistemRumahMakan.Controllers
{
    public class LaporanController : Controller
    {
        private readonly LaporanService _service;

        public LaporanController(LaporanService service)
        {
            _service = service;
        }

        // Menu Laporan
        public IActionResult Index()
        {
            return View();
        }

        // ==========================
        // LAPORAN HARIAN
        // ==========================
        public IActionResult Harian(DateTime? tanggal)
        {
            tanggal ??= DateTime.Today;

            var model = _service.GetLaporan(tanggal, tanggal);

            return View(model);
        }

        // ==========================
        // LAPORAN BULANAN
        // ==========================
        public IActionResult Bulanan(int? bulan, int? tahun)
        {
            bulan ??= DateTime.Now.Month;
            tahun ??= DateTime.Now.Year;

            var awal = new DateTime(tahun.Value, bulan.Value, 1);
            var akhir = awal.AddMonths(1).AddDays(-1);

            var model = _service.GetLaporan(awal, akhir);

            return View(model);
        }

        // ==========================
        // MENU TERLARIS
        // ==========================
        public IActionResult MenuTerlaris()
        {
            var data = _service.GetMenuTerlaris();

            return View(data);
        }

        // ==========================
        // CETAK PDF
        // ==========================
        public IActionResult Cetak(DateTime? tanggalAwal, DateTime? tanggalAkhir)
        {
            var model = _service.GetLaporan(tanggalAwal, tanggalAkhir);

            var pdf = _service.GeneratePdf(model);

            return File(pdf,
                "application/pdf",
                "Laporan.pdf");
        }
    }
}