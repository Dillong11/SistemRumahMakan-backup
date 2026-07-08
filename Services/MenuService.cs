using Microsoft.EntityFrameworkCore;
using SistemRumahMakan.Data;
using SistemRumahMakan.Models;

namespace SistemRumahMakan.Services
{
    public class MenuService
    {
        private readonly ApplicationDbContext _context;

        public MenuService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Menu> GetAll(string? keyword = null)
        {
            var query = _context.Menus
                .Include(x => x.KategoriMenu)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.NamaMenu.Contains(keyword));
            }

            return query
                .OrderBy(x => x.NamaMenu)
                .ToList();
        }

        public Menu? GetById(Guid id)
        {
            return _context.Menus
                .FirstOrDefault(x => x.Id == id);
        }

        public List<KategoriMenu> GetKategori()
        {
            return _context.KategoriMenus
                .OrderBy(x => x.NamaKategori)
                .ToList();
        }

        public void Create(Menu model)
        {
            _context.Menus.Add(model);
            _context.SaveChanges();
        }

        public void Update(Menu model)
        {
            _context.Menus.Update(model);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var data = _context.Menus.Find(id);

            if (data != null)
            {
                _context.Menus.Remove(data);
                _context.SaveChanges();
            }
        }
    }
}