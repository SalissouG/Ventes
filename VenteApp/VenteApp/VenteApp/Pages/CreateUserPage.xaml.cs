namespace VenteApp
{
    public partial class CreateUserPage : ContentPage
    {
        public CreateUserPage()
        {
            InitializeComponent();

            this.Title = "Créer un utilisateur";
        }

        private void OnSaveUserClicked(object sender, EventArgs e)
        {
            // Retrieve the user information from the fields
            string nom = NomEntry.Text;
            string prenom = PrenomEntry.Text;
            string phone = PhoneNumberEntry.Text;
            string adresse = AdresseEntry.Text;
            string email = EmailEntry.Text;

            // Add your logic to save the user (e.g., save to database)
            DisplayAlert("User Created", $"User {nom} {prenom} has been created.", "OK");
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            // Return to the previous page
            Navigation.PopAsync();
        }
    }
}
