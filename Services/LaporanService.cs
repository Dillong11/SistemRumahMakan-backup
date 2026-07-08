using Microsoft.EntityFrameworkCore;
using SistemRumahMakan.Data;
using SistemRumahMakan.Models.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemRumahMakan.Models;

namespace SistemRumahMakan.Services
{
    public class LaporanService
    {
        private readonly ApplicationDbContext _context;

        public LaporanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public LaporanVM GetLaporan(DateTime? tanggalAwal, DateTime? tanggalAkhir)
        {
            var query = _context.OrderHeaders
                .Include(x => x.Meja)
                .OrderByDescending(x => x.Tanggal)
                .AsQueryable();

            if (tanggalAwal.HasValue)
            {
                query = query.Where(x => x.Tanggal.Date >= tanggalAwal.Value.Date);
            }

            if (tanggalAkhir.HasValue)
            {
                query = query.Where(x => x.Tanggal.Date <= tanggalAkhir.Value.Date);
            }

            return new LaporanVM
            {
                TanggalAwal = tanggalAwal,
                TanggalAkhir = tanggalAkhir,
                Orders = query.ToList()
            };
        }
        public List<TopMenuVM> GetMenuTerlaris()
        {
            return _context.OrderDetails
                .Include(x => x.Menu)
                .GroupBy(x => x.Menu!.NamaMenu)
                .Select(x => new TopMenuVM
                {
                    NamaMenu = x.Key,
                    TotalTerjual = x.Sum(x => x.Qty)
                })
                .OrderByDescending(x => x.TotalTerjual)
                .Take(10)
                .ToList();
        }
        public byte[] GeneratePdf(LaporanVM model)
        {
            return QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("LAPORAN TRANSAKSI RUMAH MAKAN")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Periode : {model.TanggalAwal:dd/MM/yyyy} - {model.TanggalAkhir:dd/MM/yyyy}");

                        col.Item().PaddingVertical(10);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("No Order").Bold();
                                header.Cell().Text("Tanggal").Bold();
                                header.Cell().Text("Meja").Bold();
                                header.Cell().Text("Status").Bold();
                                header.Cell().Text("Total").Bold();
                            });

                            foreach (var item in model.Orders)
                            {
                                table.Cell().Text(item.NomorOrder);
                                table.Cell().Text(item.Tanggal.ToString("dd/MM/yyyy"));
                                table.Cell().Text(item.Meja?.NomorMeja ?? "-");
                                table.Cell().Text(item.Status);
                                table.Cell().Text(item.Total.ToString("N0"));
                            }
                        });

                        col.Item().PaddingTop(20);

                        col.Item().AlignRight()
                            .Text($"Grand Total : Rp {model.GrandTotal:N0}")
                            .Bold();
                    });
                });
            }).GeneratePdf();
        }
    }
}