using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Korean_Convenience_Store.Migrations
{
    /// <inheritdoc />
    public partial class Prueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Categoria = table.Column<string>(type: "text", nullable: false),
                    TipoCobro = table.Column<string>(type: "text", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StockActual = table.Column<int>(type: "integer", nullable: false),
                    StockSeguridad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreUsuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Rol = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Categoria", "Nombre", "PrecioVenta", "StockActual", "StockSeguridad", "TipoCobro" },
                values: new object[,]
                {
                    { 100, "Fideos", "Ramen Samyang 140g", 8.50m, 20, 5, "Unidad" },
                    { 101, "Bebidas", "Bebida Milkis 250ml", 6.00m, 15, 5, "Unidad" },
                    { 102, "Snacks", "Snack Pocky Chocolate", 5.50m, 25, 5, "Unidad" },
                    { 103, "Caldos", "Tteokbokki Picante", 12.00m, 10, 3, "Unidad" },
                    { 104, "Bebidas", "Soju Original 360ml", 18.00m, 8, 2, "Unidad" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "NombreUsuario", "PasswordHash", "Rol" },
                values: new object[,]
                {
                    { 1, true, "admin@bekeenco.com", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kim Joung", "AQAAAAIAAYagAAAAEAy+TwyNxBP60uI529mx6i7VYumar2q1oIHcjrrE1pJVUzQ+fLTPYMtXBhbmfkjsDg==", "Administrador" },
                    { 2, true, "cajero@bekeenco.com", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cajero BekeenCo", "AQAAAAIAAYagAAAAEJArxw5RUh1ystd/aQfkZ840AwGNGfAJBcAOSeT3bv1CZhoyFsqyvC+2sgajyjPqEg==", "Cajero" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
