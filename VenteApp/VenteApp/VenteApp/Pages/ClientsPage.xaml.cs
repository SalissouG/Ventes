namespace VenteApp;

public partial class ClientsPage : ContentPage
{
	public ClientsPage()
	{
		InitializeComponent();

		this.Title = "Clients";

        BindingContext = new ClientViewModel();

    }
}