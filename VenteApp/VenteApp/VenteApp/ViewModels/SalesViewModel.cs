using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace VenteApp
{
    public class SalesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Sale> Sales { get; set; }

        public SalesViewModel()
        {
            // Initialize with some sample data
            Sales = new ObservableCollection<Sale>
            {
                new Sale { Nom = "Ordinateur", Description = "Ordinateur portable", Prix = 1200m, Quantite = 0, Categorie = "Électronique", Taille = "N/A", DateLimite = DateTime.Now.AddMonths(6) },
                new Sale { Nom = "T-shirt", Description = "T-shirt coton", Prix = 25m, Quantite = 0, Categorie = "Vêtements", Taille = "L", DateLimite = DateTime.Now.AddMonths(12) }
            };

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
