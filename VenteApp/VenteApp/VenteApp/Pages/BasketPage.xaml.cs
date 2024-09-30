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
        private async void OnRetourClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page (SalesPage)
            await Navigation.PopAsync();
        }
    }
}
