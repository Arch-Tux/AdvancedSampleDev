using Microsoft.EntityFrameworkCore;
using AdvancedSampleDev.Infrastructure.Entities;

namespace AdvancedSampleDev.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<SupplierEntity> Suppliers => Set<SupplierEntity>();
    public DbSet<ProductSupplierEntity> ProductSuppliers => Set<ProductSupplierEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de ProductEntity
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(300).IsRequired();
            entity.Property(e => e.PriceHt).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.TvaType).HasConversion<int>().IsRequired(); // Stocké comme int en BDD
            entity.Property(e => e.IsActive).IsRequired();
        });

        // Configuration de SupplierEntity
        modelBuilder.Entity<SupplierEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        });

        // Configuration de la table de jointure ProductSupplierEntity
        modelBuilder.Entity<ProductSupplierEntity>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.SupplierId });

            entity.HasOne(e => e.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Supplier)
                .WithMany(s => s.ProductSuppliers)
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

