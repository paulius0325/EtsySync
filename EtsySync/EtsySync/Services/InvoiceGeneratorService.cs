using ClosedXML.Excel;
using EtsySync.Interface;
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
        

        public async Task<byte[]> GenerateInvoiceAsync(List<SalesItem> salesData, List<InvoiceItem> invoiceItems, DateTime date, int serialNumber, List<Client> clientsData)
        {
            byte[] fileData = GenerateInvoiceFile(salesData, invoiceItems, date, serialNumber, clientsData);
            return fileData;
        }

        private byte[] GenerateInvoiceFile(List<SalesItem> salesData, List<InvoiceItem> invoiceItems, DateTime date, int serialNumber, List<Client> clientsData)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Invoice");

                //------------------- Title -------------------
                var titleRange = worksheet.Range("B2:T2");
                titleRange.Merge();
                titleRange.Value = "SASKAITA - FAKTŪRA / INVOICE";
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Font.FontSize = 15;
                titleRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //------------------- Serial Number -------------------
                var serialTextRange = worksheet.Range("G4:I4");
                serialTextRange.Merge();
                serialTextRange.Value = "Serija / Serial Nr:";
                serialTextRange.Style.Font.FontSize = 11;
                serialTextRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                var serialNumberRange = worksheet.Cell("L4");
                serialNumberRange.Value = serialNumber.ToString();
                serialNumberRange.Style.Font.Bold = true;
                serialNumberRange.Style.Font.FontSize = 12;
                serialNumberRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //------------------- Date -------------------
                var dateRange = worksheet.Range("J6:L6");
                dateRange.Merge();
                dateRange.Value = $"{date.ToString("yyyy-MM-dd")}";
                dateRange.Style.Font.FontSize = 11;
                dateRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //------------------- Table -------------------
                _salesDataTableService.WorksheetsTable(worksheet, invoiceItems);

                //------------------Client---------------------
                _clientService.WorksheetClient(worksheet, clientsData);
                //------------------Seller---------------------
                _sellerService.WorksheetSeller(worksheet);

                //------------------- Save to Stream -------------------
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
