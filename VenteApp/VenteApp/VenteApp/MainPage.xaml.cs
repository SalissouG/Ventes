namespace VenteApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Example login validation logic
            string phoneNumberOrEmail = PhoneNumberEntry.Text;
            string password = PasswordEntry.Text;

            if (!string.IsNullOrEmpty(phoneNumberOrEmail) && !string.IsNullOrEmpty(password))
            {
                // Perform login validation here

                // If login is successful, navigate to the MenuPage
                await Navigation.PushAsync(new MenuPage());
            }
            else
            {
                await DisplayAlert("Error", "Please enter valid credentials", "OK");
            }
        }


        private void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            // Navigate to forgot password page
            DisplayAlert("Forgot Password", "Redirecting to forgot password page", "OK");
        }

       
    }
}
