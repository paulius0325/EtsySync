namespace SharedProject.Dtos
{
    public class SalesItemDto
    {
        public long OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ClientDto? ClientDto { get; set; }
    }
}
