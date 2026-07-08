using Microsoft.EntityFrameworkCore;
using SistemRumahMakan.Data;
using SistemRumahMakan.Models;

namespace SistemRumahMakan.Services
{
    public class KategoriService
    {
        private readonly ApplicationDbContext _context;

        public KategoriService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<KategoriMenu> GetAll(string? keyword = null)
        {
            var query = _context.KategoriMenus.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.NamaKategori.Contains(keyword));
            }

            return query
                .OrderBy(x => x.NamaKategori)
                .ToList();
        }

        public KategoriMenu? GetById(Guid id)
        {
            return _context.KategoriMenus.FirstOrDefault(x => x.Id == id);
        }

        public void Create(KategoriMenu model)
        {
            _context.KategoriMenus.Add(model);
            _context.SaveChanges();
        }

        public void Update(KategoriMenu model)
        {
            _context.KategoriMenus.Update(model);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var data = _context.KategoriMenus.Find(id);

            if (data != null)
            {
                _context.KategoriMenus.Remove(data);
                _context.SaveChanges();
            }
        }
    }
}