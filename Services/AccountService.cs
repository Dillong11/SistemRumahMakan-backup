using SistemRumahMakan.Models;

namespace SistemRumahMakan.Services
{
    public class AccountService
    {
        private readonly List<User> _users = new()
        {
            new User
            {
                Username = "admin",
                Password = "admin123",
                Nama = "Administrator"
            }
        };

        public User? Login(string username, string password)
        {
            return _users.FirstOrDefault(x =>
                x.Username == username &&
                x.Password == password);
        }
    }
}