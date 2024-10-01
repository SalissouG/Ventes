namespace VenteApp
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        public int Quantite { get; set; }
        public string Categorie { get; set; }
        public string Taille { get; set; }
        public DateTime DateLimite { get; set; }  // Expiry Date
    }

}
