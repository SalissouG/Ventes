namespace VenteApp
{
    public partial class SalesSummaryPage : ContentPage
    {
        public SalesSummaryPage()
        {
            InitializeComponent();
            this.Title = "Total des ventes";
            BindingContext = new SalesSummaryViewModel();
        }

        // Event handler for search field text change
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is SalesSummaryViewModel viewModel)
            {
                viewModel.SearchTerm = e.NewTextValue; // Update the search term in the ViewModel
            }
        }
    }
}
