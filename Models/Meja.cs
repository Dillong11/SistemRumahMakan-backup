using System.ComponentModel.DataAnnotations;

namespace SistemRumahMakan.Models
{
    public class Meja
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(20)]
        public string NomorMeja { get; set; } = string.Empty;

        [Required]
        public int Kapasitas { get; set; }

        [Required]
        public string Status { get; set; } = "Kosong";
    }
}