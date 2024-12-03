using Newtonsoft.Json;
using SharedProject.Dtos;


namespace EtsyGateway
{
    public class ReceiptTransactionsService
    {
        //    private readonly ReceiptTransactionsClient _receiptTransactionsClient;

        //    public ReceiptTransactionsService(ReceiptTransactionsClient receiptTransactionsClient)
        //    {
        //        _receiptTransactionsClient = receiptTransactionsClient;
        //    }


        //    public async Task<string?> GetShopReceiptTransactionsByListingAsync(int Shop_id, int Listing_id)
        //    {
        //        return await _receiptTransactionsClient.GetShopReceiptTransactionsByListingAsync(Shop_id, Listing_id);
        //    }


        //    public async Task<string?> GetShopReceiptTransactionsByShopAsync(int Shop_id)
        //    {
        //        return await _receiptTransactionsClient.GetShopReceiptTransactionsByShopAsync(Shop_id);
        //    }


        //    public async Task<List<InvoiceItemDto>> GetInvoiceItemsFromReceiptsAsync(int Shop_id, int Listing_id)
        //    {
        //        var rawData = await GetShopReceiptTransactionsByListingAsync(Shop_id, Listing_id);

        //        if (!string.IsNullOrEmpty(rawData))
        //        {
        //            var receiptData = JsonConvert.DeserializeObject<List<EtsyReceiptTransaction>>(rawData);

        //            var invoiceItems = receiptData.Select(transaction => new InvoiceItemDto
        //            {
        //                ItemName = transaction.ItemName,
        //                Quantity = transaction.Quantity,
        //                Price = (double)transaction.Price
        //            }).ToList();

        //            return invoiceItems;
        //        }

        //        return new List<InvoiceItemDto>();
        //    }


        //    public async Task<List<InvoiceItemDto>> GetAllInvoiceItemsAsync(int Shop_id)
        //    {
        //        var rawData = await GetShopReceiptTransactionsByShopAsync(Shop_id);

        //        if (!string.IsNullOrEmpty(rawData))
        //        {
        //            var receiptData = JsonConvert.DeserializeObject<List<EtsyReceiptTransaction>>(rawData);

        //            var invoiceItems = receiptData.Select(transaction => new InvoiceItemDto
        //            {
        //                ItemName = transaction.ItemName,
        //                Quantity = transaction.Quantity,
        //                Price = (double)transaction.Price
        //            }).ToList();

        //            return invoiceItems;
        //        }

        //        return new List<InvoiceItemDto>();
        //    }
        //}

        //public class EtsyReceiptTransaction
        //{
        //    public string ItemName { get; set; }
        //    public int Quantity { get; set; }
        //    public decimal Price { get; set; }
        //}

        //public class InvoiceItemDto
        //{
        //    public string ItemName { get; set; }
        //    public int Quantity { get; set; }
        //    public double Price { get; set; }
        //}
    }
}
