namespace VenteApp
{
    public partial class ProvidersPage : ContentPage
    {
        public ProvidersPage()
        {
            InitializeComponent();
            this.Title = "Fournisseurs";

            try
            {
                this.BindingContext = new ProviderViewModel(ConfirmDeleteProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
            }
        }

        private async Task<bool> ConfirmDeleteProvider(Provider provider)
        {
            return await DisplayAlert("Confirmation", $"Voulez-vous vraiment supprimer {provider.Nom} ?", "Oui", "Non");
        }

        private async void OnAddProviderClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateProviderPage());
        }

        private async void OnEditProviderClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var provider = (Provider)((ViewCell)button.Parent.Parent).BindingContext;
            await Navigation.PushAsync(new CreateProviderPage(provider));
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is ProviderViewModel viewModel)
            {
                viewModel.SearchCommand.Execute(e.NewTextValue);
            }
        }
    }
}
