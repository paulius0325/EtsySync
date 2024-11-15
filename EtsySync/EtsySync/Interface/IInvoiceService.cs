using EtsyGateway;

namespace EtsySync.Interface
{
    public interface IInvoiceService
    {
        Task GenerateAndSaveInvoiceAsync(int Shop_id, int Listing_id);
        Task GenerateAndSaveInvoiceForAllReceiptsAsync(int Shop_id);
        Task<byte[]> GenerateInvoicesZipForAllReceiptsAsync(int Shop_id);
        Task<(byte[]? FileData, string? FileName)> GetLastGeneratedInvoiceFileAsync();
        Task<(byte[]? FileData, string? FileName)> GetInvoiceBySerialNumberAsync(int serialNumber);
        Task GenerateAndSaveEmptyInvoiceAsync(string? fileName = null);
        Task<byte[]> CreateInvoicesZip(IEnumerable<(byte[] fileData, string fileName)> allInvoices);
        Task<IEnumerable<(byte[] fileData, string fileName)>> GetAllInvoicesFromDatabaseAsync();
    }
}
