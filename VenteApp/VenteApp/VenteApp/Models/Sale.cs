
namespace VenteApp
{
    public class Sale
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal SalePrice { get; set; }
        public decimal TotalPrice => QuantitySold * SalePrice;  // Calculated property
        public DateTime SaleDate { get; set; }
    }

}
