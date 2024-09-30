namespace VenteApp;

public partial class ProductsPage : ContentPage
{
	public ProductsPage()
	{
		InitializeComponent();

		this.Title = "Produits";

        this.BindingContext = new ProductViewModel();
    }

    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        // Rediriger vers la page de cr�ation de produit
        await Navigation.PushAsync(new CreateProductPage());
    }
}