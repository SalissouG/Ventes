namespace VenteApp;

public partial class BasketPage : ContentPage
{
	public BasketPage()
	{
		InitializeComponent();
	}

    private async void OnRetourClicked(object sender, EventArgs e)
    {
        // Navigate back to the SalesPage
        await Navigation.PopAsync();
    }
}