using EtsySync.Data;
using EtsySync.Interface;
using Microsoft.EntityFrameworkCore;


namespace EtsySync.Services
{
    public class SerialNumberService : ISerialNumberService
    {
        private readonly InvoiceDbContext _dbContext;

        public SerialNumberService(InvoiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GenerateSerialNumberAsync()
        {
            var lastSerialNumber = await _dbContext.SalesItems
                .OrderByDescending(s => s.SerialNumber)
                .FirstOrDefaultAsync();

            return lastSerialNumber?.SerialNumber + 1 ?? 1;
        }

        public async Task<int> GetStartingSerialNumberAsync()
        {
            var firstSerialNumber = await _dbContext.SalesItems
                .OrderBy(s => s.SerialNumber)
                .Select(s => s.SerialNumber)
                .FirstOrDefaultAsync();

            return firstSerialNumber == 1 ? 1 : firstSerialNumber;
        }
    }
}
