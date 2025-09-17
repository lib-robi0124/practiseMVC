namespace Lamazon.Domain.Entities
{
    public class InvoiceLineItem : BaseEntity
    {
        public int InvoiceId { get; set; }
        public int OrderLineItemId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual OrderLineItem OrderLineItem { get; set; }
    }
}
