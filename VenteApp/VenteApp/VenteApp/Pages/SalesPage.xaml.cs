namespace VenteApp;

public partial class SalesPage : ContentPage
{
	public SalesPage()
	{
		InitializeComponent();

        this.Title = "Ventes";

        BindingContext = new SalesViewModel();
    }

    private async void OnVoirLePanierClicked(object sender, EventArgs e)
    {
        // Navigate to the BasketPage
        await Navigation.PushAsync(new BasketPage());
    }
}