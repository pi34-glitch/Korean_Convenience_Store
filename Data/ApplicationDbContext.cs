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

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar tabla
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");

            // Email único
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Guardar Rol como string en la BD (más legible)
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasConversion<string>();

            // Data Seeding con hash seguro
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
        }
    }
}