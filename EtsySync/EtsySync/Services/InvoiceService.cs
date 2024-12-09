using EtsySync.Interface;
using SharedProject.Dtos;
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
        private readonly EncryptionService _encryptionService;

        public InvoiceService(
            ICsvParserService csvParserService,
            IInvoiceRepository invoiceRepository,
            IInvoiceGeneratorService invoiceGeneratorService,
            ISalesDataService salesDataService,
            EncryptionService encryptionService)
        {
            _csvParserService = csvParserService;
            _invoiceRepository = invoiceRepository;
            _invoiceGeneratorService = invoiceGeneratorService;
            _salesDataService = salesDataService;
            _encryptionService = encryptionService;
        }
        //private readonly ISerialNumberService _serialNumberService;
        //ISerialNumberService serialNumberService,
        //_serialNumberService = serialNumberService;

        public async Task<Guid> GenerateInvoicesZipForCsvAsync(IFormFile file)
        {

            var salesItems = _csvParserService.ParseCsv(file);
            if (salesItems == null || !salesItems.Any())
                throw new Exception("No valid sales data found in the CSV file.");


            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var sale in salesItems)
                {

                    if (await _invoiceRepository.ExistsBySerialNumberAsync(sale.SerialNumber))
                    {
                        continue;
                    }


                    var invoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem
                {
                    ItemName = sale.InvoiceItem.ItemName,
                    Quantity = sale.InvoiceItem.Quantity,
                    Price = sale.InvoiceItem.Price
                }
            };


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


                    var zipEntry = zipArchive.CreateEntry($"Invoice_{sale.SerialNumber}.xlsx", CompressionLevel.Optimal);
                    using var entryStream = zipEntry.Open();
                    await entryStream.WriteAsync(invoiceFile, 0, invoiceFile.Length);


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
                        FileData = invoiceFile,
                        FileName = $"Invoice_{sale.SerialNumber}.xlsx"
                    };

                    await _invoiceRepository.AddInvoiceAsync(generatedSalesItem);
                }
            }


            byte[] zipData = memoryStream.ToArray();


            var zipFileId = Guid.NewGuid();
            await _invoiceRepository.AddZipFileAsync(
                zipFileId,
                $"Invoices_{DateTime.UtcNow:yyyyMMddHHmmss}.zip",
                zipData,
                "Generated zip file containing all invoices."
            );


            return zipFileId;
        }

        public async Task<byte[]> GetInvoiceBySerialNumberAsync(long serialNumber)
        {
            return await _invoiceRepository.GetInvoiceFileDataBySerialNumberAsync(serialNumber);
        }

        public async Task<bool> DeleteInvoiceBySerialNumberAsync(long serialNumber)
        {
            // Perduodama istrynimo uzklausa informacijos valdymo sluoksniui
            return await _invoiceRepository.DeleteInvoiceFileAndDataAsync(serialNumber);
        }

        public async Task<List<InvoiceMetadataDto>> GetInvoicesMetadataAsync()
        {

            var invoices = await _invoiceRepository.GetAllInvoicesAsync();

            return invoices.Select(invoice => new InvoiceMetadataDto
            {
                SalesId = invoice.SalesId,
                FileName = invoice.FileName,
                CreatedDate = invoice.CreatedDate
            }).ToList();
        }

        public async Task<byte[]> GetInvoicesZipByIdAsync(Guid uploadId)
        {

            var zipFile = await _invoiceRepository.GetZipFileByIdAsync(uploadId);

            if (zipFile == null)
            {
                throw new Exception("Zip file not found.");
            }


            return zipFile.FileData;
        }


        //public async Task<bool> DeleteAllExcelFilesAndRelatedDataAsync()
        //{
        //    return await _invoiceRepository.DeleteAllExcelFilesAndRelatedDataAsync();
        //}

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

    }
}