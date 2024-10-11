using Microsoft.EntityFrameworkCore;

namespace VenteApp
{
    public partial class BillDetailPage : ContentPage
    {
        public BillDetailViewModel Bill { get; set; }

        public BillDetailPage(Guid orderId)
        {
            InitializeComponent();
            LoadBillDetails(orderId);
            BindingContext = this;
        }

        private void LoadBillDetails(Guid orderId)
        {
            using (var db = new AppDbContext())
            {
                var transactions = db.SaleTransactions
                                     .Include(st => st.Client)
                                     .Include(st => st.Product)
                                     .Where(st => st.OrderId == orderId)
                                     .ToList();

                if (transactions.Any())
                {
                    var firstTransaction = transactions.FirstOrDefault();
                    Bill = new BillDetailViewModel
                    {
                        ClientName = firstTransaction.Client != null ? $"{firstTransaction.Client.Nom} {firstTransaction.Client.Prenom}" : "Client inconnu",
                        TotalAmount = transactions.Sum(t => t.Product.PrixVente * t.Quantite),
                        Products = transactions.Select(t => new ProductDetailViewModel
                        {
                            ProductName = t.Product.Nom,
                            Quantity = t.Quantite,
                            Price = t.Product.PrixVente
                        }).ToList()
                    };

                    OnPropertyChanged(nameof(Bill));
                }
            }
        }
    }

    public class BillDetailViewModel
    {
        public string ClientName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ProductDetailViewModel> Products { get; set; }
    }

    public class ProductDetailViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
