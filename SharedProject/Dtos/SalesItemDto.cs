namespace SharedProject.Dtos
{
    public class SalesItemDto
    {
        public byte[]? FileData { get; set; }
        public string? FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SerialNumber { get; set; }
        public ClientDto? ClientDto { get; set; }
    }
}
