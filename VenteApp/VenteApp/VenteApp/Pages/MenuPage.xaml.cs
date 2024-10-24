using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VenteApp
{
    public partial class MenuPage : FlyoutPage, INotifyPropertyChanged
    {
        // Define the color you want to use for the selected menu item
        private readonly Color SelectedLabelColor = Color.FromHex("#16e7ed");  // Example color

        private string connectedUserName;
        public string ConnectedUserName
        {
            get => connectedUserName;
            set
            {
                if (connectedUserName != value)
                {
                    connectedUserName = value;
                    OnPropertyChanged(nameof(ConnectedUserName));
                }
            }
        }

        // Add this if you haven't already implemented INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MenuPage()
        {
            InitializeComponent();

            BindingContext = this;  // Add this line

            // Check if the connected user is an admin and show/hide menu items accordingly
            ConfigureMenuVisibility();

            // Verify if the license is valid on startup
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(new SalesPage());
                ResetLabelStyles();  // Reset styles for all labels
                SetSelectedStyle(SalesLabel);  // Highlight the Sales label
                SetSelectedStyleLa(SalesLayout);
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();  // Reset styles for all labels
                SetSelectedStyle(LicenseLabel);  // Highlight the License label
                SetSelectedStyleLa(LicenseLayout);
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

        // Navigation methods for all menu items
        private async void OnProductsClicked(object sender, EventArgs e) => await NavigateToPage(new ProductsPage(), ProductsLabel, ProductsLayout);
        private async void OnProvidersClicked(object sender, EventArgs e) => await NavigateToPage(new ProvidersPage(), ProvidersLabel, ProvidersLayout);
        private async void OnSalesClicked(object sender, EventArgs e) => await NavigateToPage(new SalesPage(), SalesLabel, SalesLayout);
        private async void OnBasketClicked(object sender, EventArgs e) => await NavigateToPage(new BasketPage(), BasketLabel, BasketLayout);
        private async void OnBillingClicked(object sender, EventArgs e) => await NavigateToPage(new BillingPage(), BillingLabel, BillingLayout);
        private async void OnClientsClicked(object sender, EventArgs e) => await NavigateToPage(new ClientsPage(), ClientsLabel, ClientsLayout);
        private async void OnHistoricalClicked(object sender, EventArgs e) => await NavigateToPage(new HistoricalPage(), HistoricalLabel, HistoricalLayout);
        private async void OnInventoryClicked(object sender, EventArgs e) => await NavigateToPage(new InventoryPage(), InventoryLabel, InventoryLayout);
        private async void OnSalesSummaryClicked(object sender, EventArgs e) => await NavigateToPage(new SalesSummaryPage(), SalesSummaryLabel, SalesSummaryLayout);
        private async void OnDashboardClicked(object sender, EventArgs e) => await NavigateToPage(new DashboardPage(), DashboardLabel, DashboardLayout);
        private async void OnUsersClicked(object sender, EventArgs e) => await NavigateToPage(new UsersPage(), UsersLabel, UsersLayout);
        private async void OnLicenseClicked(object sender, EventArgs e) => await NavigateToPage(new LicenseMenuPage(), LicenseLabel, LicenseLayout);

        // Logout handling
        private async void OnDeconnexionClicked(object sender, EventArgs e)
        {
            UserService.Instance.ClearConnectedUser();
            ConnectedUserName = UserService.Instance.GetConnectedUserName();
            OnPropertyChanged(nameof(ConnectedUserName));
            await Navigation.PushAsync(new MainPage());
            ResetLabelStyles();
        }

        // Helper method for navigation and styling
        private async Task NavigateToPage(Page page, Label label, HorizontalStackLayout layout)
        {
            if (LicenseValidator.IsLicenceValid())
            {
                Detail = new NavigationPage(page);
                ResetLabelStyles();
                SetSelectedStyle(label);
                SetSelectedStyleLa(layout);
            }
            else
            {
                Detail = new NavigationPage(new LicenseMenuPage());
                ResetLabelStyles();
                SetSelectedStyle(LicenseLabel);
                SetSelectedStyleLa(LicenseLayout);
            }
        }

        // Configure menu visibility based on user role
        private void ConfigureMenuVisibility()
        {
            bool isAdmin = UserService.Instance.IsAdmin();
            UsersLayout.IsVisible = isAdmin;
            LicenseLayout.IsVisible = isAdmin;
        }

        // Reset all labels and layouts to default style
        private void ResetLabelStyles()
        {
            // Reset background color and text color for all labels and layouts
            ProductsLabel.BackgroundColor = Colors.Transparent;
            ProvidersLabel.BackgroundColor = Colors.Transparent;
            SalesLabel.BackgroundColor = Colors.Transparent;
            BasketLabel.BackgroundColor = Colors.Transparent;
            BillingLabel.BackgroundColor = Colors.Transparent;
            ClientsLabel.BackgroundColor = Colors.Transparent;
            HistoricalLabel.BackgroundColor = Colors.Transparent;
            InventoryLabel.BackgroundColor = Colors.Transparent;
            SalesSummaryLabel.BackgroundColor = Colors.Transparent;
            DashboardLabel.BackgroundColor = Colors.Transparent;
            UsersLabel.BackgroundColor = Colors.Transparent;
            LicenseLabel.BackgroundColor = Colors.Transparent;
            DeconnexionLabel.BackgroundColor = Colors.Transparent;

            ProductsLayout.BackgroundColor = Colors.Transparent;
            ProvidersLayout.BackgroundColor = Colors.Transparent;
            SalesLayout.BackgroundColor = Colors.Transparent;
            BasketLayout.BackgroundColor = Colors.Transparent;
            BillingLayout.BackgroundColor = Colors.Transparent;
            ClientsLayout.BackgroundColor = Colors.Transparent;
            HistoricalLayout.BackgroundColor = Colors.Transparent;
            InventoryLayout.BackgroundColor = Colors.Transparent;
            SalesSummaryLayout.BackgroundColor = Colors.Transparent;
            DashboardLayout.BackgroundColor = Colors.Transparent;
            UsersLayout.BackgroundColor = Colors.Transparent;
            LicenseLayout.BackgroundColor = Colors.Transparent;
            DeconnexionLayout.BackgroundColor = Colors.Transparent;

            // Reset text color to black (default)
            ProductsLabel.TextColor = Colors.Black;
            ProvidersLabel.TextColor = Colors.Black;
            SalesLabel.TextColor = Colors.Black;
            BasketLabel.TextColor = Colors.Black;
            BillingLabel.TextColor = Colors.Black;
            ClientsLabel.TextColor = Colors.Black;
            HistoricalLabel.TextColor = Colors.Black;
            InventoryLabel.TextColor = Colors.Black;
            SalesSummaryLabel.TextColor = Colors.Black;
            DashboardLabel.TextColor = Colors.Black;
            UsersLabel.TextColor = Colors.Black;
            LicenseLabel.TextColor = Colors.Black;
        }

        // Set the selected style for the current label
        private void SetSelectedStyle(Label label)
        {
            label.BackgroundColor = SelectedLabelColor;
            label.TextColor = Colors.White;
        }

        // Set the selected style for the layout
        private void SetSelectedStyleLa(HorizontalStackLayout layout)
        {
            layout.BackgroundColor = SelectedLabelColor;
        }
    }
}
