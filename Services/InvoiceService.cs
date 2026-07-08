using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemRumahMakan.Models;

namespace SistemRumahMakan.Services
{
    public class InvoiceService
    {
        public byte[] GeneratePdf(OrderHeader order)
        {
            if (order == null)
                throw new Exception("Order tidak ditemukan");

            if (order.Details == null || !order.Details.Any())
                throw new Exception("Detail order kosong");

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Content().Column(col =>
                    {
                        col.Item().Text("STRUK RUMAH MAKAN")
                            .FontSize(18)
                            .Bold();

                        col.Item().Text($"No Order : {order.NomorOrder}");
                        col.Item().Text($"Tanggal : {order.Tanggal}");

                        col.Item().LineHorizontal(1);

                        foreach (var item in order.Details)
                        {
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Text(item.Menu?.NamaMenu ?? "-");
                                row.ConstantItem(50).Text(item.Qty.ToString());
                                row.ConstantItem(100).Text(item.Subtotal.ToString("N0"));
                            });
                        }

                        col.Item().LineHorizontal(1);

                        col.Item().AlignRight().Text($"Total : {order.Total:N0}")
                            .Bold();
                    });
                });
            }).GeneratePdf();

            return pdf;
        }
    }
}