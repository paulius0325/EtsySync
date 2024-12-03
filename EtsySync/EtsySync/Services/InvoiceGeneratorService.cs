using ClosedXML.Excel;
using EtsySync.Interface;
using SharedProject.Dtos;
using SharedProject.Models;

namespace EtsySync.Services
{
    public class InvoiceGeneratorService : IInvoiceGeneratorService
    {
        private readonly SalesDataTableService _salesDataTableService;
        private readonly SellerService _sellerService;
        private readonly ClientService _clientService;

        public InvoiceGeneratorService(SalesDataTableService salesDataTableService, SellerService sellerService, ClientService clientService)
        {
            _salesDataTableService = salesDataTableService;
            _sellerService = sellerService;
            _clientService = clientService;
        }


        public async Task<byte[]> GenerateInvoiceAsync(List<SalesItem> salesData, List<InvoiceItem> invoiceItems, DateTime date, long serialNumber, List<Client> clientsData)
        {

            List<ClientDto> clientsDtoData = clientsData.Select(client => new ClientDto
            {
                Buyer = client.Buyer,
                Address = client.Address
            }).ToList();


            byte[] fileData = GenerateInvoiceFile(salesData, invoiceItems, date, serialNumber, clientsDtoData);
            return fileData;
        }

        private byte[] GenerateInvoiceFile(List<SalesItem> salesData, List<InvoiceItem> invoiceItems, DateTime date, long serialNumber, List<ClientDto> clientsDtoData)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Invoice");

                // Pavadinimo formatavimas
                var titleRange = worksheet.Range("B2:T2");
                titleRange.Merge();
                titleRange.Value = "SASKAITA - FAKTŪRA / INVOICE";
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Font.FontSize = 15;
                titleRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                worksheet.Cell("J4").Value = $"Serial Number: {serialNumber}";
                worksheet.Cell("J5").Value = $"Date: {date:yyyy-MM-dd}";

                _salesDataTableService.WorksheetsTable(worksheet, invoiceItems);


                _clientService.WorksheetClient(worksheet, clientsDtoData);

                _sellerService.WorksheetSeller(worksheet);

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

    }
}