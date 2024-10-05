using System;
using System.Linq;
using System.Collections.Generic;
using Microcharts;
using SkiaSharp;

namespace VenteApp
{
    public class DashboardViewModel : BindableObject
    {
        private DateTime _dateDebut;
        private DateTime _dateFin;
        private Chart _mostSoldChart;
        private Chart _leastSoldChart;

        public DateTime DateDebut
        {
            get => _dateDebut;
            set
            {
                _dateDebut = value;
                OnPropertyChanged();
                LoadSalesData();
            }
        }

        public DateTime DateFin
        {
            get => _dateFin;
            set
            {
                _dateFin = value;
                OnPropertyChanged();
                LoadSalesData();
            }
        }

        public Chart MostSoldChart
        {
            get => _mostSoldChart;
            set
            {
                _mostSoldChart = value;
                OnPropertyChanged();
            }
        }

        public Chart LeastSoldChart
        {
            get => _leastSoldChart;
            set
            {
                _leastSoldChart = value;
                OnPropertyChanged();
            }
        }

        public DashboardViewModel()
        {
            // Définir la plage de dates par défaut (le mois dernier jusqu'à aujourd'hui)
            DateDebut = DateTime.Now.AddMonths(-1);
            DateFin = DateTime.Now;

            // Charger les données initiales
            LoadSalesData();
        }

        // Charger les données de vente et mettre à jour les graphiques
        private void LoadSalesData()
        {
            using (var db = new AppDbContext())
            {
                // Récupérer les ventes entre DateDebut et DateFin
                var sales = db.SaleTransactions
                    .Where(st => st.DateDeVente >= DateDebut && st.DateDeVente <= DateFin)
                    .GroupBy(st => st.ProductId)
                    .Select(group => new ProductSales
                    {
                        ProductId = group.Key,
                        TotalQuantitySold = group.Sum(st => st.Quantite),
                        ProductName = group.First().Product.Nom
                    })
                    .ToList();

                // Top 10 produits les plus vendus
                var mostSoldProducts = sales
                    .OrderByDescending(s => s.TotalQuantitySold)
                    .Take(10)
                    .ToList();

                // Top 10 produits les moins vendus
                var leastSoldProducts = sales
                    .OrderBy(s => s.TotalQuantitySold)
                    .Take(10)
                    .ToList();

                // Générer le graphique pour les produits les plus vendus
                MostSoldChart = new BarChart
                {
                    Entries = mostSoldProducts.Select(s => new ChartEntry(s.TotalQuantitySold)
                    {
                        Label = s.ProductName,
                        ValueLabel = s.TotalQuantitySold.ToString(),
                        Color = SKColor.Parse("#2c3e50")
                    }).ToList(),
                    LabelTextSize = 35,
                    ValueLabelOrientation = Orientation.Horizontal,  // Orienter les labels horizontalement
                    LabelOrientation = Orientation.Horizontal  // Textes à l'endroit
                };

                // Générer le graphique pour les produits les moins vendus
                LeastSoldChart = new BarChart
                {
                    Entries = leastSoldProducts.Select(s => new ChartEntry(s.TotalQuantitySold)
                    {
                        Label = s.ProductName,
                        ValueLabel = s.TotalQuantitySold.ToString(),
                        Color = SKColor.Parse("#e74c3c")
                    }).ToList(),
                    LabelTextSize = 35,
                    ValueLabelOrientation = Orientation.Horizontal,  // Orienter les labels horizontalement
                    LabelOrientation = Orientation.Horizontal  // Textes à l'endroit
                };
            }
        }
    }

    public class ProductSales
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
