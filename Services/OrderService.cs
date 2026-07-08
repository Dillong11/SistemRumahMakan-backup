using Microsoft.EntityFrameworkCore;
using SistemRumahMakan.Data;
using SistemRumahMakan.Models;
using SistemRumahMakan.Models.ViewModels;

namespace SistemRumahMakan.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // MENU
        // ==========================
        public List<Menu> GetMenus()
        {
            return _context.Menus
                .Include(x => x.KategoriMenu)
                .Where(x => x.Status && x.Stok > 0)
                .OrderBy(x => x.NamaMenu)
                .ToList();
        }

        // ==========================
        // MEJA
        // ==========================
        public List<Meja> GetMejas()
        {
            return _context.Mejas
                .Where(x => x.Status == "Kosong")
                .OrderBy(x => x.NomorMeja)
                .ToList();
        }

        // ==========================
        // NOMOR ORDER
        // ==========================
        public string GenerateNomorOrder()
        {
            string tanggal = DateTime.Now.ToString("yyyyMMdd");

            int jumlahHariIni = _context.OrderHeaders
                .Count(x => x.Tanggal.Date == DateTime.Today);

            return $"ORD{tanggal}{(jumlahHariIni + 1):D4}";
        }

        // ==========================
        // SIMPAN ORDER
        // ==========================
        public void SimpanOrder(Guid mejaId, List<CartItemVM> cart)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var meja = _context.Mejas.FirstOrDefault(x => x.Id == mejaId);

                if (meja == null)
                    throw new Exception("Meja tidak ditemukan");

                var header = new OrderHeader
                {
                    Id = Guid.NewGuid(),
                    NomorOrder = GenerateNomorOrder(),
                    Tanggal = DateTime.Now,
                    MejaId = mejaId,
                    Status = "Proses",
                    Total = 0
                };

                _context.OrderHeaders.Add(header);

                decimal total = 0;

                foreach (var item in cart)
                {
                    var menu = _context.Menus.FirstOrDefault(x => x.Id == item.MenuId);

                    if (menu == null)
                        continue;

                    if (menu.Stok < item.Qty)
                        throw new Exception($"Stok {menu.NamaMenu} tidak cukup");

                    var subtotal = menu.Harga * item.Qty;

                    var detail = new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        OrderHeaderId = header.Id,
                        MenuId = menu.Id,
                        Qty = item.Qty,
                        Harga = menu.Harga,
                        Subtotal = subtotal
                    };

                    _context.OrderDetails.Add(detail);

                    menu.Stok -= item.Qty;

                    total += subtotal;
                }

                header.Total = total;

                meja.Status = "Terisi";

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // ==========================
        // LIST ORDER
        // ==========================
        public List<OrderHeader> GetOrders()
        {
            return _context.OrderHeaders
                .Include(x => x.Meja)
                .OrderByDescending(x => x.Tanggal)
                .ToList();
        }

        // ==========================
        // DETAIL (🔥 FIX PENTING)
        // ==========================
        public OrderHeader? GetOrderById(Guid id)
        {
            return _context.OrderHeaders
                .Include(x => x.Meja)
                .Include(x => x.Details)
                    .ThenInclude(x => x.Menu)
                .FirstOrDefault(x => x.Id == id);
        }
        // ==========================
        // UPDATE ORDER
        // ==========================
        public void UpdateOrder(OrderHeader model)
        {
            var order = _context.OrderHeaders
                .FirstOrDefault(x => x.Id == model.Id);

            if (order == null)
                throw new Exception("Order tidak ditemukan.");

            order.MejaId = model.MejaId;
            order.Status = model.Status;

            _context.SaveChanges();
        }

        // ==========================
        // DELETE
        // ==========================
        public void DeleteOrder(Guid id)
        {
            var order = _context.OrderHeaders
                .Include(x => x.Details)
                .FirstOrDefault(x => x.Id == id);

            if (order == null)
                return;

            _context.OrderDetails.RemoveRange(order.Details!);
            _context.OrderHeaders.Remove(order);

            _context.SaveChanges();
        }

        // ==========================
        // BAYAR
        // ==========================
        public void BayarOrder(Guid orderId, decimal bayar)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var order = _context.OrderHeaders
                    .Include(x => x.Meja)
                    .FirstOrDefault(x => x.Id == orderId);

                if (order == null)
                    throw new Exception("Order tidak ditemukan");

                if (bayar < order.Total)
                    throw new Exception("Uang tidak cukup");

                order.Bayar = bayar;
                order.Kembalian = bayar - order.Total;
                order.Status = "Selesai";

                if (order.Meja != null)
                {
                    order.Meja.Status = "Kosong";
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}