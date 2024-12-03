using SharedProject.Models;

namespace EtsySync.Interface
{
    public interface IInvoiceRepository
    {

        Task AddInvoiceAsync(SalesItem salesItem);
        Task AddZipFileAsync(string fileName, byte[] fileData, string? description = null);


    }
}

//Task<SalesItem?> GetLastGeneratedInvoiceAsync();
//Task<SalesItem?> GetInvoiceBySerialNumberAsync(int serialNumber);
//Task<IEnumerable<SalesItem>> GetAllInvoicesAsync();
//Task UpdateInvoiceAsync(SalesItem salesItem);
//Task<SalesItem> GetByFileNameAsync(string fileName);
//Task<SalesItem?> GetInvoiceBySalesIdAsync(Guid salesId);
