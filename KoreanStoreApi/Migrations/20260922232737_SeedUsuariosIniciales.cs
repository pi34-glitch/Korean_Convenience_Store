using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KoreanStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsuariosIniciales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "NombreUsuario", "PasswordHash", "Rol" },
                values: new object[,]
                {
                    { 1, true, "admin@bekeenco.com", "Kim Joung", "AQAAAAIAAYagAAAAEM8EaO0Rg8e9Raib7pv4VCLrxLKSKicMTiyT/+YyEJa79ZhZNBknB2bvte8V+5J7Gg==", "Administrador" },
                    { 2, true, "cajero@bekeenco.com", "Cajero BekeenCo", "AQAAAAIAAYagAAAAEInWBP1K0riknXa78mSa5Pq+3M4kPx1xjBfezYwwOqXDT1DtJwQRxa5a4xILfKMRLQ==", "CajeroVendedor" },
                    { 3, true, "cliente@bekeenco.com", "Cliente Demo", "AQAAAAIAAYagAAAAEHJQ8cHjxX6wCKEg9NSsJXhEylzfn9WEdMp39BSDv///Y/WK0W9zn1iAt+T0NlKfIA==", "Cliente" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
