namespace VenteApp;

public partial class HistoricalPage : ContentPage
{
    public HistoricalPage()
    {
        InitializeComponent();
        this.Title = "Total des ventes";
        BindingContext = new HistoricalViewModel();
    }

    // Event handler for dynamic search
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is HistoricalViewModel viewModel)
        {
            viewModel.OnSearch(e.NewTextValue); // Trigger the search when text changes
        }
    }
}
