namespace VenteApp
{
    public partial class CreateProductPage : ContentPage
    {
        private Product _productToEdit; // Product being edited

        public CreateProductPage(Product product = null)
        {
            InitializeComponent();

            this.Title = product == null ? "Créer un produit" : "Modifier le produit";

            if (product != null)
            {
                // If editing, set the product fields
                _productToEdit = product;
                LoadProductDetails(_productToEdit);
            }
        }

        // Load the details of the product being edited into the form fields
        private void LoadProductDetails(Product product)
        {
            NomEntry.Text = product.Nom;
            DescriptionEntry.Text = product.Description;
            PrixEntry.Text = product.Prix.ToString();
            QuantiteEntry.Text = product.Quantite.ToString();
            CategorieEntry.Text = product.Categorie;
            TailleEntry.Text = product.Taille;
            DateLimitePicker.Date = product.DateLimite;
        }

        private async void OnSaveProductClicked(object sender, EventArgs e)
        {
            try
            {
                if (_productToEdit == null)
                {
                    // Create a new product if _productToEdit is null (for new product creation)
                    Product newProduct = new Product
                    {
                        Nom = NomEntry.Text,
                        Description = DescriptionEntry.Text,
                        Prix = decimal.Parse(PrixEntry.Text),
                        Quantite = int.Parse(QuantiteEntry.Text),
                        Categorie = CategorieEntry.Text,
                        Taille = TailleEntry.Text,
                        DateLimite = DateLimitePicker.Date // Get the selected date
                    };

                    // Save the new product to the database using ProductService
                    await DisplayAlert("Produit Créé", $"Produit {newProduct.Nom} a été ajouté avec succès.", "OK");
                }
                else
                {
                    // Update the existing product's details
                    _productToEdit.Nom = NomEntry.Text;
                    _productToEdit.Description = DescriptionEntry.Text;
                    _productToEdit.Prix = decimal.Parse(PrixEntry.Text);
                    _productToEdit.Quantite = int.Parse(QuantiteEntry.Text);
                    _productToEdit.Categorie = CategorieEntry.Text;
                    _productToEdit.Taille = TailleEntry.Text;
                    _productToEdit.DateLimite = DateLimitePicker.Date;

                    // Update the product in the database using ProductService
                    await DisplayAlert("Produit Modifié", $"Produit {_productToEdit.Nom} a été modifié avec succès.", "OK");
                }

                // Navigate back to the product list
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., if parsing fails)
                await DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page
            await Navigation.PopAsync();
        }
    }
}
