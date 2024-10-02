using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class SalesViewModel : BindableObject
    {
        public ObservableCollection<Sale> Sales { get; set; }
        public List<Product> AllProducts { get; set; } // To store all products for search
        public ICommand SearchCommand { get; }

        public SalesViewModel()
        {
            LoadSales();

            // Initialize the search command
            SearchCommand = new Command<string>(OnSearchSales);
        }

        // Load all products from the database and convert to sales
        private void LoadSales()
        {
            using (var db = new AppDbContext())
            {
                // Load products from the database
                AllProducts = db.Products.ToList();

                // Convert products to sales
                Sales = new ObservableCollection<Sale>(ConvertProductsToSales(AllProducts));
            }
        }

        // Search sales based on product name (case-insensitive)
        private void OnSearchSales(string query)
        {
            using (var db = new AppDbContext())
            {
                // Perform a case-insensitive search in the database
                var filteredProducts = db.Products
                                         .Where(p => p.Nom.ToLower().Contains(query.ToLower()) ||
                                                     p.Description.ToLower().Contains(query.ToLower()) ||
                                                     p.Description.ToLower().Contains(query.ToLower()))
                                         .ToList();

                // Convert filtered products to sales
                var filteredSales = ConvertProductsToSales(filteredProducts);

                // Update the Sales ObservableCollection
                Sales.Clear();
                foreach (var sale in filteredSales)
                {
                    Sales.Add(sale);
                }
            }
        }

        // Convert product list to sale list
        private List<Sale> ConvertProductsToSales(List<Product> products)
        {
            return products.Select(product => new Sale
            {
                ProductId = product.Id,
                Nom = product.Nom,
                Description = product.Description,
                Prix = product.Prix,
                Quantite = 0,  // Initialize sales quantity as 0
                Categorie = product.Categorie,
                Taille = product.Taille,
                DateDeVente = DateTime.Now
            }).ToList();
        }
    }
}
