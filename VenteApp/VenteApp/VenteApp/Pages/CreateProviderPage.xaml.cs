namespace VenteApp
{
    public partial class CreateProviderPage : ContentPage
    {
        private Provider _providerToEdit;  // Provider being edited

        public CreateProviderPage(Provider provider = null)
        {
            InitializeComponent();

            this.Title = provider == null ? "Créer un fournisseur" : "Modifier le fournisseur";

            if (provider != null)
            {
                _providerToEdit = provider; // If editing, load provider data
                LoadProviderDetails(_providerToEdit);
            }
        }

        // Load provider details into the form for editing
        private void LoadProviderDetails(Provider provider)
        {
            NomEntry.Text = provider.Nom;
            PrenomEntry.Text = provider.Prenom;
            NumeroEntry.Text = provider.Numero;
            AdresseEntry.Text = provider.Adresse;
            EmailEntry.Text = provider.Email;
        }

        // Real-time validation for Nom field
        private void OnNomTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNom();
        }

        // Real-time validation for Prenom field
        private void OnPrenomTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidatePrenom();
        }

        // Real-time validation for Numero field
        private void OnNumeroTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNumero();
        }

        // Real-time validation for Email field
        private void OnEmailTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateEmail();
        }

        // Validate all fields before saving
        private bool ValidateInputs()
        {
            bool isValid = true;

            // Validate each field
            isValid = ValidateNom() && isValid;
            isValid = ValidatePrenom() && isValid;
            isValid = ValidateNumero() && isValid;
            isValid = ValidateEmail() && isValid;

            return isValid;
        }

        // Validate Nom
        private bool ValidateNom()
        {
            if (string.IsNullOrWhiteSpace(NomEntry.Text))
            {
                NomError.Text = "Le nom est obligatoire.";
                NomError.IsVisible = true;
                return false;
            }
            else
            {
                NomError.IsVisible = false;
                return true;
            }
        }

        // Validate Prenom
        private bool ValidatePrenom()
        {
            if (string.IsNullOrWhiteSpace(PrenomEntry.Text))
            {
                PrenomError.Text = "Le prénom est obligatoire.";
                PrenomError.IsVisible = true;
                return false;
            }
            else
            {
                PrenomError.IsVisible = false;
                return true;
            }
        }

        // Validate Numero de téléphone
        private bool ValidateNumero()
        {
            if (string.IsNullOrWhiteSpace(NumeroEntry.Text))
            {
                NumeroError.Text = "Le numéro de téléphone est obligatoire.";
                NumeroError.IsVisible = true;
                return false;
            }
            else if (!NumeroEntry.Text.All(char.IsDigit))
            {
                NumeroError.Text = "Le numéro de téléphone doit contenir uniquement des chiffres.";
                NumeroError.IsVisible = true;
                return false;
            }
            else if (NumeroEntry.Text.Length < 8)
            {
                NumeroError.Text = "Le numéro de téléphone doit comporter au moins 8 chiffres.";
                NumeroError.IsVisible = true;
                return false;
            }
            else
            {
                NumeroError.IsVisible = false;
                return true;
            }
        }

        // Validate Email
        private bool ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                EmailError.Text = "L'email est obligatoire.";
                EmailError.IsVisible = true;
                return false;
            }
            else if (!IsValidEmail(EmailEntry.Text))
            {
                EmailError.Text = "L'email n'est pas valide.";
                EmailError.IsVisible = true;
                return false;
            }
            else
            {
                EmailError.IsVisible = false;
                return true;
            }
        }

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Save the provider (create or update)
        private async void OnSaveProviderClicked(object sender, EventArgs e)
        {
            bool isValid = ValidateInputs(); // Call the validation function

            if (!isValid)
                return; // Stop the method if inputs are invalid

            try
            {
                if (_providerToEdit == null)
                {
                    // Creating a new provider
                    var newProvider = new Provider
                    {
                        Nom = NomEntry.Text,
                        Prenom = PrenomEntry.Text,
                        Numero = NumeroEntry.Text,
                        Adresse = AdresseEntry.Text,
                        Email = EmailEntry.Text
                    };

                    using (var db = new AppDbContext())
                    {
                        db.Providers.Add(newProvider);
                        await db.SaveChangesAsync(); // Save new provider to the database
                    }

                    await DisplayAlert("Fournisseur Créé", $"Fournisseur {newProvider.Nom} ajouté avec succès.", "OK");
                }
                else
                {
                    // Updating an existing provider
                    using (var db = new AppDbContext())
                    {
                        var provider = db.Providers.Find(_providerToEdit.Id);
                        provider.Nom = NomEntry.Text;
                        provider.Prenom = PrenomEntry.Text;
                        provider.Numero = NumeroEntry.Text;
                        provider.Adresse = AdresseEntry.Text;
                        provider.Email = EmailEntry.Text;

                        await db.SaveChangesAsync();  // Update the provider in the database
                    }

                    await DisplayAlert("Fournisseur Modifié", $"Fournisseur {_providerToEdit.Nom} modifié avec succès.", "OK");
                }

                await Navigation.PushAsync(new ProvidersPage()); // Navigate back to the provider list page
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Return to the previous page
        }
    }
}
