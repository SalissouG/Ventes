using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public partial class BasketPage : ContentPage
    {
        public ObservableCollection<Sale> CartItems { get; set; }
        public decimal TotalPrice => CartService.Instance.GetTotalPrice(); // Dynamically calculate total price

        public ICommand RemoveCommand { get; }

        public BasketPage()
        {
            InitializeComponent();

            // Set up the remove command
            RemoveCommand = new Command<Sale>(OnRemoveItem);

            // Bind cart items to the ObservableCollection in CartService
            CartItems = CartService.Instance.CartItems;

            BindingContext = this; // Bind the BasketPage to its ViewModel

            this.Title = "Panier";
        }

        // Method to remove item from the cart
        private void OnRemoveItem(Sale sale)
        {
            CartService.Instance.RemoveFromCart(sale);
            // Trigger UI updates
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(TotalPrice)); // Update the total price after removing an item
        }

        // Override OnAppearing to refresh the data when the page is navigated to
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Ensure the UI is refreshed with the latest data from CartService
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(TotalPrice));
        }

        // Event handler for the "Retour" button (Go back to SalesPage)
        private async void OnRetournerClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page (SalesPage)
            await Navigation.PopAsync();
        }

        private void OnViderClicked(object sender, EventArgs e)
        {
            CartService.Instance.CartItems.Clear();
            CartItems.Clear();
        }

        private void OnValiderClicked(object sender, EventArgs e)
        {
            CartService.Instance.CartItems.Clear();
            CartItems.Clear();
        }

        private void OnIncrementClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var sale = (Sale)((Grid)button.Parent.Parent).BindingContext;

            // Increment the quantity
            sale.Quantite += 1;
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
            }
        }
    }
}
