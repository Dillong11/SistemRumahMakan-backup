using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemRumahMakan.Helpers;
using SistemRumahMakan.Models;
using SistemRumahMakan.Models.ViewModels;
using SistemRumahMakan.Services;



namespace SistemRumahMakan.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _service;
        private readonly InvoiceService _invoice;

        // ✅ FIX: HANYA 1 CONSTRUCTOR
        public OrderController(OrderService service, InvoiceService invoice)
        {
            _service = service;
            _invoice = invoice;
        }

        // =========================
        // LIST ORDER
        // =========================
        public IActionResult Index()
        {
            var orders = _service.GetOrders();
            return View(orders);
        }

        // =========================
        // CREATE
        // =========================
        public IActionResult Create()
        {
            ViewBag.Menu = new SelectList(
                _service.GetMenus(),
                "Id",
                "NamaMenu");

            ViewBag.Meja = new SelectList(
                _service.GetMejas(),
                "Id",
                "NomorMeja");

            var vm = new OrderCreateVM();

            vm.CartItems = HttpContext.Session
                .GetObject<List<CartItemVM>>("Cart")
                ?? new List<CartItemVM>();

            // ambil meja dari session
            var mejaSession = HttpContext.Session.GetString("MejaId");

            if (!string.IsNullOrEmpty(mejaSession))
            {
                vm.MejaId = Guid.Parse(mejaSession);
            }

            return View(vm);
        }

        // =========================
        // ADD TO CART
        // =========================
        [HttpPost]
        public IActionResult AddToCart(OrderCreateVM model)
        {
            if (model.MejaId == Guid.Empty)
            {
                TempData["Error"] = "Silakan pilih meja.";
                return RedirectToAction(nameof(Create));
            }

            if (model.Qty <= 0)
            {
                TempData["Error"] = "Qty harus lebih dari 0.";
                return RedirectToAction(nameof(Create));
            }

            // simpan meja ke session
            HttpContext.Session.SetString("MejaId", model.MejaId.ToString());

            var menu = _service.GetMenus()
                .FirstOrDefault(x => x.Id == model.MenuId);

            if (menu == null)
            {
                TempData["Error"] = "Menu tidak ditemukan.";
                return RedirectToAction(nameof(Create));
            }

            var cart = HttpContext.Session
                .GetObject<List<CartItemVM>>("Cart")
                ?? new List<CartItemVM>();

            var item = cart.FirstOrDefault(x => x.MenuId == model.MenuId);

            if (item == null)
            {
                cart.Add(new CartItemVM
                {
                    MenuId = menu.Id,
                    NamaMenu = menu.NamaMenu,
                    Harga = menu.Harga,
                    Qty = model.Qty
                });
            }
            else
            {
                item.Qty += model.Qty;
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction(nameof(Create));
        }

        // =========================
        // REMOVE CART
        // =========================
        public IActionResult Remove(Guid id)
        {
            var cart = HttpContext.Session
                .GetObject<List<CartItemVM>>("Cart")
                ?? new List<CartItemVM>();

            var item = cart.FirstOrDefault(x => x.MenuId == id);

            if (item != null)
            {
                cart.Remove(item);
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction(nameof(Create));
        }

        // =========================
        // SAVE ORDER
        // =========================
        [HttpPost]
        public IActionResult Save(OrderCreateVM model)
        {
            var cart = HttpContext.Session
                .GetObject<List<CartItemVM>>("Cart");

            var mejaSession = HttpContext.Session.GetString("MejaId");

            if (string.IsNullOrEmpty(mejaSession))
            {
                TempData["Error"] = "Silakan pilih meja.";
                return RedirectToAction(nameof(Create));
            }

            var mejaId = Guid.Parse(mejaSession);

            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Keranjang masih kosong.";
                return RedirectToAction(nameof(Create));
            }

            try
            {
                _service.SimpanOrder(mejaId, cart);

                HttpContext.Session.Remove("Cart");
                HttpContext.Session.Remove("MejaId");

                TempData["Success"] = "Transaksi berhasil disimpan.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Create));
            }
        }

        // =========================
        // DETAIL
        // =========================
        public IActionResult Detail(Guid id)
        {
            var order = _service.GetOrderById(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // =========================
        // EDIT (GET)
        // =========================
        public IActionResult Edit(Guid id)
        {
            var order = _service.GetOrderById(id);

            if (order == null)
                return NotFound();

            ViewBag.Meja = new SelectList(
                _service.GetMejas(),
                "Id",
                "NomorMeja",
                order.MejaId);

            return View(order);
        }

        
        // =========================
        // DELETE
        // =========================
        public IActionResult Delete(Guid id)
        {
            var order = _service.GetOrderById(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.DeleteOrder(id);

            TempData["Success"] = "Transaksi berhasil dihapus.";

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // BAYAR (GET)
        // =========================
        public IActionResult Bayar(Guid id)
        {
            var order = _service.GetOrderById(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // =========================
        // BAYAR (POST)
        // =========================
        [HttpPost]
        public IActionResult Bayar(Guid id, decimal bayar)
        {
            try
            {
                _service.BayarOrder(id, bayar);

                TempData["Success"] = "Pembayaran berhasil";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Bayar), new { id });
            }
        }

        // =========================
        // CETAK STRUK
        // =========================
        public IActionResult Cetak(Guid id)
        {
            var order = _service.GetOrderById(id);

            if (order == null)
                return NotFound();

            var pdf = _invoice.GeneratePdf(order);

            return File(pdf, "application/pdf", $"Struk_{order.NomorOrder}.pdf");
        }
        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderHeader model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Meja = new SelectList(
                    _service.GetMejas(),
                    "Id",
                    "NomorMeja",
                    model.MejaId);

                return View(model);
            }

            try
            {
                _service.UpdateOrder(model);

                TempData["Success"] = "Data berhasil diubah.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Meja = new SelectList(
                    _service.GetMejas(),
                    "Id",
                    "NomorMeja",
                    model.MejaId);

                TempData["Error"] = ex.Message;

                return View(model);
            }
        }
    }
}