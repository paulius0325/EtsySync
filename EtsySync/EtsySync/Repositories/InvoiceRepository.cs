using EtsySync.Data;
using EtsySync.Interface;
using Microsoft.EntityFrameworkCore;
using SharedProject.Models;
using System.IO.Compression;

namespace EtsySync.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly InvoiceDbContext _dbContext;

        public InvoiceRepository(InvoiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddInvoiceAsync(SalesItem invoice)
        {
            _dbContext.SalesItems.Add(invoice);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SalesItem?> GetLastGeneratedInvoiceAsync()
        {
            return await _dbContext.SalesItems
                .OrderByDescending(i => i.CreatedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<SalesItem?> GetInvoiceBySerialNumberAsync(int serialNumber)
        {
            return await _dbContext.SalesItems
                .FirstOrDefaultAsync(i => i.SerialNumber == serialNumber);
        }

        public async Task<IEnumerable<SalesItem>> GetAllInvoicesAsync()
        {
            return await _dbContext.SalesItems
                .OrderBy(i => i.CreatedDate)
                .ToListAsync();
        }
    }
}
