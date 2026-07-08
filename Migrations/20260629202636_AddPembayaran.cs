using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemRumahMakan.Migrations
{
    /// <inheritdoc />
    public partial class AddPembayaran : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Bayar",
                table: "OrderHeaders",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Kembalian",
                table: "OrderHeaders",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bayar",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Kembalian",
                table: "OrderHeaders");
        }
    }
}
