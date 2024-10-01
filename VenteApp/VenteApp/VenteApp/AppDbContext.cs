using Microsoft.EntityFrameworkCore;

namespace VenteApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Products.db"); // SQLite database file path
        }
    }
}
