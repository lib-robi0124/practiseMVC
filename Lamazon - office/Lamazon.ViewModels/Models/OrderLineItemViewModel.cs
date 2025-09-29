namespace Lamazon.ViewModels.Models
{
    public class OrderLineItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductViewModel Product { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int DiscountedPercentage { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
