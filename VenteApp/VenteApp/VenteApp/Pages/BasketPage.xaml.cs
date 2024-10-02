using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace VenteApp
{
    public partial class BasketPage : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<Sale> CartItems { get; set; }

        private decimal totalPrice;
        public decimal TotalPrice
        {
            get => totalPrice;
            set
            {
                if (totalPrice != value)
                {
                    totalPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RemoveCommand { get; }

        public BasketPage()
        {
            InitializeComponent();

            // Set up the remove command
            RemoveCommand = new Command<Sale>(OnRemoveItem);

            // Bind cart items to the ObservableCollection in CartService
            CartItems = CartService.Instance.CartItems;

            // Set the initial total price
            TotalPrice = CartService.Instance.GetTotalPrice();

            BindingContext = this;

            this.Title = "Panier";
        }

        // Method to remove item from the cart
        private void OnRemoveItem(Sale sale)
        {
            CartService.Instance.RemoveFromCart(sale);
            // Update UI
            TotalPrice = CartService.Instance.GetTotalPrice();
        }

        // Override OnAppearing to refresh data
        protected override void OnAppearing()
        {
            base.OnAppearing();
            TotalPrice = CartService.Instance.GetTotalPrice();
        }

        // Event handler for incrementing the quantity
        private void OnIncrementClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var sale = (Sale)((Grid)button.Parent.Parent).BindingContext;

            sale.Quantite += 1;
            CartService.Instance.AddToCart(sale);

            // Update TotalPrice
            TotalPrice = CartService.Instance.GetTotalPrice();
        }

        // Event handler for decrementing the quantity
        private void OnDecrementClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var sale = (Sale)((Grid)button.Parent.Parent).BindingContext;

            if (sale.Quantite > 0)
            {
                sale.Quantite -= 1;
                CartService.Instance.AddToCart(sale);
            }

            // Update TotalPrice
            TotalPrice = CartService.Instance.GetTotalPrice();
        }

        // Event handler for the "Retour" button (Go back to SalesPage)
        private async void OnRetournerClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page (SalesPage)
            await Navigation.PopAsync();
        }

        private void OnViderClicked(object sender, EventArgs e)
        {
            CartService.Instance.CartItems.Clear();
            TotalPrice = 0;
            CartItems.Clear();
        }

        private void OnValiderClicked(object sender, EventArgs e)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    foreach (var sale in CartItems)
                    {
                        // Verify that the product exists in the database
                        var product = db.Products.FirstOrDefault(p => p.Id == sale.ProductId);
                        if (product == null)
                        {
                            DisplayAlert("Erreur", $"Produit avec ID {sale.ProductId} n'existe pas dans la base de données.", "OK");
                            return;
                        }

                        var saleTransaction = new SaleTransaction
                        {
                            ProductId = sale.ProductId, // Foreign key to Product
                            Quantite = sale.Quantite,
                            DateDeVente = DateTime.Now
                        };

                        db.SaleTransactions.Add(saleTransaction); // Add the sale transaction
                    }

                    int changes = db.SaveChanges(); // Save changes to the database
                    if (changes > 0)
                    {
                        DisplayAlert("Succès", $"{changes} transaction(s) ajoutée(s) avec succès.", "OK");
                    }
                    else
                    {
                        DisplayAlert("Erreur", "Aucune transaction ajoutée.", "OK");
                    }
                }

                // Clear the cart and update the UI
                CartService.Instance.CartItems.Clear();
                TotalPrice = 0;
                CartItems.Clear();

                // Navigate to the ProductsPage
                Navigation.PushAsync(new ProductsPage());
            }
            catch (Exception ex)
            {
                DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
