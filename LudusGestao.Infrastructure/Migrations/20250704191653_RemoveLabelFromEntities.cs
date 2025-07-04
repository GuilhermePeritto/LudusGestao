using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LudusGestao.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLabelFromEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Label",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Locais");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Clientes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Locais",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
