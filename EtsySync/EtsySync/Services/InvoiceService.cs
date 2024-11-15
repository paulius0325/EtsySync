using EtsyGateway;
using EtsySync.Data;
using EtsySync.Interface;
using Microsoft.EntityFrameworkCore;
using SharedProject.Models;
using System.IO.Compression;


namespace EtsySync.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ISerialNumberService _serialNumberService;
        private readonly IInvoiceGeneratorService _invoiceGeneratorService;
        private readonly ISalesDataService _salesDataService;
        private readonly ReceiptTransactionsService _receiptTransactionsService;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            ISerialNumberService serialNumberService,
            IInvoiceGeneratorService invoiceGeneratorService,
            ISalesDataService salesDataService,
            ReceiptTransactionsService receiptTransactionsService)
        {
            _invoiceRepository = invoiceRepository;
            _serialNumberService = serialNumberService;
            _invoiceGeneratorService = invoiceGeneratorService;
            _salesDataService = salesDataService;
            _receiptTransactionsService = receiptTransactionsService;
        }

        public async Task GenerateAndSaveInvoiceAsync(int Shop_id, int Listing_id)
        {
            var invoiceItemsDto = await _receiptTransactionsService.GetInvoiceItemsFromReceiptsAsync(Shop_id, Listing_id);
            await GenerateAndSaveInvoiceFromItemsAsync(Shop_id, invoiceItemsDto);
        }

        public async Task GenerateAndSaveInvoiceForAllReceiptsAsync(int Shop_id)
        {
            var allInvoiceItemsDto = await _receiptTransactionsService.GetAllInvoiceItemsAsync(Shop_id);
            await GenerateAndSaveInvoiceFromItemsAsync(Shop_id, allInvoiceItemsDto);
        }

        public async Task<byte[]> GenerateInvoicesZipForAllReceiptsAsync(int Shop_id)
        {
            var allInvoiceItemsDto = await _receiptTransactionsService.GetAllInvoiceItemsAsync(Shop_id);

            if (allInvoiceItemsDto == null || !allInvoiceItemsDto.Any())
                throw new Exception("No receipt transactions found for the given shop.");

            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                int serialNumber = await _serialNumberService.GetStartingSerialNumberAsync();

                foreach (var invoiceItem in allInvoiceItemsDto)
                {
                    var salesData = await _salesDataService.GetSalesDataAsync();
                    var invoiceItems = new List<InvoiceItem>
                    {
                        new InvoiceItem
                        {
                            ItemName = invoiceItem.ItemName,
                            Quantity = invoiceItem.Quantity,
                            Price = invoiceItem.Price
                        }
                    };

                    var clientsData = GetClientsData();
                    byte[] invoiceFile = await _invoiceGeneratorService.GenerateInvoiceAsync(
                        salesData,
                        invoiceItems,
                        DateTime.Now,
                        serialNumber++,
                        clientsData
                    );

                    var zipEntry = zipArchive.CreateEntry($"Invoice_{serialNumber - 1}.xlsx", CompressionLevel.Optimal);
                    using var entryStream = zipEntry.Open();
                    await entryStream.WriteAsync(invoiceFile, 0, invoiceFile.Length);
                }
            }

            return memoryStream.ToArray();
        }

        private async Task GenerateAndSaveInvoiceFromItemsAsync(int Shop_id, List<InvoiceItemDto> invoiceItemsDto)
        {
            int serialNumber = await _serialNumberService.GenerateSerialNumberAsync();
            var salesData = await _salesDataService.GetSalesDataAsync();

            var invoiceItems = invoiceItemsDto.Select(dto => new InvoiceItem
            {
                ItemName = dto.ItemName,
                Quantity = dto.Quantity,
                Price = dto.Price
            }).ToList();

            var clientsData = GetClientsData();

            byte[] invoiceFile = await _invoiceGeneratorService.GenerateInvoiceAsync(salesData, invoiceItems, DateTime.Now, serialNumber, clientsData);

            var salesItem = new SalesItem
            {
                FileData = invoiceFile,
                FileName = $"Invoice_{serialNumber}.xlsx",
                CreatedDate = DateTime.Now,
                SerialNumber = serialNumber
            };

            await _invoiceRepository.AddInvoiceAsync(salesItem);
        }

        public async Task<(byte[]? FileData, string? FileName)> GetLastGeneratedInvoiceFileAsync()
        {
            var lastInvoice = await _invoiceRepository.GetLastGeneratedInvoiceAsync();
            return (lastInvoice?.FileData, lastInvoice?.FileName);
        }

        public async Task<(byte[]? FileData, string? FileName)> GetInvoiceBySerialNumberAsync(int serialNumber)
        {
            var invoice = await _invoiceRepository.GetInvoiceBySerialNumberAsync(serialNumber);
            return (invoice?.FileData, invoice?.FileName);
        }

        public async Task GenerateAndSaveEmptyInvoiceAsync(string? fileName = null)
        {
            int serialNumber = await _serialNumberService.GenerateSerialNumberAsync();

            var emptySalesData = new List<SalesItem>();
            var emptyInvoiceItems = new List<InvoiceItem>();
            DateTime currentDate = DateTime.Now;
            var clientsData = GetClientsData();

            byte[] fileData = await _invoiceGeneratorService.GenerateInvoiceAsync(
                emptySalesData,
                emptyInvoiceItems,
                currentDate,
                serialNumber,
                clientsData
            );

            var salesItem = new SalesItem
            {
                FileData = fileData,
                FileName = fileName ?? $"EmptyInvoice_{serialNumber}.xlsx",
                CreatedDate = DateTime.Now,
                SerialNumber = serialNumber
            };

            await _invoiceRepository.AddInvoiceAsync(salesItem);
        }

        public async Task<byte[]> CreateInvoicesZip(IEnumerable<(byte[] fileData, string fileName)> allInvoices)
        {
            using var memoryStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var invoice in allInvoices)
                {
                    var zipEntry = zipArchive.CreateEntry(invoice.fileName, CompressionLevel.Optimal);
                    using (var entryStream = zipEntry.Open())
                    {
                        await entryStream.WriteAsync(invoice.fileData, 0, invoice.fileData.Length);
                    }
                }
            }

            return memoryStream.ToArray();
        }

        public async Task<IEnumerable<(byte[] fileData, string fileName)>> GetAllInvoicesFromDatabaseAsync()
        {
            var invoices = await _invoiceRepository.GetAllInvoicesAsync();
            return invoices.Select(invoice => (invoice.FileData, invoice.FileName));
        }

        private List<Client> GetClientsData()
        {
            return new List<Client>
            {
                new Client
                {
                    Buyer = "Sample Buyer",
                    Address = "Sample Address"
                }
            };
        }
    }
}
