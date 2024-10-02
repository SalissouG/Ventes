using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace VenteApp
{
    public class HistoricalViewModel : BindableObject
    {
        private DateTime _dateDebut;
        private DateTime _dateFin;
        private ObservableCollection<SaleTransaction> _filteredSales;

        public DateTime DateDebut
        {
            get => _dateDebut;
            set
            {
                _dateDebut = value;
                OnPropertyChanged();
                FilterSales(); // Automatically filter when DateDebut changes
            }
        }

        public DateTime DateFin
        {
            get => _dateFin;
            set
            {
                _dateFin = value;
                OnPropertyChanged();
                FilterSales(); // Automatically filter when DateFin changes
            }
        }

        public ObservableCollection<SaleTransaction> FilteredSales
        {
            get => _filteredSales;
            set
            {
                _filteredSales = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }

        public HistoricalViewModel()
        {
            DateDebut = DateTime.Now.AddMonths(-1);
            DateFin = DateTime.Now;

            // Initialize filtered sales
            FilteredSales = new ObservableCollection<SaleTransaction>();

            // Load sales from the database
            LoadSalesFromDatabase();
        }

        // Method to load sales data from the database
        private void LoadSalesFromDatabase()
        {
            using (var db = new AppDbContext())
            {
                var salesData = db.SaleTransactions
                                  .Include(sale => sale.Product) // Include the related Product
                                  .ToList();

                FilteredSales = new ObservableCollection<SaleTransaction>(salesData);
            }
        }

        // Filter sales based on date range
        private void FilterSales()
        {
            using (var db = new AppDbContext())
            {
                // Fetch sales data from the database within the specified date range
                var filteredSalesData = db.SaleTransactions
                                          .Include(sale => sale.Product) // Include the Product details
                                          .Where(sale => sale.DateDeVente >= DateDebut && sale.DateDeVente <= DateFin)
                                          .ToList();

                // Populate the FilteredSales collection with filtered data
                FilteredSales = new ObservableCollection<SaleTransaction>(filteredSalesData);
            }
        }

        // Search for sales based on the product name or description
        public void OnSearch(string searchTerm)
        {
            using (var db = new AppDbContext())
            {
                var filteredSales = db.SaleTransactions
                                      .Include(sale => sale.Product)
                                      .Where(sale => sale.Product.Nom.ToLower().Contains(searchTerm.ToLower()) ||
                                                     sale.Product.Description.ToLower().Contains(searchTerm.ToLower()) ||
                                                     sale.Product.Categorie.ToLower().Contains(searchTerm.ToLower()))
                                      .Where(sale => sale.DateDeVente >= DateDebut && sale.DateDeVente <= DateFin)
                                      .ToList();

                FilteredSales = new ObservableCollection<SaleTransaction>(filteredSales);
            }
        }
    }
}
