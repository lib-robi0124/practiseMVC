namespace Lamazon.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
    }
}
