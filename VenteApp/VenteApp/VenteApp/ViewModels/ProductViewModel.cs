using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class ProductViewModel : BindableObject
    {
        public ObservableCollection<Product> Products { get; set; }

        public ICommand DeleteCommand { get; }

        public ProductViewModel()
        {
            // Initialize with some sample data
            Products = new ObservableCollection<Product>
            {
                new Product { Nom = "Laptop", Description = "Gaming Laptop", Prix = 1500, Quantite = 10, Categorie = "Electronics", Taille = "N/A", DateLimite = DateTime.Now.AddMonths(12) },
                new Product { Nom = "Shirt", Description = "Cotton T-Shirt", Prix = 20, Quantite = 50, Categorie = "Clothing", Taille = "M", DateLimite = DateTime.Now.AddMonths(6) },
                new Product { Nom = "Phone", Description = "Smartphone", Prix = 800, Quantite = 30, Categorie = "Electronics", Taille = "N/A", DateLimite = DateTime.Now.AddMonths(24) }
            };

            DeleteCommand = new Command<Product>(OnDeleteProduct);
        }


        // Handle deleting a product
        private void OnDeleteProduct(Product product)
        {
            Products.Remove(product);
        }
    }
}
