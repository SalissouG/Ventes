namespace VenteApp;

public partial class DashboardPage : ContentPage
{
	public DashboardPage()
	{
		InitializeComponent();

		this.Title = "Total des ventes";

        BindingContext = new DashboardViewModel();
    }
}