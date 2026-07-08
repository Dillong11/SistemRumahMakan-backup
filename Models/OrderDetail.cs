using System.ComponentModel.DataAnnotations;

namespace SistemRumahMakan.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderHeaderId { get; set; }

        public Guid MenuId { get; set; }

        public int Qty { get; set; }

        public decimal Harga { get; set; }

        public decimal Subtotal { get; set; }

        public virtual OrderHeader? OrderHeader { get; set; }

        public virtual Menu? Menu { get; set; }
    }
}