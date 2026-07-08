using SistemRumahMakan.Data;
using SistemRumahMakan.Models;

namespace SistemRumahMakan.Services
{
    public class MejaService
    {
        private readonly ApplicationDbContext _context;

        public MejaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Meja> GetAll(string? keyword = null)
        {
            var query = _context.Mejas.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    x.NomorMeja.Contains(keyword));
            }

            return query
                .OrderBy(x => x.NomorMeja)
                .ToList();
        }

        public Meja? GetById(Guid id)
        {
            return _context.Mejas.Find(id);
        }

        public void Create(Meja model)
        {
            _context.Mejas.Add(model);
            _context.SaveChanges();
        }

        public void Update(Meja model)
        {
            _context.Mejas.Update(model);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var data = _context.Mejas.Find(id);

            if (data != null)
            {
                _context.Mejas.Remove(data);
                _context.SaveChanges();
            }
        }
    }
}