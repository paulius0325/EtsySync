using System.ComponentModel.DataAnnotations;

namespace SharedProject.Models
{
    public class InvoiceItem
    {
        [Key]
        public Guid InvoiceId { get; set; }
        public int ItemNumber { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
