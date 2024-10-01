using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VenteApp
{
    public class SalesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Sale> Sales { get; set; }
        public List<Product> Products { get; set; }

        public SalesViewModel()
        {
            // Initialize the product list
            Products = new List<Product>
            {
                new Product { Nom = "Laptop", Description = "Gaming Laptop", Prix = 1500, Quantite = 10, Categorie = "Electronics", Taille = "N/A", DateLimite = DateTime.Now.AddMonths(12) },
                new Product { Nom = "Shirt", Description = "Cotton T-Shirt", Prix = 20, Quantite = 50, Categorie = "Clothing", Taille = "M", DateLimite = DateTime.Now.AddMonths(6) },
                new Product { Nom = "Phone", Description = "Smartphone", Prix = 800, Quantite = 30, Categorie = "Electronics", Taille = "N/A", DateLimite = DateTime.Now.AddMonths(24) }
            };

            // Convert the product list to sales list
            Sales = new ObservableCollection<Sale>(ConvertProductsToSales(Products));
        }

        // Method to convert Product list to Sale list
        private List<Sale> ConvertProductsToSales(List<Product> products)
        {
            var sales = new List<Sale>();

            foreach (var product in products)
            {
                sales.Add(new Sale
                {
                    Nom = product.Nom,
                    Description = product.Description,
                    Prix = product.Prix,
                    Quantite = 0,
                    Categorie = product.Categorie,
                    Taille = product.Taille,
                    DateLimite = product.DateLimite
                });
            }

            return sales;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
