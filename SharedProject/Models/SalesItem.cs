using System.ComponentModel.DataAnnotations;

namespace SharedProject.Models
{
    public class SalesItem
    {
        [Key]
        public Guid SalesId { get; set; }
        public byte[]? FileData { get; set; }
        public string? FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long SerialNumber { get; set; }
        public Client? Client { get; set; }
        public InvoiceItem? InvoiceItem { get; set; }
    }
}
