using Microsoft.EntityFrameworkCore;
using VenteApp;

public class VenteDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use SQLite database with a local file
        optionsBuilder.UseSqlite($"Filename={Path.Combine(FileSystem.AppDataDirectory, "ventes.db")}");
    }
}
