using EtsyGateway;

namespace EtsySync.Interface
{
    public interface IInvoiceService
    {
        Task<byte[]> GenerateInvoicesZipForCsvAsync(IFormFile file);
    }
}

//Task<(byte[]? FileData, string? FileName)> GetLastGeneratedInvoiceFileAsync();
//Task GenerateAndSaveEmptyInvoiceAsync(string? fileName = null);
//Task<byte[]> CreateInvoicesZip(IEnumerable<(byte[] fileData, string fileName)> allInvoices, string password);
//Task<IEnumerable<(byte[] fileData, string fileName)>> GetAllInvoicesFromDatabaseAsync();
//Task SaveOrUpdateUploadedFileAsync(IFormFile file, int serialNumber);
//Task GenerateAndSaveInvoiceAsync(int Shop_id, int Listing_id);
//Task GenerateAndSaveInvoiceForAllReceiptsAsync(int Shop_id);
//Task<byte[]> GenerateInvoicesZipForAllReceiptsAsync(int Shop_id);
//Task<(byte[]? FileData, string? FileName)> GetInvoiceBySerialNumberAsync(int serialNumber);
//Task<int> GetNextSerialNumberAsync();