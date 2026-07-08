using SistemRumahMakan.Models;

namespace SistemRumahMakan.Models.ViewModels
{
    public class LaporanVM
    {
        public DateTime? TanggalAwal { get; set; }

        public DateTime? TanggalAkhir { get; set; }

        public List<OrderHeader> Orders { get; set; } = new();

        public decimal GrandTotal
        {
            get
            {
                return Orders.Sum(x => x.Total);
            }
        }
    }
}