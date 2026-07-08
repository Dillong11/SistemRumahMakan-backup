  using System.ComponentModel.DataAnnotations;

namespace SistemRumahMakan.Models
{
    public class KategoriMenu
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string NamaKategori { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Deskripsi { get; set; }
        public virtual ICollection<Menu>? Menus { get; set; }
    }
}