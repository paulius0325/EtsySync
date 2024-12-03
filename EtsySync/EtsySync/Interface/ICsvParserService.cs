using SharedProject.Models;

namespace EtsySync.Interface
{
    public interface ICsvParserService
    {
        List<SalesItem> ParseCsv(IFormFile file);
    }
}
