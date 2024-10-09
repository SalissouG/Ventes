namespace VenteApp
{
    public partial class ProviderProductsPage : ContentPage
    {
        public ProviderProductsPage(Provider provider)
        {
            InitializeComponent();
            this.Title = $"Produits du fournisseur: {provider.Nom}";
            BindingContext = new ProviderProductsViewModel(provider);
        }
    }
}
