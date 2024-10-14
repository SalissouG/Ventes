namespace VenteApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();  // Create database if it doesn't exist
            db.InitializeDatabase();
        }

        // Vérifier si la licence est valide au démarrage
        if (LicenseValidator.IsLicenceValid())
        {
            MainPage = new NavigationPage(new MenuPage());
        }
        else
        {
            MainPage = new NavigationPage(new LicensePage()); // Rediriger vers la page de licence
        }

        
    }
}
