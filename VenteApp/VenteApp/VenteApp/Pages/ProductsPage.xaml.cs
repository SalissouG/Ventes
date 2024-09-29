namespace VenteApp;

public partial class ProductsPage : ContentPage
{
	public ProductsPage()
	{
		InitializeComponent();

		this.Title = "Produits";

        this.BindingContext = new ProductViewModel();
    }
}