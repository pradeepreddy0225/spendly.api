using Microsoft.EntityFrameworkCore;
using Spendly.API.Models;

namespace Spendly.API.Data;

public class SpendlyDbContext : DbContext
{
    public SpendlyDbContext(
        DbContextOptions<SpendlyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Amount)
                .HasPrecision(18, 2);

            entity.Property(x => x.Description)
                .HasMaxLength(250);

            entity.Property(x => x.Category)
                .HasMaxLength(100);

            entity.Property(x => x.PaymentMethod)
                .HasMaxLength(50);

            entity.Property(x => x.Source)
                .HasMaxLength(30);
        });
    }
}