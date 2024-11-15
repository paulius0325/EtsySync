using SharedProject.Models;

namespace EtsySync.Interface
{
    public interface IInvoiceRepository
    {
        Task AddInvoiceAsync(SalesItem invoice);
        Task<SalesItem?> GetLastGeneratedInvoiceAsync();
        Task<SalesItem?> GetInvoiceBySerialNumberAsync(int serialNumber);
        Task<IEnumerable<SalesItem>> GetAllInvoicesAsync();
    }
}
