using System.Collections.ObjectModel;

namespace VenteApp
{
    public class ProviderProductsViewModel : BindableObject
    {
        public ObservableCollection<Product> Products { get; set; }

        public ProviderProductsViewModel(Provider provider)
        {
            Products = new ObservableCollection<Product>();
            LoadProviderProducts(provider);
        }

        // Load the products provided by this provider
        private void LoadProviderProducts(Provider provider)
        {
            using (var db = new AppDbContext())
            {
                var products = db.Products.Where(p => p.ProviderId == provider.Id).ToList();
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
        }
    }
}
