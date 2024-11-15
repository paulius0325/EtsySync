using ClosedXML.Excel;
using EtsySync.Data;
using EtsySync.Interface;
using Microsoft.EntityFrameworkCore;
using SharedProject.Models;

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
    }
}
