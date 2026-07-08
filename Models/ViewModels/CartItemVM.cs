namespace SistemRumahMakan.Models.ViewModels
{
    public class CartItemVM
    {
        public Guid MenuId { get; set; }

        public string NamaMenu { get; set; } = string.Empty;

        public decimal Harga { get; set; }

        public int Qty { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Harga * Qty;
            }
        }
    }
}