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

        public async Task<bool> ExistsBySerialNumberAsync(long serialNumber)
        {
            return await _dbContext.SalesItems.AnyAsync(x => x.SerialNumber == serialNumber);
        }

        public async Task<byte[]> GetInvoiceFileDataBySerialNumberAsync(long serialNumber)
        {
            var invoice = await _dbContext.SalesItems
                .Where(i => i.SerialNumber == serialNumber)
                .Select(i => i.FileData)
                .FirstOrDefaultAsync();

            if (invoice == null || invoice.Length == 0)
                throw new Exception($"Invoice file data for Serial Number {serialNumber} not found.");

            string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();
            byte[] decryptedFileData = _encryptionService.DecryptData(invoice, encryptionKey);

            return decryptedFileData;
        }

        public async Task<bool> DeleteInvoiceFileAndDataAsync(long serialNumber)
        {
            // gaunamas saskaitos failas naudojant identifikacijos numeri
            var salesItem = await _dbContext.SalesItems
                .Include(s => s.InvoiceItem)
                .Include(s => s.Client)
                .FirstOrDefaultAsync(s => s.SerialNumber == serialNumber);

            if (salesItem == null)
                return false;

            // Istrinamas excel failo turinys
            if (salesItem.InvoiceItem != null)
            {
                _dbContext.InvoiceItems.Remove(salesItem.InvoiceItem);
            }

            if (salesItem.Client != null)
            {
                _dbContext.Clients.Remove(salesItem.Client);
            }

            // Istrinamas excel failas
            _dbContext.SalesItems.Remove(salesItem);

            // Issaugomi pakitimai duomenu bazeje
            await _dbContext.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> DeleteZipFileAsync(Guid zipFileId)
        //{
        //    // Gaunamas zip failas is duomenu bazes naudojant identifikacini numeri
        //    var zipFile = await _dbContext.ZipFiles.FirstOrDefaultAsync(z => z.ZipFileId == zipFileId);
        //    if (zipFile == null)
        //        return false;

        //    // Randami susija failai, naudojant tam tikrus kriterijus (e.g., filename patterns, zip file metadata)
        //    var relatedExcelFiles = await _dbContext.SalesItems
        //        .Where(s => s.FileName.StartsWith($"Invoices_{zipFileId}")) // Adjust this condition as needed
        //        .ToListAsync();

        //    // Istrinami susija excel failai ir su jais susijusi infomacija
        //    foreach (var salesItem in relatedExcelFiles)
        //    {
        //        if (salesItem.InvoiceItem != null)
        //        {
        //            _dbContext.InvoiceItems.Remove(salesItem.InvoiceItem);
        //        }

        //        if (salesItem.Client != null)
        //        {
        //            _dbContext.Clients.Remove(salesItem.Client);
        //        }

        //        _dbContext.SalesItems.Remove(salesItem);
        //    }

        //    // Istrinamas zip failas
        //    _dbContext.ZipFiles.Remove(zipFile);

        //    // Pakitimai issaugomi duomenu bazeje
        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> DeleteAllExcelFilesAndRelatedDataAsync()
        {
            // Gaunami visi Excel failai
            var allSalesItems = await _dbContext.SalesItems.Include(s => s.InvoiceItem).Include(s => s.Client).ToListAsync();
            if (!allSalesItems.Any())
                return false;

            // Istrinama atskirai issaugota Excel failo informacija
            foreach (var salesItem in allSalesItems)
            {
                if (salesItem.InvoiceItem != null)
                {
                    _dbContext.InvoiceItems.Remove(salesItem.InvoiceItem);
                }

                if (salesItem.Client != null)
                {
                    _dbContext.Clients.Remove(salesItem.Client);
                }

                _dbContext.SalesItems.Remove(salesItem);
            }

            // Pakeitimai issaugomi duomenu bazeje
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}


