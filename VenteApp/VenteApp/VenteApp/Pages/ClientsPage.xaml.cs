namespace VenteApp;

public partial class ClientsPage : ContentPage
{
    public ClientsPage()
    {
        InitializeComponent();
        this.Title = "Clients";

        try
        {
            this.BindingContext = new ClientViewModel(ConfirmDeleteClient);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
        }
    }

    private async Task<bool> ConfirmDeleteClient(Client client)
    {
        return await DisplayAlert("Confirmation", $"Voulez-vous vraiment supprimer {client.Nom} ?", "Oui", "Non");
    }

    private async void OnAddClientClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateClientPage());
    }

    private async void OnEditClientClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var client = (Client)((ViewCell)button.Parent.Parent).BindingContext;
        await Navigation.PushAsync(new CreateClientPage(client));
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ClientViewModel viewModel)
        {
            viewModel.SearchCommand.Execute(e.NewTextValue);
        }
    }
}
