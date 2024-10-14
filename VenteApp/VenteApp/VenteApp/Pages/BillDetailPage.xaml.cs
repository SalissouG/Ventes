using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

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
                        SaleDate = firstTransaction.DateDeVente,
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

        private async void OnDownloadPdfClicked(object sender, EventArgs e)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Facture";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Define fonts
            XFont titleFont = new XFont("Verdana", 20, XFontStyle.Bold);
            XFont labelFont = new XFont("Verdana", 12, XFontStyle.Regular);

            // Draw title
            gfx.DrawString("Facture", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);

            // Draw client information
            gfx.DrawString($"Client: {Bill.ClientName}", labelFont, XBrushes.Black, new XRect(40, 100, page.Width - 80, 0), XStringFormats.Default);
            //gfx.DrawString($"Date: {Bill.S:dd/MM/yyyy}", labelFont, XBrushes.Black, new XRect(40, 120, page.Width - 80, 0), XStringFormats.Default);

            // Draw table headers
            gfx.DrawString("Produit", labelFont, XBrushes.Black, new XRect(40, 160, 0, 0), XStringFormats.Default);
            gfx.DrawString("Quantité", labelFont, XBrushes.Black, new XRect(240, 160, 0, 0), XStringFormats.Default);
            gfx.DrawString("Prix", labelFont, XBrushes.Black, new XRect(340, 160, 0, 0), XStringFormats.Default);

            // Draw products
            int yOffset = 180;
            foreach (var product in Bill.Products)
            {
                gfx.DrawString(product.ProductName, labelFont, XBrushes.Black, new XRect(40, yOffset, 0, 0), XStringFormats.Default);
                gfx.DrawString(product.Quantity.ToString(), labelFont, XBrushes.Black, new XRect(240, yOffset, 0, 0), XStringFormats.Default);
                gfx.DrawString(product.Price.ToString("C"), labelFont, XBrushes.Black, new XRect(340, yOffset, 0, 0), XStringFormats.Default);
                yOffset += 20;
            }

            // Draw total amount
            gfx.DrawString($"Total: {Bill.TotalAmount:C}", labelFont, XBrushes.Black, new XRect(40, yOffset + 20, page.Width - 80, 0), XStringFormats.Default);

            try
            {
                // Define the path for the download folder
                string downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Download");

                // Check if the folder exists, if not, create it
                if (!Directory.Exists(downloadFolder))
                {
                    Directory.CreateDirectory(downloadFolder);
                }

                // Get the current date formatted as yyyyMMdd
                string currentDate = DateTime.Now.ToString("yyyyMMdd_HHmm");

                // Define the file name with the current date included
                string fileName = Path.Combine(downloadFolder, $"Facture_{Bill.ClientName}_{currentDate}.pdf");

                // Save the PDF file
                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    document.Save(stream);
                }

                // Notify the user about the successful save
                await DisplayAlert("Téléchargement", "Le fichier PDF a été enregistré dans le dossier Téléchargements.", "OK");
            }
            catch (Exception ex)
            {
                // Handle the exception (log it or display an error message)
                await DisplayAlert("Erreur", "Une erreur s'est produite lors de la création du fichier PDF.", "OK");
            }



            // Notify the user
            //await DisplayAlert("Téléchargement", "Le fichier PDF a été enregistré dans le dossier Téléchargements.", "OK");
        }
    }

    public class BillDetailViewModel
    {
        public string ClientName { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime SaleDate { get; set; }

        public List<ProductDetailViewModel> Products { get; set; }
    }

    public class ProductDetailViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
