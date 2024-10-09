namespace VenteApp;

public partial class ProductsPage : ContentPage
{
    public ProductsPage()
    {
        InitializeComponent();

        this.Title = "Produits";

        try
        {
            // Pass the confirmation function to the ViewModel
            this.BindingContext = new ProductViewModel(ConfirmDeleteProduct);
        }
        catch (Exception ex)
        {
            // Log the inner exception to see what exactly is causing the issue
            Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
        }
    }

    private async Task<bool> ConfirmDeleteProduct(Product product)
    {
        return await DisplayAlert("Confirmation", $"Voulez-vous vraiment supprimer {product.Nom} ?", "Oui", "Non");
    }

    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        // Rediriger vers la page de création de produit
        await Navigation.PushAsync(new CreateProductPage());
    }

    // Event handler for incrementing the quantity
    private async void OnEditProductClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var product = (Product)((ViewCell)button.Parent.Parent).BindingContext;
        // Rediriger vers la page de création de produit
        await Navigation.PushAsync(new CreateProductPage(product));
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ProductViewModel viewModel)
        {
            viewModel.SearchCommand.Execute(e.NewTextValue);
        }
    }

    private async void OnShowProductClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var product = (Product)((ViewCell)button.Parent.Parent).BindingContext;

        // Navigate to a new page or show modal to display product details
        await Navigation.PushAsync(new ShowProductPage(product));
    }

}
