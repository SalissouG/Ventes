namespace VenteApp
{
    public partial class CreateProductPage : ContentPage
    {
        private Product _productToEdit;  // Product being edited

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

        // Real-time validation for Nom field
        private void OnNomTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNom();
        }

        // Real-time validation for Description field
        private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDescription();
        }

        // Real-time validation for Prix field
        private void OnPrixTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidatePrix();
        }

        // Real-time validation for Quantite field
        private void OnQuantiteTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateQuantite();
        }

        // Validate all fields before saving
        private bool ValidateInputs()
        {
            bool isValid = true;

            // Validate each field
            isValid = ValidateNom() && isValid;
            isValid = ValidateDescription() && isValid;
            isValid = ValidatePrix() && isValid;
            isValid = ValidateQuantite() && isValid;

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

        // Validate Description
        private bool ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(DescriptionEntry.Text))
            {
                DescriptionError.Text = "La description est obligatoire.";
                DescriptionError.IsVisible = true;
                return false;
            }
            else
            {
                DescriptionError.IsVisible = false;
                return true;
            }
        }

        // Validate Prix
        private bool ValidatePrix()
        {
            if (string.IsNullOrWhiteSpace(PrixEntry.Text))
            {
                PrixError.Text = "Le prix est obligatoire.";
                PrixError.IsVisible = true;
                return false;
            }
            else if (!decimal.TryParse(PrixEntry.Text, out decimal _))
            {
                PrixError.Text = "Le prix doit être un nombre valide.";
                PrixError.IsVisible = true;
                return false;
            }
            else
            {
                PrixError.IsVisible = false;
                return true;
            }
        }

        // Validate Quantite
        private bool ValidateQuantite()
        {
            if (string.IsNullOrWhiteSpace(QuantiteEntry.Text))
            {
                QuantiteError.Text = "La quantité est obligatoire.";
                QuantiteError.IsVisible = true;
                return false;
            }
            else if (!int.TryParse(QuantiteEntry.Text, out int _))
            {
                QuantiteError.Text = "La quantité doit être un nombre entier.";
                QuantiteError.IsVisible = true;
                return false;
            }
            else
            {
                QuantiteError.IsVisible = false;
                return true;
            }
        }

        // Save the product (create or update)
        private async void OnSaveProductClicked(object sender, EventArgs e)
        {
            bool isValid = ValidateInputs(); // Call the validation function

            if (!isValid)
                return; // Stop the method if the inputs are invalid

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
                        Categorie = CategorieEntry.Text,  // Optional field
                        Taille = TailleEntry.Text         // Optional field
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
                        product.Categorie = CategorieEntry.Text;  // Optional field
                        product.Taille = TailleEntry.Text;        // Optional field

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
