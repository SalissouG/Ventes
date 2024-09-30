using System.Collections.ObjectModel;

namespace VenteApp
{
    public class InventoryViewModel
    {
        public ObservableCollection<Product> Products { get; set; }


        public InventoryViewModel()
        {
            // Pre-fill with some product data
            Products = new ObservableCollection<Product>
            {
                new Product { Nom = "Laptop", Description = "Gaming Laptop", Quantite = 10, Categorie = "Electronics", Taille = "N/A" },
                new Product { Nom = "Shirt", Description = "Cotton T-Shirt",  Quantite = 50, Categorie = "Clothing", Taille = "M" },
                new Product { Nom = "Phone", Description = "Smartphone",  Quantite = 30, Categorie = "Electronics", Taille = "N/A" },
            };

        }

    }
}
