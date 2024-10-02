namespace VenteApp
{
    public partial class MenuPage : FlyoutPage
    {
        // Define the color you want to use for the buttons
        private readonly Color SelectedButtonColor = Color.FromHex("#512bd4");  // Purple color from image

        public MenuPage()
        {
            InitializeComponent();

            // Set UsersPage as the default Detail page
            Detail = new NavigationPage(new SalesPage());
            ResetButtonStyles();  // Reset styles for all buttons
            SalesButton.BackgroundColor = SelectedButtonColor;  // Highlight Users button
            SalesButton.TextColor = Colors.White;
        }


        // Navigate to Products Page
        private async void OnProductsClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ProductsPage());
            ResetButtonStyles();  // Reset styles for all buttons
            ProductsButton.BackgroundColor = SelectedButtonColor;  // Set selected button color
            ProductsButton.TextColor = Colors.White;
        }

        // Add more methods for Inventory, Sales, and Dashboard
        private async void OnInventoryClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new InventoryPage());
            ResetButtonStyles();  // Reset styles for all buttons
            InventoryButton.BackgroundColor = SelectedButtonColor;  // Set selected button color
            InventoryButton.TextColor = Colors.White;
        }

        private async void OnSalesClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SalesPage());
            ResetButtonStyles();  // Reset styles for all buttons
            SalesButton.BackgroundColor = SelectedButtonColor;  // Set selected button color
            SalesButton.TextColor = Colors.White;
        }

        private async void OnHistoricalClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HistoricalPage());
            ResetButtonStyles();  // Reset styles for all buttons
            HistoricalButton.BackgroundColor = SelectedButtonColor;  // Set selected button color
            HistoricalButton.TextColor = Colors.White;
        }

        private async void OnBasketClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new BasketPage());
            ResetButtonStyles();  // Reset styles for all buttons
            BasketButton.BackgroundColor = SelectedButtonColor;  // Set selected button color
            BasketButton.TextColor = Colors.White;
        }

        // Reset all buttons to default style
        private void ResetButtonStyles()
        {
            // Reset background color and text color for all buttons
            ProductsButton.BackgroundColor = Colors.Transparent;
            InventoryButton.BackgroundColor = Colors.Transparent;
            SalesButton.BackgroundColor = Colors.Transparent;
            HistoricalButton.BackgroundColor = Colors.Transparent;
            BasketButton.BackgroundColor = Colors.Transparent;

            // Reset text color to black (default)
            ProductsButton.TextColor = Colors.Black;
            InventoryButton.TextColor = Colors.Black;
            SalesButton.TextColor = Colors.Black;
            HistoricalButton.TextColor = Colors.Black;
            BasketButton.TextColor = Colors.Black;
        }
    }
}
