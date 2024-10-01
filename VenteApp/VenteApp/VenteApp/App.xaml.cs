namespace VenteApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();  // Create database if it doesn't exist
        }

        MainPage = new NavigationPage(new MenuPage());
    }
}
