using SistemRumahMakan.Models;

namespace SistemRumahMakan.Models.ViewModels
{
    public class DashboardVM
    {
        public int TotalKategori { get; set; }

        public int TotalMenu { get; set; }

        public int TotalMeja { get; set; }

        public int OrderHariIni { get; set; }

        public decimal PendapatanHariIni { get; set; }

        public decimal PendapatanBulanIni { get; set; }

        public int OrderProses { get; set; }

        public int OrderSelesai { get; set; }

        // Tambahan
        public List<MenuTerlarisVM> TopMenus { get; set; } = new();

        public List<OrderHeader> RecentOrders { get; set; } = new();
    }
}