namespace VenteApp
{
    public partial class MenuPage : FlyoutPage
    {
        public MenuPage()
        {
            InitializeComponent();

            // Set UsersPage as the default Detail page
            Detail = new NavigationPage(new UsersPage());
            ResetButtonStyles();  // Reset styles for all buttons
            UsersButton.BackgroundColor = Colors.LightBlue;
            UsersButton.TextColor = Colors.White;
        }

        // Navigate to Users Page
        private async void OnUsersClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new UsersPage());
            ResetButtonStyles();  // Reset styles for all buttons
            UsersButton.BackgroundColor = Colors.LightBlue;
            UsersButton.TextColor = Colors.White;
        }

        private void ResetButtonStyles()
        {
            UsersButton.BackgroundColor = Colors.Transparent;
            ClientsButton.BackgroundColor = Colors.Transparent;
            ProductsButton.BackgroundColor = Colors.Transparent;
            InventoryButton.BackgroundColor = Colors.Transparent;
            SalesButton.BackgroundColor = Colors.Transparent;
            DashboardButton.BackgroundColor = Colors.Transparent;

            UsersButton.TextColor = Colors.Black;
            ClientsButton.TextColor = Colors.Black;
            ProductsButton.TextColor = Colors.Black;
            InventoryButton.TextColor = Colors.Black;
            SalesButton.TextColor = Colors.Black;
            DashboardButton.TextColor = Colors.Black;
        }

        // Navigate to Clients Page
        private async void OnClientsClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ClientsPage());
            ResetButtonStyles();  // Reset styles for all buttons
            ClientsButton.BackgroundColor = Colors.LightBlue;
            ClientsButton.TextColor = Colors.White;
        }

        // Navigate to Products Page
        private async void OnProductsClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ProductsPage());
            ResetButtonStyles();  // Reset styles for all buttons
            ProductsButton.BackgroundColor = Colors.LightBlue;
            ProductsButton.TextColor = Colors.White;
        }

        // Add more methods for Inventory, Sales, and Dashboard
        private async void OnInventoryClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new InventoryPage());
            ResetButtonStyles();  // Reset styles for all buttons
            InventoryButton.BackgroundColor = Colors.LightBlue;
            InventoryButton.TextColor = Colors.White;
        }

        private async void OnSalesClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SalesPage());
            ResetButtonStyles();  // Reset styles for all buttons
            SalesButton.BackgroundColor = Colors.LightBlue;
            SalesButton.TextColor = Colors.White;
        }

        private async void OnDashboardClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new DashboardPage());
            ResetButtonStyles();  // Reset styles for all buttons
            DashboardButton.BackgroundColor = Colors.LightBlue;
            DashboardButton.TextColor = Colors.White;
        }
    }
}
