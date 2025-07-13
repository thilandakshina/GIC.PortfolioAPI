using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Domain;
using System;

namespace PortfolioAPI.Infrastructure
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) { }

        public DbSet<Portfolio> Portfolios { get; set; } = null!;
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<PortfolioAsset> PortfolioAssets { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Portfolio>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.Name)
                .IsUnique();

            modelBuilder.Entity<PortfolioAsset>()
                .HasKey(pa => new { pa.PortfolioId, pa.AssetId });

            modelBuilder.Entity<PortfolioAsset>()
                .HasOne(pa => pa.Portfolio)
                .WithMany(p => p.PortfolioAssets)
                .HasForeignKey(pa => pa.PortfolioId);

            modelBuilder.Entity<PortfolioAsset>()
                .HasOne(pa => pa.Asset)
                .WithMany(a => a.PortfolioAssets)
                .HasForeignKey(pa => pa.AssetId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Portfolio)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.PortfolioId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Asset)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AssetId);

            modelBuilder.Entity<Asset>().HasData(
                new Asset { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Apple Inc.", CurrentPrice = 150.00m },
                new Asset { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Microsoft Corp.", CurrentPrice = 300.00m }
            );
        }
    }
} 