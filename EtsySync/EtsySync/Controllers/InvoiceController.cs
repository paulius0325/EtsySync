using EtsyGateway;
using EtsySync.Interface;
using EtsySync.Services;
using Microsoft.AspNetCore.Mvc;


namespace EtsySync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ReceiptTransactionsService _receiptTransactionsService;

        public InvoiceController(IInvoiceService invoiceService, ReceiptTransactionsService receiptTransactionsService)
        {
            _invoiceService = invoiceService;
            _receiptTransactionsService = receiptTransactionsService;
        }

        [HttpPost("generate/{Shop_id}/{Listing_id}")]
        public async Task<IActionResult> GenerateInvoice(int Shop_id, int Listing_id)
        {
            try
            {
                var invoiceItems = await _receiptTransactionsService.GetInvoiceItemsFromReceiptsAsync(Shop_id, Listing_id);

                if (invoiceItems == null || !invoiceItems.Any())
                {
                    return BadRequest(new { message = "No invoice items found for the specified shop and listing." });
                }

                await _invoiceService.GenerateAndSaveInvoiceAsync(Shop_id, Listing_id);

                return Ok(new { message = "Invoice generated and saved successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error generating invoice", error = ex.Message });
            }
        }

        [HttpGet("download-all-invoices/{Shop_id}")]
        public async Task<IActionResult> DownloadAllInvoices(int Shop_id)
        {
            try
            {
                
                byte[] zipFile = await _invoiceService.GenerateInvoicesZipForAllReceiptsAsync(Shop_id);

               
                return File(zipFile, "application/zip", $"Invoices_Shop_{Shop_id}.zip");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error generating invoices", error = ex.Message });
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadLastInvoice()
        {
            try
            {
                var (fileData, fileName) = await _invoiceService.GetLastGeneratedInvoiceFileAsync();

                if (fileData == null || fileName == null)
                {
                    return NotFound(new { message = "No invoice available for download." });
                }

                return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error downloading invoice", error = ex.Message });
            }
        }

        [HttpGet("download/{serialNumber}")]
        public async Task<IActionResult> DownloadInvoiceBySerialNumber(int serialNumber)
        {
            try
            {
                var (fileData, fileName) = await _invoiceService.GetInvoiceBySerialNumberAsync(serialNumber);

                if (fileData == null || fileName == null)
                {
                    return NotFound(new { message = $"Invoice with serial number {serialNumber} not found." });
                }

                return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error downloading invoice", error = ex.Message });
            }
        }


        [HttpPost("generate-empty-invoice")]
        public async Task<IActionResult> GenerateEmptyInvoice(string? fileName = null)
        {
            try
            {
                await _invoiceService.GenerateAndSaveEmptyInvoiceAsync(fileName);
                return Ok(new { message = $"Empty invoice '{fileName ?? "EmptyInvoice"}.xlsx' generated and saved successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error generating empty invoice", error = ex.Message });
            }
        }

        [HttpGet("download-all-invoices-zip")]
        public async Task<IActionResult> DownloadAllInvoicesZip()
        {
            try
            {
               
                var allInvoices = await _invoiceService.GetAllInvoicesFromDatabaseAsync();

                if (allInvoices == null || !allInvoices.Any())
                {
                    
                    return NotFound(new { message = "No invoices found in the database" });
                }

               
                byte[] zipFile = await _invoiceService.CreateInvoicesZip(allInvoices);

               
                return File(zipFile, "application/zip", "Invoices.zip");
            }
            catch (Exception ex)
            {
               
                return BadRequest(new { message = "Error generating the zip file", error = ex.Message });
            }
        }
    }
}
