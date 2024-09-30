namespace VenteApp
{
    public partial class CreateProductPage : ContentPage
    {
        public CreateProductPage()
        {
            InitializeComponent();
            this.Title = "Créer un produit";
        }

        private async void OnSaveProductClicked(object sender, EventArgs e)
        {
            try
            {
                // Créer un nouvel objet Product et récupérer les valeurs des champs
                Product newProduct = new Product
                {
                    Nom = NomEntry.Text,
                    Description = DescriptionEntry.Text,
                    Prix = decimal.Parse(PrixEntry.Text),
                    Quantite = int.Parse(QuantiteEntry.Text),
                    Categorie = CategorieEntry.Text,
                    Taille = TailleEntry.Text,
                    DateLimite = DateLimitePicker.Date // Récupère la date sélectionnée

                };

                // Sauvegarder le produit (logique à définir, par exemple une sauvegarde en base de données)
                // Exemple : await Database.SaveProductAsync(newProduct);

                await DisplayAlert("Produit Créé", $"Produit {newProduct.Nom} a été ajouté avec succès.", "OK");
            }
            catch (Exception ex)
            {
                // Gérer les erreurs (par ex., si le parsing échoue)
                await DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Retourner à la page précédente
            await Navigation.PopAsync();
        }
    }
}
