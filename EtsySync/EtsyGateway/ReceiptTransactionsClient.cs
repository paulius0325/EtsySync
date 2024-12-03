

namespace EtsyGateway
{
    public class ReceiptTransactionsClient
    {
        //private readonly HttpClient _httpClient;

        //public ReceiptTransactionsClient(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClient = httpClientFactory.CreateClient(nameof(ReceiptTransactionsClient));
        //}

        //public async Task<string?> GetShopReceiptTransactionsByListingAsync(int Shop_id, int Listing_id)
        //{
        //    var url = $"v3/application/shops/{Shop_id}/listings/{Listing_id}/transactions";
        //    var response = await _httpClient.GetAsync(url);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return await response.Content.ReadAsStringAsync();

        //    }
        //    return null;
        //}

        //public async Task<string?> GetShopReceiptTransactionsByShopAsync(int Shop_id)
        //{
        //    var url = $"v3/application/shops/{Shop_id}/transactions";
        //    var response = await _httpClient.GetAsync(url);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return await response.Content.ReadAsStringAsync();
        //    }

        //    return null;
        //}
    }
}
