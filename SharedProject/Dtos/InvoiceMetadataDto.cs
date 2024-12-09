namespace SharedProject.Dtos
{
    public class InvoiceMetadataDto
    {
        public Guid SalesId { get; set; }
        public string? FileName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
