namespace VenteApp
{
    public partial class CreateClientPage : ContentPage
    {
        private Client _clientToEdit;  // Client being edited

        public CreateClientPage(Client client = null)
        {
            InitializeComponent();

            this.Title = client == null ? "Créer un client" : "Modifier le client";

            if (client != null)
            {
                _clientToEdit = client; // If editing, load client data
                LoadClientDetails(_clientToEdit);
            }
        }

        // Load client details into the form for editing
        private void LoadClientDetails(Client client)
        {
            NomEntry.Text = client.Nom;
            PrenomEntry.Text = client.Prenom;
            NumeroEntry.Text = client.Numero;
            AdresseEntry.Text = client.Adresse;
            EmailEntry.Text = client.Email;
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

        // Save the client (create or update)
        private async void OnSaveClientClicked(object sender, EventArgs e)
        {
            bool isValid = ValidateInputs(); // Call the validation function

            if (!isValid)
                return; // Stop the method if inputs are invalid

            try
            {
                if (_clientToEdit == null)
                {
                    // Creating a new client
                    var newClient = new Client
                    {
                        Nom = NomEntry.Text,
                        Prenom = PrenomEntry.Text,
                        Numero = NumeroEntry.Text,
                        Adresse = AdresseEntry.Text,
                        Email = EmailEntry.Text
                    };

                    using (var db = new AppDbContext())
                    {
                        db.Clients.Add(newClient);
                        await db.SaveChangesAsync(); // Save new client to the database
                    }

                    await DisplayAlert("Client Créé", $"Client {newClient.Nom} ajouté avec succès.", "OK");
                }
                else
                {
                    // Updating an existing client
                    using (var db = new AppDbContext())
                    {
                        var client = db.Clients.Find(_clientToEdit.Id);
                        client.Nom = NomEntry.Text;
                        client.Prenom = PrenomEntry.Text;
                        client.Numero = NumeroEntry.Text;
                        client.Adresse = AdresseEntry.Text;
                        client.Email = EmailEntry.Text;

                        await db.SaveChangesAsync();  // Update the client in the database
                    }

                    await DisplayAlert("Client Modifié", $"Client {_clientToEdit.Nom} modifié avec succès.", "OK");
                }

                await Navigation.PushAsync(new ClientsPage()); // Navigate back to the client list page
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
