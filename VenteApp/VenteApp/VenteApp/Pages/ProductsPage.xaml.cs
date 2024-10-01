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
        // Rediriger vers la page de création de produit
        await Navigation.PushAsync(new CreateProductPage());
    }

    // Event handler for incrementing the quantity
    private async void OnEditProductClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var product = (Product)((ViewCell)button.Parent.Parent).BindingContext;
        // Rediriger vers la page de création de produit
        await Navigation.PushAsync(new CreateProductPage(product));
    }
    
}