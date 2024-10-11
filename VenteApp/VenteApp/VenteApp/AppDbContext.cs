using Microsoft.EntityFrameworkCore;

namespace VenteApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<SaleTransaction> SaleTransactions { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Get the path to the local application folder (cross-platform)
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Combine the folder path with the database file name
            var databasePath = Path.Combine(folderPath, "VenteApp5.db");

            // Log the database path (for debugging purposes)
            Console.WriteLine($"Database path: {databasePath}");

            // Use SQLite with the full database path
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // SaleTransaction to Product
            modelBuilder.Entity<SaleTransaction>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.ProductId);

            // SaleTransaction to Client
            modelBuilder.Entity<SaleTransaction>()
                .HasOne(s => s.Client)
                .WithMany(c => c.Transactions)
                .HasForeignKey(s => s.ClientId);

            // Product to Provider (Foreign key ProviderId)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Provider)
                .WithMany(pr => pr.ProductsSupplied)
                .HasForeignKey(p => p.ProviderId);
        }
    }
}
