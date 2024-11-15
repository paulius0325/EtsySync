

namespace EtsyGateway
{
    public class InvoiceProcessor
    {
        private readonly ReceiptTransactionsService _receiptTransactionsService;

        public InvoiceProcessor(ReceiptTransactionsService receiptTransactionsService)
        {
            _receiptTransactionsService = receiptTransactionsService;
        }

        public async Task ProcessInvoicesAsync(int Shop_id, int Listing_id)
        {

            var invoiceItems = await _receiptTransactionsService.GetInvoiceItemsFromReceiptsAsync(Shop_id, Listing_id);


        }

        public async Task ProcessAllInvoicesForShopAsync(int Shop_id)
        {
            var allInvoices = await _receiptTransactionsService.GetShopReceiptTransactionsByShopAsync(Shop_id);

            
        }
    }
}
