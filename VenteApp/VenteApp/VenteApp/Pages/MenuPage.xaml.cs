using Newtonsoft.Json;

namespace VenteApp
{
    public partial class MenuPage : FlyoutPage
    {
        // Define the color you want to use for the selected menu item
        private readonly Color SelectedLabelColor = Color.FromHex("#16e7ed");  // Purple color

        public string ConnectedUserName { get; set; }

        public MenuPage()
        {
            InitializeComponent();

            // Check if the connected user is an admin and show/hide menu items accordingly
            ConfigureMenuVisibility();

            // Verify if the license is valid on startup
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(new SalesPage());
                ResetLabelStyles();  // Reset styles for all labels
                SetSelectedStyle(SalesLabel);  // Highlight the Sales label
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();  // Reset styles for all labels
                SetSelectedStyle(LicenseLabel);  // Highlight the License label
            }

            // Set the connected user's name
            ConnectedUserName = GetConnectedUserName();
        }

        public string GetConnectedUserName()
        {
            var userJson = Preferences.Get("ConnectedUser", string.Empty);
            if (!string.IsNullOrEmpty(userJson))
            {
                var user = JsonConvert.DeserializeObject<User>(userJson);
                return $"{user.Prenom} {user.Nom}";
            }
            return "No user connected";
        }

        // Navigate to Products Page
        private async void OnProductsClicked(object sender, EventArgs e)
        {
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(new ProductsPage());
                ResetLabelStyles();  // Reset styles for all labels
                SetSelectedStyle(ProductsLabel);  // Set selected label color
                SetSelectedStyleLa(ProductsLayout);
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();
                SetSelectedStyle(LicenseLabel);
            }
        }

        // Navigate to Inventory Page
        private async void OnInventoryClicked(object sender, EventArgs e)
        {
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(new InventoryPage());
                ResetLabelStyles();
                SetSelectedStyle(InventoryLabel);
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();
                SetSelectedStyle(LicenseLabel);
            }
        }

        // Navigate to Sales Page
        private async void OnSalesClicked(object sender, EventArgs e)
        {
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(new SalesPage());
                ResetLabelStyles();
                SetSelectedStyle(SalesLabel);
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();
                SetSelectedStyle(LicenseLabel);
            }
        }

        // Add similar methods for the other menu items
        private async void OnHistoricalClicked(object sender, EventArgs e)
        {
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(new HistoricalPage());
                ResetLabelStyles();
                SetSelectedStyle(HistoricalLabel);
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();
                SetSelectedStyle(LicenseLabel);
            }
        }

        private async void OnBasketClicked(object sender, EventArgs e)
        {
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(new BasketPage());
                ResetLabelStyles();
                SetSelectedStyle(BasketLabel);
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();
                SetSelectedStyle(LicenseLabel);
            }
        }

        private async void OnSalesSummaryClicked(object sender, EventArgs e)
        {
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(new SalesSummaryPage());
                ResetLabelStyles();
                SetSelectedStyle(SalesSummaryLabel);
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();
                SetSelectedStyle(LicenseLabel);
            }
        }

        private async void OnDashboardClicked(object sender, EventArgs e)
        {
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(new DashboardPage());
                ResetLabelStyles();
                SetSelectedStyle(DashboardLabel);
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();
                SetSelectedStyle(LicenseLabel);
            }
        }

        private async void OnLicenseClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new LicenseMenuPage());
            ResetLabelStyles();
            SetSelectedStyle(LicenseLabel);
        }

        private async void OnProvidersClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ProvidersPage());
            ResetLabelStyles();
            SetSelectedStyle(ProvidersLabel);
        }

        private async void OnClientsClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ClientsPage());
            ResetLabelStyles();
            SetSelectedStyle(ClientsLabel);
        }

        private async void OnUsersClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new UsersPage());
            ResetLabelStyles();
            SetSelectedStyle(UsersLabel);
        }

        private async void OnBillingClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new BillingPage());
            ResetLabelStyles();
            SetSelectedStyle(BillingLabel);
        }


        private async void OnDeconnexionClicked(object sender, EventArgs e)
        {
            UserService.Instance.ClearConnectedUser();

            // Update the ConnectedUserName after logout
            ConnectedUserName = UserService.Instance.GetConnectedUserName();
            OnPropertyChanged(nameof(ConnectedUserName));

            await Navigation.PushAsync(new MainPage());

            ResetLabelStyles();
        }

        // Configure menu visibility based on user role
        private void ConfigureMenuVisibility()
        {
            bool isAdmin = UserService.Instance.IsAdmin();

            // Show or hide the Users and License menu items based on admin status
            UsersLabel.IsVisible = isAdmin;
            LicenseLabel.IsVisible = isAdmin;
        }

        // Reset all labels to default style
        private void ResetLabelStyles()
        {
            // Reset background color and text color for all labels
            ProductsLabel.BackgroundColor = Colors.Transparent;
            InventoryLabel.BackgroundColor = Colors.Transparent;
            SalesLabel.BackgroundColor = Colors.Transparent;
            HistoricalLabel.BackgroundColor = Colors.Transparent;
            BasketLabel.BackgroundColor = Colors.Transparent;
            SalesSummaryLabel.BackgroundColor = Colors.Transparent;
            DashboardLabel.BackgroundColor = Colors.Transparent;
            LicenseLabel.BackgroundColor = Colors.Transparent;
            ProvidersLabel.BackgroundColor = Colors.Transparent;
            ClientsLabel.BackgroundColor = Colors.Transparent;
            UsersLabel.BackgroundColor = Colors.Transparent;
            BillingLabel.BackgroundColor = Colors.Transparent;
            DeconnexionLabel.BackgroundColor = Colors.Transparent;

            ProductsLayout.BackgroundColor = Colors.Transparent;

            // Reset text color to black (default)
            ProductsLabel.TextColor = Colors.Black;
            InventoryLabel.TextColor = Colors.Black;
            SalesLabel.TextColor = Colors.Black;
            HistoricalLabel.TextColor = Colors.Black;
            BasketLabel.TextColor = Colors.Black;
            SalesSummaryLabel.TextColor = Colors.Black;
            DashboardLabel.TextColor = Colors.Black;
            LicenseLabel.TextColor = Colors.Black;
            ProvidersLabel.TextColor = Colors.Black;
            ClientsLabel.TextColor = Colors.Black;
            UsersLabel.TextColor = Colors.Black;
            BillingLabel.TextColor = Colors.Black;
        }

        // Set the selected style for the current label
        private void SetSelectedStyle(Label label)
        {
            label.BackgroundColor = SelectedLabelColor;
            label.TextColor = Colors.White;
        }

        private void SetSelectedStyleLa(HorizontalStackLayout label)
        {
            label.BackgroundColor = SelectedLabelColor;
        }

    }
}
