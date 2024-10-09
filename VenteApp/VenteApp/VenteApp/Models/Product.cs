namespace VenteApp
{
    public class Product
    {
        public Guid Id { get; set; } // Primary key
        public string Nom { get; set; }
        public string Description { get; set; }
        public string Categorie { get; set; }
        public string Taille { get; set; }
        public decimal Prix { get; set; } // Unit price
        public int Quantite { get; set; }

        public Guid? ProviderId { get; set; } // Foreign key to Product
        public Provider Provider { get; set; } // Navigation property to Product

        // Optionally, a collection of sales linked to this product
        public ICollection<SaleTransaction> Sales { get; set; }
    }
}
