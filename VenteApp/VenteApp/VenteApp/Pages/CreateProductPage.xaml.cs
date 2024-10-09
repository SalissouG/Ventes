using System.Collections.ObjectModel;

namespace VenteApp
{
    public partial class CreateProductPage : ContentPage
    {
        private Product _productToEdit;  // Product being edited
        private ObservableCollection<Provider> _providers;

        public CreateProductPage(Product product = null)
        {
            InitializeComponent();

            this.Title = product == null ? "Créer un produit" : "Modifier le produit";

            // Load providers and set the picker
            LoadProviders();

            if (product != null)
            {
                _productToEdit = product; // If editing, load product data
                LoadProductDetails(_productToEdit);
            }
        }

        // Load available providers from the database
        private void LoadProviders()
        {
            using (var db = new AppDbContext())
            {
                var providersFromDb = db.Providers.ToList();
                _providers = new ObservableCollection<Provider>(providersFromDb);
                ProviderPicker.ItemsSource = _providers;
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

            // Set selected provider if one exists
            ProviderPicker.SelectedItem = _providers.FirstOrDefault(p => p.Id == product.ProviderId);
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
            bool isValid = ValidateInputs();

            if (!isValid)
                return;

            var selectedProvider = (Provider)ProviderPicker.SelectedItem;
            //if (selectedProvider == null)
            //{
            //    ProviderError.Text = "Le fournisseur est obligatoire.";
            //    ProviderError.IsVisible = true;
            //    return;
            //}
            //ProviderError.IsVisible = false;

            try
            {
                using (var db = new AppDbContext())
                {
                    if (_productToEdit == null)
                    {
                        var newProduct = new Product
                        {
                            Nom = NomEntry.Text,
                            Description = DescriptionEntry.Text,
                            Prix = decimal.Parse(PrixEntry.Text),
                            Quantite = int.Parse(QuantiteEntry.Text),
                            Categorie = CategorieEntry.Text,
                            Taille = TailleEntry.Text,
                            ProviderId = selectedProvider == null ? null : selectedProvider.Id // Associate the provider
                        };

                        db.Products.Add(newProduct);
                        await db.SaveChangesAsync();
                        await DisplayAlert("Produit Créé", $"Produit {newProduct.Nom} ajouté avec succès.", "OK");
                    }
                    else
                    {
                        var product = db.Products.Find(_productToEdit.Id);
                        product.Nom = NomEntry.Text;
                        product.Description = DescriptionEntry.Text;
                        product.Prix = decimal.Parse(PrixEntry.Text);
                        product.Quantite = int.Parse(QuantiteEntry.Text);
                        product.Categorie = CategorieEntry.Text;
                        product.Taille = TailleEntry.Text;
                        product.ProviderId = selectedProvider == null ? null : selectedProvider.Id; // Update the provider

                        await db.SaveChangesAsync();
                        await DisplayAlert("Produit Modifié", $"Produit {product.Nom} modifié avec succès.", "OK");
                    }
                }

                await Navigation.PushAsync(new ProductsPage()); // Navigate back
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
