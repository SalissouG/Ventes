using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class SalesViewModel
    {
        public ObservableCollection<Sale> Sales { get; set; }

        // Commands for editing and deleting sales
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public SalesViewModel()
        {
            // Initialize with sample data
            Sales = new ObservableCollection<Sale>
        {
            new Sale { ProductName = "Laptop", QuantitySold = 2, SalePrice = 1500, SaleDate = DateTime.Now.AddDays(-2) },
            new Sale { ProductName = "Smartphone", QuantitySold = 5, SalePrice = 800, SaleDate = DateTime.Now.AddDays(-5) },
        };

            // Commands
            EditCommand = new Command<Sale>(OnEditSale);
            DeleteCommand = new Command<Sale>(OnDeleteSale);
        }

        private void OnEditSale(Sale sale)
        {
            // Handle the edit action (e.g., navigate to an edit page)
            App.Current.MainPage.DisplayAlert("Edit", $"Edit Sale for: {sale.ProductName}", "OK");
        }

        private void OnDeleteSale(Sale sale)
        {
            // Remove the sale from the collection
            Sales.Remove(sale);
            App.Current.MainPage.DisplayAlert("Deleted", $"Deleted Sale for: {sale.ProductName}", "OK");
        }
    }
}
