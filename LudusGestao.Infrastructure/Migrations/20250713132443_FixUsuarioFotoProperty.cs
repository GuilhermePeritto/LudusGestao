using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LudusGestao.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUsuarioFotoProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiraEm",
                table: "Usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Usuarios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiraEm",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
