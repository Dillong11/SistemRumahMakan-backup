using System.ComponentModel.DataAnnotations;

namespace SistemRumahMakan.Models
{
    public class OrderHeader
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string NomorOrder { get; set; } = string.Empty;

        public DateTime Tanggal { get; set; } = DateTime.Now;

        [Required]
        public Guid MejaId { get; set; }

        public decimal Total { get; set; }

        public string Status { get; set; } = "Proses";

        public virtual Meja? Meja { get; set; }

        public virtual ICollection<OrderDetail>? Details { get; set; }
        public decimal Bayar { get; set; }
        public decimal Kembalian { get; set; }
    }
}