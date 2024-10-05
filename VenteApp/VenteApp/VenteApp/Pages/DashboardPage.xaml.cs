namespace VenteApp
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
            this.BindingContext = new DashboardViewModel();
        }
    }
}
