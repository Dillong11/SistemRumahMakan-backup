using System.ComponentModel.DataAnnotations;

namespace SistemRumahMakan.Models.ViewModels
{
    public class OrderCreateVM
    {
        [Required]
        public Guid MejaId { get; set; }

        public Guid MenuId { get; set; }

        public int Qty { get; set; }

        public List<CartItemVM> CartItems { get; set; } = new();

        public decimal GrandTotal => CartItems.Sum(x => x.Subtotal);
    }
}