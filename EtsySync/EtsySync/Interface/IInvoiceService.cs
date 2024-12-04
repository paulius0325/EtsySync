using EtsyGateway;

namespace EtsySync.Interface
{
    public interface IInvoiceService
    {
        Task<byte[]> GenerateInvoicesZipForCsvAsync(IFormFile file);
        Task<byte[]> GetInvoiceBySerialNumberAsync(long serialNumber);
        Task<bool> DeleteInvoiceBySerialNumberAsync(long serialNumber);
        //Task<bool> DeleteZipFileAndRelatedDataAsync(Guid zipFileId);
        Task<bool> DeleteAllExcelFilesAndRelatedDataAsync();
    }
}


//Task GenerateAndSaveInvoiceAsync(int Shop_id, int Listing_id);
//Task GenerateAndSaveInvoiceForAllReceiptsAsync(int Shop_id);
//Task<byte[]> GenerateInvoicesZipForAllReceiptsAsync(int Shop_id);
