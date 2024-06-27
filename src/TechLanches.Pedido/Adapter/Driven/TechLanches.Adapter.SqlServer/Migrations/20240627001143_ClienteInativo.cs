using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechLanches.Adapter.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ClienteInativo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ClienteInativo",
                table: "Pedidos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteInativo",
                table: "Pedidos");
        }
    }
}
