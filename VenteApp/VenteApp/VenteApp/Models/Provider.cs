namespace VenteApp
{
    public class Provider
    {
        public Guid Id { get; set; } // Primary key

        public string Nom { get; set; } // Required: Name of the provider
        public string Prenom { get; set; } // Required: First name of the provider
        public string Numero { get; set; } // Required: Phone number
        public string Adresse { get; set; } // Optional: Address of the provider
        public string Email { get; set; } // Required: Email address

        // Optional: If you want to track related data (e.g., products supplied by this provider)
        public ICollection<Product> ProductsSupplied { get; set; }

    }
}
