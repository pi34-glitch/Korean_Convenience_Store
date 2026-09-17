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
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Resena> Resenas { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================================================
            // USUARIO
            // =========================================================
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.NombreUsuario)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(u => u.PasswordHash).IsRequired();

                entity.Property(u => u.Rol)
                      .HasConversion<string>()
                      .IsRequired();

                entity.Property(u => u.Activo).HasDefaultValue(true);
            });

            // =========================================================
            // PROVEEDOR
            // =========================================================
            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.ToTable("Proveedores");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.Contacto).HasMaxLength(20);
                entity.Property(p => p.Telefono).HasMaxLength(20);
                entity.Property(p => p.Direccion).HasMaxLength(150);
            });

            // =========================================================
            // PRODUCTO
            // =========================================================
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Productos");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasColumnName("Id_producto");
                entity.Property(p => p.IdProveedor).HasColumnName("Id_proveedor");
                entity.Property(p => p.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.TipoVenta)
                      .HasColumnName("Tipo_venta")
                      .HasConversion<string>()
                      .IsRequired();

                entity.Property(p => p.PrecioUnitario)
                      .HasColumnName("Precio_unitario")
                      .HasColumnType("decimal(10,2)");

                entity.Property(p => p.StockActual)
                      .HasColumnName("Stock_actual")
                      .HasColumnType("decimal(10,2)");

                entity.Property(p => p.StockMinimo)
                      .HasColumnName("Stock_minimo")
                      .HasColumnType("decimal(10,2)");

                // Relación: Proveedor (1) → Productos (N)
                entity.HasOne(p => p.Proveedor)
                      .WithMany(pr => pr.Productos)
                      .HasForeignKey(p => p.IdProveedor)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================================================
            // RESENA
            // =========================================================
            modelBuilder.Entity<Resena>(entity =>
            {
                entity.ToTable("Resenas");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.IdUser).HasColumnName("Id_user");
                entity.Property(r => r.IdProducto).HasColumnName("Id_producto");

                entity.Property(r => r.Calificacion).IsRequired();

                entity.Property(r => r.Comentario).HasColumnType("text");
                entity.Property(r => r.Fecha).IsRequired();

                entity.HasOne(r => r.Usuario)
                      .WithMany(u => u.Resenas)
                      .HasForeignKey(r => r.IdUser)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Producto)
                      .WithMany(p => p.Resenas)
                      .HasForeignKey(r => r.IdProducto)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================================================
            // CARRITO
            // =========================================================
            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.ToTable("Carrito");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id).HasColumnName("Id_carrito");
                entity.Property(c => c.IdUser).HasColumnName("Id_user");
                entity.Property(c => c.IdProducto).HasColumnName("Id_producto");

                entity.Property(c => c.CantidadUnitario)
                      .HasColumnName("CantidadUnitario")
                      .IsRequired();

                entity.Property(c => c.CantidadGramo)
                      .HasColumnName("CantidadGramo")
                      .HasColumnType("decimal(10,2)");

                entity.Property(c => c.Subtotal)
                      .HasColumnName("Subtotal")
                      .HasColumnType("decimal(10,2)");

                entity.HasOne(c => c.Usuario)
                      .WithMany(u => u.Carritos)
                      .HasForeignKey(c => c.IdUser)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Producto)
                      .WithMany(p => p.Carritos)
                      .HasForeignKey(c => c.IdProducto)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================================================
            // VENTA
            // =========================================================
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.ToTable("Ventas");
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Id).HasColumnName("Id_venta");
                entity.Property(v => v.IdUser).HasColumnName("Id_user");
                entity.Property(v => v.FechaVenta).HasColumnName("Fecha_venta").IsRequired();

                entity.Property(v => v.Total)
                      .HasColumnType("decimal(10,2)");

                entity.Property(v => v.MetodoPago)
                      .HasColumnName("Metodo_pago")
                      .HasConversion<string>()
                      .IsRequired();

                entity.HasOne(v => v.Usuario)
                      .WithMany(u => u.Ventas)
                      .HasForeignKey(v => v.IdUser)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================================================
            // DETALLE VENTA
            // =========================================================
            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.ToTable("DetalleVenta");
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Id).HasColumnName("Id_detalle");
                entity.Property(d => d.IdVenta).HasColumnName("Id_venta");
                entity.Property(d => d.IdProducto).HasColumnName("Id_producto");

                entity.Property(d => d.CantidadOGramos)
                      .HasColumnName("Cantidad_o_gramos")
                      .HasColumnType("decimal(10,2)");

                entity.Property(d => d.PrecioAplicado)
                      .HasColumnName("Precio_aplicado")
                      .HasColumnType("decimal(10,2)");

                entity.Property(d => d.Subtotal)
                      .HasColumnName("Subtotal")
                      .HasColumnType("decimal(10,2)");

                entity.HasOne(d => d.Venta)
                      .WithMany(v => v.DetallesVenta)
                      .HasForeignKey(d => d.IdVenta)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Producto)
                      .WithMany(p => p.DetallesVenta)
                      .HasForeignKey(d => d.IdProducto)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================================================
            // DATA SEEDING
            // =========================================================
            var hasher = new PasswordHasher<Usuario>();

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
                Rol = RolUsuario.Cajero,
                Activo = true
            };
            cajero.PasswordHash = hasher.HashPassword(cajero, "Cajero123!");

            var cliente = new Usuario
            {
                Id = 3,
                NombreUsuario = "Cliente Kiosco",
                Email = "cliente@bekeenco.com",
                Rol = RolUsuario.Cliente,
                Activo = true
            };
            cliente.PasswordHash = hasher.HashPassword(cliente, "Cliente123!");

            modelBuilder.Entity<Usuario>().HasData(admin, cajero, cliente);
        }
    }
}