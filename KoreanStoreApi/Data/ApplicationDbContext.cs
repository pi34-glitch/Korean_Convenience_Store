using KoreanStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KoreanStoreApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Reseña> Reseñas { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Email único en Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Enums como string
            modelBuilder.Entity<Usuario>().Property(u => u.Rol).HasConversion<string>();
            modelBuilder.Entity<Producto>().Property(p => p.Tipo_venta).HasConversion<string>();
            modelBuilder.Entity<Venta>().Property(v => v.Metodo_pago).HasConversion<string>();

            // Relaciones explícitas
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Proveedor)
                .WithMany(pr => pr.Productos)
                .HasForeignKey(p => p.Id_proveedor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reseña>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reseñas)
                .HasForeignKey(r => r.Id_user)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reseña>()
                .HasOne(r => r.Producto)
                .WithMany(p => p.Reseñas)
                .HasForeignKey(r => r.Id_producto)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Carrito>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Carritos)
                .HasForeignKey(c => c.Id_user)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Carrito>()
                .HasOne(c => c.Producto)
                .WithMany(p => p.Carritos)
                .HasForeignKey(c => c.Id_producto)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Ventas)
                .HasForeignKey(v => v.Id_user)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Venta)
                .WithMany(v => v.Detalles)
                .HasForeignKey(d => d.Id_venta)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetallesVenta)
                .HasForeignKey(d => d.Id_producto)
                .OnDelete(DeleteBehavior.Restrict);
            // Seed de usuario Admin inicial
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Usuario>();
            var admin = new Usuario
            {
                Id = 1,
                NombreUsuario = "Kim Joung",
                Email = "admin@bekeenco.com",
                Rol = RolUsuario.Administrador,
                Activo = true
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            var cajero = new Usuario
            {
                Id = 2,
                NombreUsuario = "Cajero BekeenCo",
                Email = "cajero@bekeenco.com",
                Rol = RolUsuario.CajeroVendedor,
                Activo = true
            };
            cajero.PasswordHash = hasher.HashPassword(cajero, "Cajero123!");

            var cliente = new Usuario
            {
                Id = 3,
                NombreUsuario = "Cliente Demo",
                Email = "cliente@bekeenco.com",
                Rol = RolUsuario.Cliente,
                Activo = true
            };
            cliente.PasswordHash = hasher.HashPassword(cliente, "Cliente123!");

            modelBuilder.Entity<Usuario>().HasData(admin, cajero, cliente);
        }
    }
}