
namespace VenteApp
{
    public partial class SalesPage : ContentPage
    {
        public SalesPage()
        {
            InitializeComponent();
            this.Title = "Ventes";
            BindingContext = new SalesViewModel(); // Set the ViewModel as the BindingContext
        }

        // Event handler for incrementing the quantity
        private void OnIncrementClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var sale = (Sale)((Grid)button.Parent.Parent).BindingContext;

            // Increment the quantity
            sale.Quantite += 1;

            // Add to cart automatically
            CartService.Instance.AddToCart(sale);
        }

        // Event handler for decrementing the quantity
        private void OnDecrementClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var sale = (Sale)((Grid)button.Parent.Parent).BindingContext;

            // Decrement the quantity, but ensure it doesn't go below 0
            if (sale.Quantite > 0)
            {
                sale.Quantite -= 1;

                // Add to cart automatically
                CartService.Instance.AddToCart(sale);
            }
            else
            {
                // Optionally remove from cart if the quantity reaches zero
                CartService.Instance.RemoveFromCart(sale);
            }
        }

        private async void OnShowBasketClicked(object sender, EventArgs e)
        {
            // Navigate to the BasketPage to show the cart items
            await Navigation.PushAsync(new BasketPage());
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is SalesViewModel viewModel)
            {
                viewModel.SearchCommand.Execute(e.NewTextValue);
            }
        }

    }
}
