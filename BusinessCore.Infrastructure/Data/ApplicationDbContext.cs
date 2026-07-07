using BusinessCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessCore.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets para todas las entidades
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todas las configuraciones
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Seed data inicial
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Roles por defecto
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin", Description = "Super Administrator", IsActive = true },
                new Role { Id = 2, Name = "Admin", Description = "Administrator", IsActive = true },
                new Role { Id = 3, Name = "Manager", Description = "Manager", IsActive = true },
                new Role { Id = 4, Name = "Employee", Description = "Employee", IsActive = true },
                new Role { Id = 5, Name = "Customer", Description = "Customer", IsActive = true }
            );

            // Categorías por defecto
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electrónicos", Description = "Productos electrónicos", Slug = "electronicos", DisplayOrder = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Category { Id = 2, Name = "Ropa", Description = "Prendas de vestir", Slug = "ropa", DisplayOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Category { Id = 3, Name = "Hogar", Description = "Productos para el hogar", Slug = "hogar", DisplayOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Category { Id = 4, Name = "Deportes", Description = "Artículos deportivos", Slug = "deportes", DisplayOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow }
            );

            // Marcas por defecto
            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Samsung", Description = "Samsung Electronics", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Brand { Id = 2, Name = "Apple", Description = "Apple Inc.", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Brand { Id = 3, Name = "Sony", Description = "Sony Corporation", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Brand { Id = 4, Name = "Nike", Description = "Nike Inc.", IsActive = true, CreatedAt = DateTime.UtcNow }
            );

            // Almacenes por defecto
            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { Id = 1, Name = "Almacén Principal", Code = "W001", Address = "Calle Principal 123", City = "Ciudad", State = "Estado", Country = "País", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Warehouse { Id = 2, Name = "Almacén Secundario", Code = "W002", Address = "Avenida Secundaria 456", City = "Ciudad", State = "Estado", Country = "País", IsActive = true, CreatedAt = DateTime.UtcNow }
            );
        }
    }
}