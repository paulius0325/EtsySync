using SharedProject.Models;

namespace EtsySync.Interface
{
    public interface IInvoiceRepository
    {
        Task AddInvoiceAsync(SalesItem salesItem);
        Task AddZipFileAsync(string fileName, byte[] fileData, string? description = null);
        Task<bool> ExistsBySerialNumberAsync(long serialNumber);
        Task<byte[]> GetInvoiceFileDataBySerialNumberAsync(long serialNumber);
        Task<bool> DeleteInvoiceFileAndDataAsync(long serialNumber);
       // Task<bool> DeleteZipFileAsync(Guid zipFileId);
        Task<bool> DeleteAllExcelFilesAndRelatedDataAsync();

    }
}

