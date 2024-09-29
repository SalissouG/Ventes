using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class InventoryViewModel
    {
        public ObservableCollection<Inventory> InventoryItems { get; set; }

        // Commands for editing and deleting inventory items
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public InventoryViewModel()
        {
            // Initialize with sample data
            InventoryItems = new ObservableCollection<Inventory>
        {
            new Inventory { ProductName = "Laptop", StockLevel = 50, Location = "Warehouse A", Supplier = "Tech Supplier Co.", LastUpdated = DateTime.Now.AddDays(-5) },
            new Inventory { ProductName = "Smartphone", StockLevel = 30, Location = "Warehouse B", Supplier = "Mobile World", LastUpdated = DateTime.Now.AddDays(-10) },
        };

            // Commands
            EditCommand = new Command<Inventory>(OnEditInventoryItem);
            DeleteCommand = new Command<Inventory>(OnDeleteInventoryItem);
        }

        private void OnEditInventoryItem(Inventory inventory)
        {
            // Handle the edit action (e.g., navigate to an edit page)
            App.Current.MainPage.DisplayAlert("Edit", $"Edit Inventory Item: {inventory.ProductName}", "OK");
        }

        private void OnDeleteInventoryItem(Inventory inventory)
        {
            // Remove the inventory item from the collection
            InventoryItems.Remove(inventory);
            App.Current.MainPage.DisplayAlert("Deleted", $"Deleted Inventory Item: {inventory.ProductName}", "OK");
        }
    }
}
