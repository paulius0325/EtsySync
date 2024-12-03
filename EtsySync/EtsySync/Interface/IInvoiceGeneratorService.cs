using SharedProject.Models;

namespace EtsySync.Interface
{
    public interface IInvoiceGeneratorService
    {
        Task<byte[]> GenerateInvoiceAsync(List<SalesItem> salesData, List<InvoiceItem> invoiceItems, DateTime date, long serialNumber, List<Client> clientsData);

    }
}
