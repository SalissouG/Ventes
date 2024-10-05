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

        // Method to remove item from the cart with confirmation
        private async void OnRemoveItem(Sale sale)
        {
            // Ask for confirmation before removing
            bool confirm = await DisplayAlert("Confirmation", $"Voulez-vous vraiment retirer {sale.Nom} du panier ?", "Oui", "Non");
            if (!confirm)
                return;

            // Remove the item from the cart
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

            using (var db = new AppDbContext())
            {
                // Fetch the product to check stock
                var product = db.Products.FirstOrDefault(p => p.Id == sale.ProductId);
                if (product == null)
                {
                    DisplayAlert("Erreur", "Le produit n'existe pas.", "OK");
                    return;
                }

                // Check if stock is available
                if (sale.Quantite < product.Quantite)
                {
                    sale.Quantite += 1;
                    CartService.Instance.AddToCart(sale);

                    // Update TotalPrice
                    TotalPrice = CartService.Instance.GetTotalPrice();
                }
                else
                {
                    DisplayAlert("Stock insuffisant", $"Stock insuffisant pour le produit {product.Nom}.", "OK");
                }
            }
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

                // Update TotalPrice
                TotalPrice = CartService.Instance.GetTotalPrice();
            }
        }

        // Handle finalizing the sale and updating the product stock
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

                        // Check if there is enough stock
                        if (product.Quantite < sale.Quantite)
                        {
                            DisplayAlert("Erreur", $"Stock insuffisant pour le produit {product.Nom}.", "OK");
                            return;
                        }

                        // Create a sale transaction
                        var saleTransaction = new SaleTransaction
                        {
                            ProductId = sale.ProductId, // Foreign key to Product
                            Quantite = sale.Quantite,
                            DateDeVente = DateTime.Now
                        };
                        db.SaleTransactions.Add(saleTransaction);

                        // Update the product stock
                        product.Quantite -= sale.Quantite;
                    }

                    int changes = db.SaveChanges(); // Save changes to the database
                    if (changes > 0)
                    {
                        DisplayAlert("Succès", $"{changes} transaction(s) ajoutée(s) avec succès, stock mis à jour.", "OK");
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
