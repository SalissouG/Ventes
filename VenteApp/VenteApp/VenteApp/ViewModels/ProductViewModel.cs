using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class ProductViewModel : BindableObject
    {
        private const int PageSize = 1; // Number of products per page
        private int _currentPage = 1; // Current page number
        private int _totalPages; // Total number of pages

        public ObservableCollection<Product> Products { get; set; } // Only products for the current page

        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged();
            }
        }

        public ProductViewModel()
        {
            Products = new ObservableCollection<Product>();

            DeleteCommand = new Command<Product>(OnDeleteProduct);
            SearchCommand = new Command<string>(OnSearchProducts);
            NextPageCommand = new Command(OnNextPage);
            PreviousPageCommand = new Command(OnPreviousPage);

            LoadProducts(); // Load products for the first page
        }

        // Load total number of products and first page from the database
        private void LoadProducts()
        {
            using (var db = new AppDbContext())
            {
                // Get the total number of products
                int totalProducts = db.Products.Count();

                // Calculate total pages
                TotalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);

                // Load the first page
                LoadPage(CurrentPage);
            }
        }

        // Load products by page, directly from the database
        private void LoadPage(int pageNumber)
        {
            using (var db = new AppDbContext())
            {
                // Calculate the number of products to skip based on the current page
                int skip = (pageNumber - 1) * PageSize;

                // Fetch the products for the current page
                var pagedProducts = db.Products
                                      .OrderBy(p => p.Nom) // Optional: Order by name
                                      .Skip(skip)
                                      .Take(PageSize)
                                      .ToList();

                // Clear and load the products for the current page
                Products.Clear();
                foreach (var product in pagedProducts)
                {
                    Products.Add(product);
                }
            }
        }

        // Handle the next page navigation
        private void OnNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadPage(CurrentPage); // Load the next page
            }
        }

        // Handle the previous page navigation
        private void OnPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadPage(CurrentPage); // Load the previous page
            }
        }

        // Handle deleting a product from the database and UI
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

            // Reload current page after deletion
            LoadPage(CurrentPage);
        }

        // Handle searching products directly from the database
        private void OnSearchProducts(string query)
        {
            using (var db = new AppDbContext())
            {
                // Fetch products matching the search query, applying pagination
                var filteredProducts = db.Products
                                         .Where(p => p.Nom.ToLower().Contains(query.ToLower()) ||
                                                     p.Description.ToLower().Contains(query.ToLower()) ||
                                                     p.Categorie.ToLower().Contains(query.ToLower()))
                                         .OrderBy(p => p.Nom) // Optional: Order by name
                                         .Skip((CurrentPage - 1) * PageSize)
                                         .Take(PageSize)
                                         .ToList();

                // Update the ObservableCollection with the filtered products
                Products.Clear();
                foreach (var product in filteredProducts)
                {
                    Products.Add(product);
                }

                // Update the total pages based on the filtered result count
                int totalFilteredProducts = db.Products
                                              .Count(p => p.Nom.ToLower().Contains(query.ToLower()) ||
                                                          p.Description.ToLower().Contains(query.ToLower()) ||
                                                          p.Categorie.ToLower().Contains(query.ToLower()));
                TotalPages = (int)Math.Ceiling(totalFilteredProducts / (double)PageSize);
            }
        }
    }
}
