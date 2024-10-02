
namespace VenteApp
{
    public class SaleTransaction
    {
        public Guid Id { get; set; } // Primary key
        public Guid ProductId { get; set; } // Foreign key to Product
        public Product Product { get; set; } // Navigation property to Product

        public int Quantite { get; set; } // Quantity sold
        public DateTime DateDeVente { get; set; } // Date of sale
    }


}
