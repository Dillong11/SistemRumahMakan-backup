using System.ComponentModel.DataAnnotations;

namespace SistemRumahMakan.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username wajib diisi.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password wajib diisi.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}