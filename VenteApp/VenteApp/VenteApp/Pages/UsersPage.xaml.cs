namespace VenteApp;

public partial class UsersPage : ContentPage
{
	public UsersPage()
	{
		InitializeComponent();

		this.Title = "Utilisateurs";

        BindingContext = new UserViewModel();
    }

}