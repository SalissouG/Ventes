
namespace VenteApp
{
    public class Dashboard
    {
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public List<SaleDashboard> SalesData { get; set; }  // Sales data between start and end dates
    }

}
