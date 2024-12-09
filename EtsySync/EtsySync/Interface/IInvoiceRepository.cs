using SharedProject.Models;

namespace EtsySync.Interface
{
    public interface IInvoiceRepository
    {
        Task AddInvoiceAsync(SalesItem salesItem);
        Task<Guid> AddZipFileAsync(Guid zipFileId, string fileName, byte[] fileData, string? description = null);
        Task<bool> ExistsBySerialNumberAsync(long serialNumber);
        Task<byte[]> GetInvoiceFileDataBySerialNumberAsync(long serialNumber);
        Task<bool> DeleteInvoiceFileAndDataAsync(long serialNumber);
        Task<CompressedFile> GetZipFileByIdAsync(Guid id);
       // Task<bool> DeleteAllExcelFilesAndRelatedDataAsync();
        Task<List<SalesItem>> GetAllInvoicesAsync();

    }
}

