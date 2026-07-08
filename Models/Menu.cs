using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace SistemRumahMakan.Models
{
    public class Menu
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid KategoriMenuId { get; set; }

        [Required]
        [StringLength(150)]
        public string NamaMenu { get; set; } = string.Empty;

        [Required]
        public decimal Harga { get; set; }

        public int Stok { get; set; }

        public string? Deskripsi { get; set; }

        public string? Foto { get; set; }

        [NotMapped]
        public IFormFile? FotoFile { get; set; }

        public bool Status { get; set; } = true;

        [ForeignKey("KategoriMenuId")]
        public virtual KategoriMenu? KategoriMenu { get; set; }
    }
}