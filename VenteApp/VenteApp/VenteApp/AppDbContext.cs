using Microsoft.EntityFrameworkCore;

namespace VenteApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<SaleTransaction> SaleTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Get the path to the local application folder (cross-platform)
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Combine the folder path with the database file name
            var databasePath = Path.Combine(folderPath, "VenteApp2.db");

            // Log the database path (for debugging purposes)
            Console.WriteLine($"Database path: {databasePath}");

            // Use SQLite with the full database path
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleTransaction>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.ProductId);
        }

    }
}
