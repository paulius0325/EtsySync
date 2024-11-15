using SharedProject.Models;

namespace EtsySync.Interface
{
    public interface IInvoiceGeneratorService
    {
        Task<byte[]> GenerateInvoiceAsync(List<SalesItem> salesData, List<InvoiceItem> invoiceItems, DateTime date, int serialNumber, List<Client> clientsData);

    }
}
