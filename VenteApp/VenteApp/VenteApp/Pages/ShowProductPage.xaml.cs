namespace VenteApp
{
    public partial class ShowProductPage : TabbedPage
    {
        public ShowProductPage(Product product)
        {
            InitializeComponent();
            BindingContext = product;

            // Check if the product has a provider
            if (product.Provider == null)
            {
                // You can either disable the provider tab or remove it
                this.Children.RemoveAt(1); // Removing the provider tab if there's no provider
            }
        }
    }

}
