using Microsoft.EntityFrameworkCore;
using SistemRumahMakan.Models;

namespace SistemRumahMakan.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<KategoriMenu> KategoriMenus { get; set; }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Meja> Mejas { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        
    }
}