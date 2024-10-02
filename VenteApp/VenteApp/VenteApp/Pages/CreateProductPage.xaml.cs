namespace VenteApp
{
    public partial class CreateProductPage : ContentPage
    {
        private Product _productToEdit;  // Product being edited

        // Constructor for creating a new product or editing an existing one
        public CreateProductPage(Product product = null)
        {
            InitializeComponent();

            this.Title = product == null ? "Créer un produit" : "Modifier le produit";

            if (product != null)
            {
                _productToEdit = product; // If editing, load product data
                LoadProductDetails(_productToEdit);
            }
        }

        // Load product details into the form for editing
        private void LoadProductDetails(Product product)
        {
            NomEntry.Text = product.Nom;
            DescriptionEntry.Text = product.Description;
            PrixEntry.Text = product.Prix.ToString();
            QuantiteEntry.Text = product.Quantite.ToString();
            CategorieEntry.Text = product.Categorie;
            TailleEntry.Text = product.Taille;
        }

        // Save the product (create or update)
        private async void OnSaveProductClicked(object sender, EventArgs e)
        {
            try
            {
                if (_productToEdit == null)
                {
                    // Creating a new product
                    var newProduct = new Product
                    {
                        Nom = NomEntry.Text,
                        Description = DescriptionEntry.Text,
                        Prix = decimal.Parse(PrixEntry.Text),
                        Quantite = int.Parse(QuantiteEntry.Text),
                        Categorie = CategorieEntry.Text,
                        Taille = TailleEntry.Text,
                    };

                    using (var db = new AppDbContext())
                    {
                        db.Products.Add(newProduct);
                        await db.SaveChangesAsync(); // Save new product to the database
                    }

                    await DisplayAlert("Produit Créé", $"Produit {newProduct.Nom} ajouté avec succès.", "OK");
                }
                else
                {
                    // Updating an existing product
                    using (var db = new AppDbContext())
                    {
                        var product = db.Products.Find(_productToEdit.Id);
                        product.Nom = NomEntry.Text;
                        product.Description = DescriptionEntry.Text;
                        product.Prix = decimal.Parse(PrixEntry.Text);
                        product.Quantite = int.Parse(QuantiteEntry.Text);
                        product.Categorie = CategorieEntry.Text;
                        product.Taille = TailleEntry.Text;

                        await db.SaveChangesAsync();  // Update the product in the database
                    }

                    await DisplayAlert("Produit Modifié", $"Produit {_productToEdit.Nom} modifié avec succès.", "OK");
                }

                // Navigate back to the product list page
                await Navigation.PushAsync(new ProductsPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }

        // Cancel and go back to the product list page
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Return to the previous page
        }
    }
}
