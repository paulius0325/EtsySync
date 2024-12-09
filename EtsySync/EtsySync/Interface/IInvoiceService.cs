using EtsyGateway;
using SharedProject.Dtos;

namespace EtsySync.Interface
{
    public interface IInvoiceService
    {
        Task<Guid> GenerateInvoicesZipForCsvAsync(IFormFile file);
        Task<List<InvoiceMetadataDto>> GetInvoicesMetadataAsync();
        Task<byte[]> GetInvoiceBySerialNumberAsync(long serialNumber);
        Task<bool> DeleteInvoiceBySerialNumberAsync(long serialNumber);
        Task<byte[]> GetInvoicesZipByIdAsync(Guid uploadId);
        //Task<bool> DeleteAllExcelFilesAndRelatedDataAsync();
    }
}


//Task GenerateAndSaveInvoiceAsync(int Shop_id, int Listing_id);
//Task GenerateAndSaveInvoiceForAllReceiptsAsync(int Shop_id);
//Task<byte[]> GenerateInvoicesZipForAllReceiptsAsync(int Shop_id);
