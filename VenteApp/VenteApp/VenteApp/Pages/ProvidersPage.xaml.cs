using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace VenteApp;

public partial class ProvidersPage : ContentPage
{
    public ProvidersPage()
    {
        InitializeComponent();
        this.Title = "Fournisseurs";

        try
        {
            this.BindingContext = new ProviderViewModel(ConfirmDeleteProvider);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
        }
    }

    private async Task<bool> ConfirmDeleteProvider(Provider provider)
    {
        return await DisplayAlert("Confirmation", $"Voulez-vous vraiment supprimer {provider.Nom} ?", "Oui", "Non");
    }

    private async void OnAddProviderClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateProviderPage());
    }

    private async void OnEditProviderClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var provider = (Provider)((ViewCell)button.Parent.Parent).BindingContext;
        await Navigation.PushAsync(new CreateProviderPage(provider));
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ProviderViewModel viewModel)
        {
            viewModel.SearchCommand.Execute(e.NewTextValue);
        }
    }

    // New Method to Show Provider's Products
    private async void OnShowProviderProductsClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var provider = (Provider)((ViewCell)button.Parent.Parent).BindingContext;

        // Navigate to the page showing the provider's products
        await Navigation.PushAsync(new ProviderProductsPage(provider));
    }

    private async void OnDownloadProvidersPdfClicked(object sender, EventArgs e)
    {
        try
        {
            var viewModel = (ProviderViewModel)BindingContext;
            var providers = viewModel.GetAllProvidersForPdf();

            using (PdfDocument document = new PdfDocument())
            {
                document.Info.Title = "Liste des Fournisseurs";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Verdana", 20, XFontStyle.Bold);
                XFont headerFont = new XFont("Verdana", 12, XFontStyle.Bold);
                XFont contentFont = new XFont("Verdana", 10, XFontStyle.Regular);

                // Draw title
                gfx.DrawString("Liste des Fournisseurs", titleFont, XBrushes.Black, new XRect(0, 40, page.Width, 0), XStringFormats.TopCenter);

                // Set initial Y position for the content
                int yOffset = 80;

                // Define column widths and positions
                int[] columnWidths = { 100, 100, 100, 150, 150 };
                int[] columnPositions = { 20, 120, 220, 320, 470 };
                string[] headers = { "Nom", "Prénom", "Numéro", "Adresse", "Email" };

                // Draw table header
                for (int i = 0; i < headers.Length; i++)
                {
                    gfx.DrawString(headers[i], headerFont, XBrushes.Black,
                        new XRect(columnPositions[i], yOffset, columnWidths[i], 20),
                        XStringFormats.TopLeft);
                }
                yOffset += 20;

                // Draw a line below the header
                gfx.DrawLine(XPens.Black, 20, yOffset, page.Width - 20, yOffset);
                yOffset += 10;

                // Add each provider to the PDF
                foreach (var provider in providers)
                {
                    // Check if there's space for more content, otherwise add a new page
                    if (yOffset + 20 > page.Height - 40)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yOffset = 40;
                    }

                    // Draw provider details
                    gfx.DrawString(provider.Nom ?? "", contentFont, XBrushes.Black,
                        new XRect(columnPositions[0], yOffset, columnWidths[0], 20), XStringFormats.TopLeft);
                    gfx.DrawString(provider.Prenom ?? "", contentFont, XBrushes.Black,
                        new XRect(columnPositions[1], yOffset, columnWidths[1], 20), XStringFormats.TopLeft);
                    gfx.DrawString(provider.Numero ?? "", contentFont, XBrushes.Black,
                        new XRect(columnPositions[2], yOffset, columnWidths[2], 20), XStringFormats.TopLeft);
                    gfx.DrawString(provider.Adresse ?? "", contentFont, XBrushes.Black,
                        new XRect(columnPositions[3], yOffset, columnWidths[3], 20), XStringFormats.TopLeft);
                    gfx.DrawString(provider.Email ?? "", contentFont, XBrushes.Black,
                        new XRect(columnPositions[4], yOffset, columnWidths[4], 20), XStringFormats.TopLeft);

                    yOffset += 20;
                }

                // Save the PDF file to the download folder
                string downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Download");
                Directory.CreateDirectory(downloadFolder);
                string currentDate = DateTime.Now.ToString("yyyyMMdd_HHmm");
                string fileName = Path.Combine(downloadFolder, $"Liste_des_Fournisseurs_{currentDate}.pdf");

                document.Save(fileName);
            }

            await DisplayAlert("Téléchargement", "Le fichier PDF de la liste des fournisseurs a été enregistré dans le dossier Téléchargements.", "OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating PDF: {ex.Message}");
            await DisplayAlert("Erreur", $"Une erreur s'est produite lors de la création du fichier PDF: {ex.Message}", "OK");
        }
    }
}
