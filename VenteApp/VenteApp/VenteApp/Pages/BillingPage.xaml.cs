using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VenteApp
{
    public partial class BillingPage : ContentPage
    {
        public ObservableCollection<BillingViewModel> BillingList { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public BillingPage()
        {
            InitializeComponent();
            LoadBills();
            BindingContext = this;
        }

        // Load bills by grouping SaleTransactions by OrderId
        private void LoadBills(string searchQuery = "")
        {
            using (var db = new AppDbContext())
            {
                // Step 1: Query the database and load data into memory
                var transactionsQuery = db.SaleTransactions
                                          .Include(st => st.Client)
                                          .Include(st => st.Product)
                                          .AsQueryable();

                // Step 2: Apply filtering in the query
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    transactionsQuery = transactionsQuery.Where(st => (st.Client.Nom.ToLower().Contains(searchQuery.ToLower()) || st.Client.Prenom.ToLower().Contains(searchQuery.ToLower())) ||
                                                                      st.Product.Nom.ToLower().Contains(searchQuery.ToLower()));
                }

                // Fetch the data from the database
                var transactions = transactionsQuery.ToList();

                // Step 3: Group and aggregate in-memory
                var groupedBills = transactions
                                   .GroupBy(st => st.OrderId)
                                   .Select(g => new BillingViewModel
                                   {
                                       OrderId = g.Key,
                                       ClientName = g.FirstOrDefault()?.Client != null ? $"{g.FirstOrDefault().Client.Nom} {g.FirstOrDefault().Client.Prenom}" : "Client inconnu",
                                       TotalAmount = g.Sum(st => st.Product.PrixVente * st.Quantite), // Sum in-memory (LINQ to objects)
                                       NumberOfProducts = g.Sum(st => st.Quantite)
                                   })
                                   .ToList();

                // Pagination logic
                int totalBills = groupedBills.Count();
                TotalPages = (int)Math.Ceiling(totalBills / 10.0);
                BillingList = new ObservableCollection<BillingViewModel>(
                    groupedBills.Skip((CurrentPage - 1) * 10).Take(10)
                );

                // Notify UI about updates
                OnPropertyChanged(nameof(BillingList));
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(TotalPages));
            }
        }


        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            LoadBills(e.NewTextValue);
        }

        private void OnViewBillClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var orderId = (Guid)button.CommandParameter;

            // Navigate to Bill Details Page
            Navigation.PushAsync(new BillDetailPage(orderId));
        }
    }

    public class BillingViewModel
    {
        public Guid OrderId { get; set; }
        public string ClientName { get; set; }
        public decimal TotalAmount { get; set; }
        public int NumberOfProducts { get; set; }
    }
}
