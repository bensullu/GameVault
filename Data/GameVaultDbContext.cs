using Microsoft.EntityFrameworkCore;
using GameVault.Models;

namespace GameVault.Data;

public class GameVaultDbContext : DbContext
{
    public GameVaultDbContext(DbContextOptions<GameVaultDbContext> options) : base(options) { }

    public DbSet<Studio> Studios => Set<Studio>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Platform> Platforms => Set<Platform>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<GamePlatform> GamePlatforms => Set<GamePlatform>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Composite primary key for the junction table
        modelBuilder.Entity<GamePlatform>()
            .HasKey(gp => new { gp.GameId, gp.PlatformId });

        // Game many-to-many Platform via GamePlatform
        modelBuilder.Entity<GamePlatform>()
            .HasOne(gp => gp.Game)
            .WithMany(g => g.GamePlatforms)
            .HasForeignKey(gp => gp.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GamePlatform>()
            .HasOne(gp => gp.Platform)
            .WithMany(p => p.GamePlatforms)
            .HasForeignKey(gp => gp.PlatformId)
            .OnDelete(DeleteBehavior.Cascade);

        // Game - Studio (many-to-one)
        modelBuilder.Entity<Game>()
            .HasOne(g => g.Studio)
            .WithMany(s => s.Games)
            .HasForeignKey(g => g.StudioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Game - Genre (many-to-one)
        modelBuilder.Entity<Game>()
            .HasOne(g => g.Genre)
            .WithMany(ge => ge.Games)
            .HasForeignKey(g => g.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        // Game - Review (one-to-many)
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Game)
            .WithMany(g => g.Reviews)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        // Decimal precision for price
        modelBuilder.Entity<Game>()
            .Property(g => g.Price)
            .HasColumnType("decimal(10,2)");
    }
}
