using SharedProject.Models;

namespace EtsySync.Interface
{
    public interface ISalesDataService
    {
        Task<List<SalesItem>> GetSalesDataAsync();
    }
}
