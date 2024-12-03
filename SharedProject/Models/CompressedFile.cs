namespace SharedProject.Models
{
    public class CompressedFile
    {
        public Guid ZipFileId { get; set; } // Primary key
        public string FileName { get; set; } = string.Empty; // Failo pavadinimas
        public byte[] FileData { get; set; } = Array.Empty<byte>(); // Dvejetainis failo turinys
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Sukurimo data
    }
}
