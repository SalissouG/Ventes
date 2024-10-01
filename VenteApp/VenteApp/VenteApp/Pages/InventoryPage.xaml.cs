namespace VenteApp
{
    public partial class InventoryPage : ContentPage
    {
        public InventoryPage()
        {
            InitializeComponent();
            this.Title = "Inventaires";

            BindingContext = new InventoryViewModel();
        }

        // Search as the user types
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is InventoryViewModel viewModel)
            {
                viewModel.SearchCommand.Execute(e.NewTextValue);
            }
        }
    }
}
