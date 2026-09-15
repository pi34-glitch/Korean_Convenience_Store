using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Korean_Convenience_Store.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "NombreUsuario", "PasswordHash", "Rol" },
                values: new object[,]
                {
                    { 1, true, "admin@bekeenco.com", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kim Joung", "AQAAAAIAAYagAAAAEKKrLhqj8+YIl4ppE1QJWSvsY/f3133Fnbsls6sYHDIp9eUel8H/+gWwWSwzm9Zgow==", "Administrador" },
                    { 2, true, "cajero@bekeenco.com", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cajero BekeenCo", "AQAAAAIAAYagAAAAEFkIySsbItm/NM+LEzz0rjSXkDhmhQBmB5LXcSXRAXJ/BlSXLeT80UvfbRueR/09lQ==", "Cajero" }
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
                name: "Usuarios");
        }
    }
}
