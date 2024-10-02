using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class InventoryViewModel : BindableObject
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> AllProducts { get; set; } // Used for keeping all products for search functionality

        public ICommand SearchCommand { get; }

        public InventoryViewModel()
        {
            LoadProducts();

            // Initialize the search command
            SearchCommand = new Command<string>(OnSearchProducts);
        }

        // Load all products from the database
        private void LoadProducts()
        {
            using (var db = new AppDbContext())
            {
                var productList = db.Products.ToList();
                Products = new ObservableCollection<Product>(productList);
                AllProducts = new ObservableCollection<Product>(productList); // Keep a copy for searching
            }
        }

        // Handle product search
        private void OnSearchProducts(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // If the search query is empty, show all products
                Products.Clear();
                foreach (var product in AllProducts)
                {
                    Products.Add(product);
                }
            }
            else
            {
                using (var db = new AppDbContext())
                {
                    var filteredProducts = db.Products
                                             .Where(p => p.Nom.ToLower().Contains(query.ToLower()) ||
                                                     p.Description.ToLower().Contains(query.ToLower()))
                                             .ToList();

                    // Update the ObservableCollection
                    Products.Clear();
                    foreach (var product in filteredProducts)
                    {
                        Products.Add(product);
                    }
                }
            }
        }
    }
}
