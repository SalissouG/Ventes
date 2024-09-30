namespace VenteApp
{
    public partial class CreateProductPage : ContentPage
    {
        public CreateProductPage()
        {
            InitializeComponent();
            this.Title = "Cr�er un produit";
        }

        private async void OnSaveProductClicked(object sender, EventArgs e)
        {
            try
            {
                // Cr�er un nouvel objet Product et r�cup�rer les valeurs des champs
                Product newProduct = new Product
                {
                    Nom = NomEntry.Text,
                    Description = DescriptionEntry.Text,
                    Prix = decimal.Parse(PrixEntry.Text),
                    Quantite = int.Parse(QuantiteEntry.Text),
                    Categorie = CategorieEntry.Text,
                    Taille = TailleEntry.Text,
                    DateLimite = DateLimitePicker.Date // R�cup�re la date s�lectionn�e

                };

                // Sauvegarder le produit (logique � d�finir, par exemple une sauvegarde en base de donn�es)
                // Exemple : await Database.SaveProductAsync(newProduct);

                await DisplayAlert("Produit Cr��", $"Produit {newProduct.Nom} a �t� ajout� avec succ�s.", "OK");
            }
            catch (Exception ex)
            {
                // G�rer les erreurs (par ex., si le parsing �choue)
                await DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Retourner � la page pr�c�dente
            await Navigation.PopAsync();
        }
    }
}
