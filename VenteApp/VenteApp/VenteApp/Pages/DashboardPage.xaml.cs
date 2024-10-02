namespace VenteApp;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        this.Title = "Total des ventes";
        BindingContext = new DashboardViewModel();
    }

    // Event handler for dynamic search
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is DashboardViewModel viewModel)
        {
            viewModel.OnSearch(e.NewTextValue); // Trigger the search when text changes
        }
    }
}
