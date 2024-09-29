using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class DashboardViewModel : BindableObject
    {
        private DateTime _dateDebut;
        private DateTime _dateFin;
        private ObservableCollection<SaleDashboard> _filteredSales;

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

        public ObservableCollection<SaleDashboard> FilteredSales
        {
            get => _filteredSales;
            set
            {
                _filteredSales = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }

        public DashboardViewModel()
        {
            // Sample sales data
            List<SaleDashboard> salesData = new List<SaleDashboard>
            {
                new SaleDashboard { Nom = "Laptop", Description = "Gaming Laptop", Categorie = "Electronics", Taille = "N/A", DateLimite = DateTime.Now.AddMonths(3), PrixDeVentes = 1500, QuantiteVendu = 5 },
                new SaleDashboard { Nom = "Shirt", Description = "Cotton T-shirt", Categorie = "Clothing", Taille = "M", DateLimite = DateTime.Now.AddMonths(6), PrixDeVentes = 25, QuantiteVendu = 20 },
            };

            // Initialize dashboard data
            Dashboard dashboard = new Dashboard
            {
                DateDebut = DateTime.Now.AddMonths(-1),
                DateFin = DateTime.Now,
                SalesData = salesData
            };

            DateDebut = dashboard.DateDebut;
            DateFin = dashboard.DateFin;

            // Initialize filtered sales
            FilteredSales = new ObservableCollection<SaleDashboard>(dashboard.SalesData);

            // Command to search
            SearchCommand = new Command<string>(OnSearch);
        }

        private void FilterSales()
        {
            // Filter based on date range
            var allSales = new List<SaleDashboard>
            {
                new SaleDashboard { Nom = "Laptop", Description = "Gaming Laptop", Categorie = "Electronics", Taille = "N/A", DateLimite = DateTime.Now.AddMonths(3), PrixDeVentes = 1500, QuantiteVendu = 5 },
                new SaleDashboard { Nom = "Shirt", Description = "Cotton T-shirt", Categorie = "Clothing", Taille = "M", DateLimite = DateTime.Now.AddMonths(6), PrixDeVentes = 25, QuantiteVendu = 20 },
            };

            //FilteredSales = new ObservableCollection<SaleDashboard>(allSales.Where(sale => sale.DateLimite >= DateDebut && sale.DateLimite <= DateFin));
            FilteredSales = new ObservableCollection<SaleDashboard>(allSales.Where(sale => true));
        }

        private void OnSearch(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                FilterSales();  // Reset filter if search is empty
                return;
            }

            FilteredSales = new ObservableCollection<SaleDashboard>(
                FilteredSales.Where(sale => sale.Nom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                            sale.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            );
        }
    }
}
