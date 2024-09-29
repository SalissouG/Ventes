namespace VenteApp;

public partial class InventoryPage : ContentPage
{
	public InventoryPage()
	{
		InitializeComponent();

		this.Title = "Inventaires";

        BindingContext = new InventoryViewModel();

    }
}