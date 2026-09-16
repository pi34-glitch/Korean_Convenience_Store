using Korean_Convenience_Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Korean_Convenience_Store.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ===== DbSets =====
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }   // ← ESTA ES LA LÍNEA QUE FALTA

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============ CONFIGURACIÓN DE USUARIO ============
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasConversion<string>();

            // Data Seeding de Usuarios con hash seguro
            var hasher = new PasswordHasher<Usuario>();

            var admin = new Usuario
            {
                Id = 1,
                NombreUsuario = "Kim Joung",
                Email = "admin@bekeenco.com",
                Rol = RolUsuario.Administrador,
                Activo = true,
                FechaCreacion = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            var cajero = new Usuario
            {
                Id = 2,
                NombreUsuario = "Cajero BekeenCo",
                Email = "cajero@bekeenco.com",
                Rol = RolUsuario.Cajero,
                Activo = true,
                FechaCreacion = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            cajero.PasswordHash = hasher.HashPassword(cajero, "Cajero123!");

            modelBuilder.Entity<Usuario>().HasData(admin, cajero);

            // ============ CONFIGURACIÓN DE PRODUCTO ============
            modelBuilder.Entity<Producto>().ToTable("Productos");

            modelBuilder.Entity<Producto>()
                .Property(p => p.PrecioVenta)
                .HasColumnType("decimal(18,2)");

            // Data Seeding de Productos de ejemplo (HU-04)
            modelBuilder.Entity<Producto>().HasData(
                new Producto { Id = 100, Nombre = "Ramen Samyang 140g",  Categoria = "Fideos",  TipoCobro = "Unidad", PrecioVenta = 8.50m,  StockActual = 20, StockSeguridad = 5 },
                new Producto { Id = 101, Nombre = "Bebida Milkis 250ml",  Categoria = "Bebidas", TipoCobro = "Unidad", PrecioVenta = 6.00m,  StockActual = 15, StockSeguridad = 5 },
                new Producto { Id = 102, Nombre = "Snack Pocky Chocolate", Categoria = "Snacks", TipoCobro = "Unidad", PrecioVenta = 5.50m,  StockActual = 25, StockSeguridad = 5 },
                new Producto { Id = 103, Nombre = "Tteokbokki Picante",   Categoria = "Caldos",  TipoCobro = "Unidad", PrecioVenta = 12.00m, StockActual = 10, StockSeguridad = 3 },
                new Producto { Id = 104, Nombre = "Soju Original 360ml",   Categoria = "Bebidas", TipoCobro = "Unidad", PrecioVenta = 18.00m, StockActual = 8,  StockSeguridad = 2 }
            );
        }
    }
}