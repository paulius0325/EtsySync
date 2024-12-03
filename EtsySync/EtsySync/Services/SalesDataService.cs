using CsvHelper;
using EtsySync.Data;
using EtsySync.Interface;
using Microsoft.EntityFrameworkCore;
using SharedProject.Models;
using System.Globalization;

namespace EtsySync.Services
{
    public class SalesDataService : ISalesDataService
    {
        private readonly InvoiceDbContext _dbContext;

        public SalesDataService(InvoiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SalesItem>> GetSalesDataAsync()
        {
            return await _dbContext.SalesItems.ToListAsync();
        }

        public async Task ImportSalesDataFromCsvAsync(Stream csvStream)
        {
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var salesItems = csv.GetRecords<SalesItem>().ToList();
                await _dbContext.SalesItems.AddRangeAsync(salesItems);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
