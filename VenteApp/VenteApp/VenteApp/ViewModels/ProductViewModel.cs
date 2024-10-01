using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class ProductViewModel : BindableObject
    {
        public ObservableCollection<Product> AllProducts { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }

        public ProductViewModel()
        {
            LoadProducts();

            DeleteCommand = new Command<Product>(OnDeleteProduct);
            SearchCommand = new Command<string>(OnSearchProducts);
        }

        // Load products from the database
        private void LoadProducts()
        {
            using (var db = new AppDbContext())
            {
                var productList = db.Products.ToList();
                Products = new ObservableCollection<Product>(productList);
                AllProducts = new ObservableCollection<Product>(productList); // Store all products for search functionality
            }
        }

        // Handle deleting a product from both the collection and the database
        private void OnDeleteProduct(Product product)
        {
            if (product == null)
                return;

            using (var db = new AppDbContext())
            {
                // Remove product from the database
                var productToDelete = db.Products.Find(product.Id);
                if (productToDelete != null)
                {
                    db.Products.Remove(productToDelete);
                    db.SaveChanges();
                }
            }

            // Remove the product from the ObservableCollection (UI)
            Products.Remove(product);
            AllProducts.Remove(product); // Ensure consistency for search
        }

        // Handle searching products from the database
        private void OnSearchProducts(string query)
        {
            using (var db = new AppDbContext())
            {
                // Fetch products matching the search query
                var filteredProducts = db.Products
                                         .Where(p => p.Nom.Contains(query))
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
