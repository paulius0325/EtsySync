using EtsySync.Data;
using EtsySync.Interface;
using EtsySync.Services;
using Microsoft.EntityFrameworkCore;
using SharedProject.Models;

namespace EtsySync.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly InvoiceDbContext _dbContext;
        private readonly EncryptionService _encryptionService;

        public InvoiceRepository(InvoiceDbContext dbContext, EncryptionService encryptionService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
        }



        public async Task AddInvoiceAsync(SalesItem salesItem)
        {

            string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();
            byte[] encryptedFileData = _encryptionService.EncryptData(salesItem.FileData, encryptionKey);

            salesItem.FileData = encryptedFileData;

            _dbContext.SalesItems.Add(salesItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddZipFileAsync(string fileName, byte[] fileData, string? description = null)
        {
            var zipFile = new CompressedFile
            {
                ZipFileId = Guid.NewGuid(),
                FileName = fileName,
                FileData = fileData,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.ZipFiles.Add(zipFile);
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<SalesItem?> GetLastGeneratedInvoiceAsync()
        //{
        //    var salesItem = await _dbContext.SalesItems
        //        .OrderByDescending(i => i.CreatedDate)
        //        .FirstOrDefaultAsync();

        //    if (salesItem != null)
        //    {

        //        string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();
        //        salesItem.FileData = _encryptionService.DecryptData(salesItem.FileData, encryptionKey);
        //    }

        //    return salesItem;
        //}

        //public async Task<SalesItem?> GetInvoiceBySerialNumberAsync(int serialNumber)
        //{
        //    var salesItem = await _dbContext.SalesItems
        //        .FirstOrDefaultAsync(i => i.SerialNumber == serialNumber);

        //    if (salesItem != null)
        //    {

        //        string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();
        //        salesItem.FileData = _encryptionService.DecryptData(salesItem.FileData, encryptionKey);
        //    }

        //    return salesItem;
        //}

        //public async Task<IEnumerable<SalesItem>> GetAllInvoicesAsync()
        //{
        //    var salesItems = await _dbContext.SalesItems
        //        .OrderBy(i => i.CreatedDate)
        //        .ToListAsync();

        //    string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();


        //    foreach (var salesItem in salesItems)
        //    {
        //        salesItem.FileData = _encryptionService.DecryptData(salesItem.FileData, encryptionKey);
        //    }

        //    return salesItems;
        //}

        //public async Task UpdateInvoiceAsync(SalesItem salesItem)
        //{

        //    string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();
        //    salesItem.FileData = _encryptionService.EncryptData(salesItem.FileData, encryptionKey);

        //    _dbContext.SalesItems.Update(salesItem);
        //    await _dbContext.SaveChangesAsync();
        //}

        //public async Task<SalesItem> GetByFileNameAsync(string fileName)
        //{
        //    return await _dbContext.SalesItems.FirstOrDefaultAsync(s => s.FileName == fileName);
        //}

        //public async Task<SalesItem?> GetInvoiceBySalesIdAsync(Guid salesId)
        //{
        //    return await _dbContext.SalesItems
        //        .FirstOrDefaultAsync(s => s.SalesId == salesId);
        //}




    }
}

