using ClosedXML.Excel;
using EtsyGateway;
using EtsySync.Data;
using EtsySync.Interface;
using SharedProject.Models;
using System.IO.Compression;


namespace EtsySync.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ICsvParserService _csvParserService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceGeneratorService _invoiceGeneratorService;
        private readonly ISalesDataService _salesDataService;
        private readonly ReceiptTransactionsService _receiptTransactionsService;
        private readonly EncryptionService _encryptionService;

        public InvoiceService(
            ICsvParserService csvParserService,
            IInvoiceRepository invoiceRepository,
            IInvoiceGeneratorService invoiceGeneratorService,
            ISalesDataService salesDataService,
            ReceiptTransactionsService receiptTransactionsService,
            EncryptionService encryptionService)
        {
            _csvParserService = csvParserService;
            _invoiceRepository = invoiceRepository;
            _invoiceGeneratorService = invoiceGeneratorService;
            _salesDataService = salesDataService;
            _receiptTransactionsService = receiptTransactionsService;
            _encryptionService = encryptionService;
        }
        //private readonly ISerialNumberService _serialNumberService;
        //ISerialNumberService serialNumberService,
        //_serialNumberService = serialNumberService;
        public async Task<byte[]> GenerateInvoicesZipForCsvAsync(IFormFile file)
        {
            var salesItems = _csvParserService.ParseCsv(file);
            if (salesItems == null || !salesItems.Any())
                throw new Exception("No valid sales data found in the CSV file.");

            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var sale in salesItems)
                {
                    var invoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem
                {
                    ItemName = sale.InvoiceItem.ItemName,
                    Quantity = sale.InvoiceItem.Quantity,
                    Price = sale.InvoiceItem.Price
                }
            };

                    // Kuriamas Excel saskaitos failas
                    byte[] invoiceFile = await _invoiceGeneratorService.GenerateInvoiceAsync(
                        new List<SalesItem> { sale },
                        invoiceItems,
                        sale.CreatedDate,
                        sale.SerialNumber,
                        new List<Client>
                        {
                    new Client
                    {
                        Buyer = sale.Client.Buyer,
                        Address = sale.Client.Address
                    }
                        }
                    );

                    // Kiekvienas Excel saskaitos failas idedamas i zip faila
                    var zipEntry = zipArchive.CreateEntry($"Invoice_{sale.SerialNumber}.xlsx", CompressionLevel.Optimal);
                    using var entryStream = zipEntry.Open();
                    await entryStream.WriteAsync(invoiceFile, 0, invoiceFile.Length);

                    // Kiekvienas excel saskaitos failas atskirai issaugojamas duomenu bazeje
                    var generatedSalesItem = new SalesItem
                    {
                        SalesId = Guid.NewGuid(),
                        CreatedDate = sale.CreatedDate,
                        SerialNumber = sale.SerialNumber,
                        InvoiceItem = new InvoiceItem
                        {
                            ItemName = sale.InvoiceItem.ItemName,
                            Quantity = sale.InvoiceItem.Quantity,
                            Price = sale.InvoiceItem.Price
                        },
                        Client = new Client
                        {
                            Buyer = sale.Client.Buyer,
                            Address = sale.Client.Address
                        },
                        FileData = invoiceFile, // Pridedamas saskaitos failo informacija
                        FileName = $"Invoice_{sale.SerialNumber}.xlsx" // Pridedamas saskaitos failo pavadinimas
                    };

                    await _invoiceRepository.AddInvoiceAsync(generatedSalesItem);
                }
            }

            byte[] zipData = memoryStream.ToArray(); // Gaunama Zip failo informacija

            // Zip failas issaugojamas duomenu bazeje
            await _invoiceRepository.AddZipFileAsync(
                $"Invoices_{DateTime.UtcNow:yyyyMMddHHmmss}.zip",
                zipData,
                "Generated zip file containing all invoices."
            );

            return zipData;
        }


        //public async Task GenerateAndSaveInvoiceAsync(int Shop_id, int Listing_id)
        //    {
        //      var invoiceItemsDto = await _receiptTransactionsService.GetInvoiceItemsFromReceiptsAsync(Shop_id, Listing_id);
        //      await GenerateAndSaveInvoiceFromItemsAsync(Shop_id, invoiceItemsDto);
        //    }

        //public async Task GenerateAndSaveInvoiceForAllReceiptsAsync(int Shop_id)
        //{
        //    var allInvoiceItemsDto = await _receiptTransactionsService.GetAllInvoiceItemsAsync(Shop_id);
        //    await GenerateAndSaveInvoiceFromItemsAsync(Shop_id, allInvoiceItemsDto);
        //}

        //public async Task<byte[]> GenerateInvoicesZipForAllReceiptsAsync(int Shop_id)
        //{
        //    var allInvoiceItemsDto = await _receiptTransactionsService.GetAllInvoiceItemsAsync(Shop_id);

        //    if (allInvoiceItemsDto == null || !allInvoiceItemsDto.Any())
        //        throw new Exception("No receipt transactions found for the given shop.");

        //    using var memoryStream = new MemoryStream();
        //    using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //    {
        //        //int serialNumber = await _serialNumberService.GetStartingSerialNumberAsync();

        //        foreach (var invoiceItem in allInvoiceItemsDto)
        //        {
        //            var salesData = await _salesDataService.GetSalesDataAsync();
        //            var invoiceItems = new List<InvoiceItem>
        //            {
        //                new InvoiceItem
        //                {
        //                    ItemName = invoiceItem.ItemName,
        //                    Quantity = invoiceItem.Quantity,
        //                    Price = invoiceItem.Price
        //                }
        //            };

        //            //var clientsData = GetClientsData();
        //            //byte[] invoiceFile = await _invoiceGeneratorService.GenerateInvoiceAsync(
        //            //    salesData,
        //            //    invoiceItems,
        //            //    DateTime.Now,
        //            //    serialNumber++,
        //            //    clientsData
        //            //);

        //            //var zipEntry = zipArchive.CreateEntry($"Invoice_{serialNumber - 1}.xlsx", CompressionLevel.Optimal);
        //            //using var entryStream = zipEntry.Open();
        //            //await entryStream.WriteAsync(invoiceFile, 0, invoiceFile.Length);
        //        }
        //    }

        //    return memoryStream.ToArray();
        //}

        //private async Task GenerateAndSaveInvoiceFromItemsAsync(int Shop_id, List<InvoiceItemDto> invoiceItemsDto)
        //{
        //    //int serialNumber = await _serialNumberService.GenerateSerialNumberAsync();
        //    var salesData = await _salesDataService.GetSalesDataAsync();

        //    var invoiceItems = invoiceItemsDto.Select(dto => new InvoiceItem
        //    {
        //        ItemName = dto.ItemName,
        //        Quantity = dto.Quantity,
        //        Price = dto.Price
        //    }).ToList();

        //    //var clientsData = GetClientsData();

        //    //byte[] invoiceFile = await _invoiceGeneratorService.GenerateInvoiceAsync(salesData, invoiceItems, DateTime.Now, serialNumber, clientsData);

        //    //var salesItem = new SalesItem
        //    //{
        //    //    FileData = invoiceFile,
        //    //    FileName = $"Invoice_{serialNumber}.xlsx",
        //    //    CreatedDate = DateTime.Now,
        //    //    SerialNumber = serialNumber
        //    //};

        //    //await _invoiceRepository.AddInvoiceAsync(salesItem);
        //}

        //public async Task<(byte[]? FileData, string? FileName)> GetLastGeneratedInvoiceFileAsync()
        //{
        //    var lastInvoice = await _invoiceRepository.GetLastGeneratedInvoiceAsync();

        //    if (lastInvoice == null)
        //    {
        //        return (null, null);
        //    }


        //    string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();


        //    byte[] decryptedFileData = _encryptionService.DecryptData(lastInvoice.FileData, encryptionKey);
        //    return (decryptedFileData, lastInvoice.FileName);
        //}

        //public async Task<(byte[]? FileData, string? FileName)> GetInvoiceBySerialNumberAsync(int serialNumber)
        //{
        //    var invoice = await _invoiceRepository.GetInvoiceBySerialNumberAsync(serialNumber);

        //    if (invoice == null)
        //    {
        //        return (null, null);
        //    }


        //    string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();


        //    byte[] decryptedFileData = _encryptionService.DecryptData(invoice.FileData, encryptionKey);
        //    return (decryptedFileData, invoice.FileName);
        //}



        //public async Task GenerateAndSaveEmptyInvoiceAsync(string? fileName = null)
        //{
            //int serialNumber = await _serialNumberService.GenerateSerialNumberAsync();

            //var emptySalesData = new List<SalesItem>();
            //var emptyInvoiceItems = new List<InvoiceItem>();
            //DateTime currentDate = DateTime.Now;
            //var clientsData = GetClientsData();

            //byte[] fileData = await _invoiceGeneratorService.GenerateInvoiceAsync(
            //    emptySalesData,
            //    emptyInvoiceItems,
            //    currentDate,
            //    serialNumber,
            //    clientsData
            //);


            //string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();
            //byte[] encryptedFileData = _encryptionService.EncryptData(fileData, encryptionKey);

            //var salesItem = new SalesItem
            //{
            //    FileData = encryptedFileData,
            //    FileName = fileName ?? $"EmptyInvoice_{serialNumber}.xlsx",
            //    CreatedDate = DateTime.Now,
            //    SerialNumber = serialNumber
            //};

            //await _invoiceRepository.AddInvoiceAsync(salesItem);
       // }

        //public async Task<byte[]> CreateInvoicesZip(IEnumerable<(byte[] fileData, string fileName)> allInvoices, string password)
        //{
        //    using var memoryStream = new MemoryStream();


        //    using (var zipStream = new ZipOutputStream(memoryStream))
        //    {
        //        zipStream.Password = password;
        //        zipStream.IsStreamOwner = false;
        //        zipStream.SetLevel(5);

        //        foreach (var invoice in allInvoices)
        //        {


        //            string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();
        //            byte[] encryptedFileData = _encryptionService.EncryptData(invoice.fileData, encryptionKey);


        //            var zipEntry = new ZipEntry($"{invoice.fileName}.xlsx")
        //            {
        //                DateTime = DateTime.Now
        //            };

        //            zipStream.PutNextEntry(zipEntry);


        //            await zipStream.WriteAsync(invoice.fileData, 0, invoice.fileData.Length);
        //            zipStream.CloseEntry();
        //        }
        //    }

        //    return memoryStream.ToArray();
        //}

        //public async Task<IEnumerable<(byte[] fileData, string fileName)>> GetAllInvoicesFromDatabaseAsync()
        //{
        //    var invoices = await _invoiceRepository.GetAllInvoicesAsync();

        //    string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();

        //    return invoices.Select(invoice =>
        //    {
        //        byte[] decryptedFileData = _encryptionService.DecryptData(invoice.FileData, encryptionKey);
        //        return (decryptedFileData, invoice.FileName);
        //    });
        //}

        //private List<Client> GetClientsData()
        //{
        //    return new List<Client>
        //    {
        //        new Client
        //        {
        //            Buyer = "Buyer",
        //            Address = "Address"
        //        }
        //    };
        //}

        //public async Task SaveOrUpdateUploadedFileAsync(IFormFile file, int serialNumber)
        //{
        //    if (file == null || file.Length == 0)
        //        throw new ArgumentException("Invalid file.");

        //    var fileName = file.FileName;

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(memoryStream);
        //        var fileData = memoryStream.ToArray();

        //        string encryptionKey = await _encryptionService.GetEncryptionKeyAsync();
        //        byte[] encryptedFileData = _encryptionService.EncryptData(fileData, encryptionKey);


                //var existingSalesItem = await _invoiceRepository.GetInvoiceBySerialNumberAsync(serialNumber);
                //if (existingSalesItem != null)
                //{

                //    existingSalesItem.FileData = encryptedFileData;
                //    existingSalesItem.FileName = fileName;
                //    await _invoiceRepository.UpdateInvoiceAsync(existingSalesItem);
                //}
                //else
                //{

                //    var newSalesItem = new SalesItem
                //    {
                //        SerialNumber = serialNumber,
                //        FileData = encryptedFileData,
                //        FileName = fileName,
                //        CreatedDate = DateTime.UtcNow,
                //    };
                //    await _invoiceRepository.AddInvoiceAsync(newSalesItem);
                //}
           // }
       // }

        //public async Task<int> GetNextSerialNumberAsync()
        //{

        //    return await _serialNumberService.GenerateSerialNumberAsync();
        //}
    }
}