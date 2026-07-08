using SistemRumahMakan.Data;
using SistemRumahMakan.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace SistemRumahMakan.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public DashboardVM GetDashboard()
        {
            var vm = new DashboardVM();

            vm.TotalKategori = _context.KategoriMenus.Count();

            vm.TotalMenu = _context.Menus.Count();

            vm.TotalMeja = _context.Mejas.Count();

            vm.OrderHariIni = _context.OrderHeaders
                .Count(x => x.Tanggal.Date == DateTime.Today);

            vm.PendapatanHariIni = _context.OrderHeaders
                .Where(x => x.Status == "Selesai"
                    && x.Tanggal.Date == DateTime.Today)
                .Sum(x => (decimal?)x.Total) ?? 0;

            vm.PendapatanBulanIni = _context.OrderHeaders
                .Where(x => x.Status == "Selesai"
                    && x.Tanggal.Month == DateTime.Today.Month
                    && x.Tanggal.Year == DateTime.Today.Year)
                .Sum(x => (decimal?)x.Total) ?? 0;

            vm.OrderProses = _context.OrderHeaders
                .Count(x => x.Status == "Proses");

            vm.OrderSelesai = _context.OrderHeaders
                .Count(x => x.Status == "Selesai");

            vm.TopMenus = _context.OrderDetails
                .Include(x => x.Menu)
                .GroupBy(x => x.Menu!.NamaMenu)
                .Select(x => new MenuTerlarisVM
                {
                    NamaMenu = x.Key,
                    TotalTerjual = x.Sum(y => y.Qty)
                })
                .OrderByDescending(x => x.TotalTerjual)
                .Take(5)
                .ToList();

            vm.RecentOrders = _context.OrderHeaders
                .Include(x => x.Meja)
                .OrderByDescending(x => x.Tanggal)
                .Take(5)
                .ToList();

            return vm;
        }
    }
}