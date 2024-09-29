using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace VenteApp
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; }

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ProductViewModel()
        {
            // Pre-fill with some product data
                Products = new ObservableCollection<Product>
            {
                new Product { Nom = "Laptop", Description = "Gaming Laptop", Prix = 1500, Quantite = 10, Categorie = "Electronics", Taille = "N/A", DateLimite = DateTime.Now.AddMonths(12) },
                new Product { Nom = "Shirt", Description = "Cotton T-Shirt", Prix = 20, Quantite = 50, Categorie = "Clothing", Taille = "M", DateLimite = DateTime.Now.AddMonths(6) },
                new Product { Nom = "Phone", Description = "Smartphone", Prix = 800, Quantite = 30, Categorie = "Electronics", Taille = "N/A", DateLimite = DateTime.Now.AddMonths(24) },
            };
            // Command for editing a product
            EditCommand = new Command<Product>(OnEditProduct);

            // Command for deleting a product
            DeleteCommand = new Command<Product>(OnDeleteProduct);
        }

        private void OnEditProduct(Product product)
        {
            // Edit logic here
            App.Current.MainPage.DisplayAlert("Edit", $"Edit Product: {product.Nom}", "OK");
        }

        private void OnDeleteProduct(Product product)
        {
            // Delete product from the collection
            Products.Remove(product);
            App.Current.MainPage.DisplayAlert("Deleted", $"Deleted Product: {product.Nom}", "OK");
        }
        // INotifyPropertyChanged implementation to notify UI of property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
