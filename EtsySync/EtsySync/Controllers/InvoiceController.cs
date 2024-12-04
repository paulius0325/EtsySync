using EtsyGateway;
using EtsySync.Interface;
using EtsySync.Repositories;
using EtsySync.Services;
using Microsoft.AspNetCore.Mvc;
using SharedProject.Models;


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
        //_oauthService = oauthService;
        //private readonly OauthService _oauthService;

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                var invoicesZip = await _invoiceService.GenerateInvoicesZipForCsvAsync(file);

                return File(invoicesZip, "application/zip", "Invoices.zip");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("by-serial/{serialNumber}")]
        public async Task<IActionResult> GetInvoiceBySerialNumber(long serialNumber)
        {
            try
            {
                var fileData = await _invoiceService.GetInvoiceBySerialNumberAsync(serialNumber);
                return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Invoice_{serialNumber}.xlsx");
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("delete-invoice/{serialNumber}")]
        public async Task<IActionResult> DeleteInvoice(long serialNumber)
        {
            try
            {
                var isDeleted = await _invoiceService.DeleteInvoiceBySerialNumberAsync(serialNumber);
                if (!isDeleted)
                    return NotFound(new { Message = $"Invoice with Serial Number {serialNumber} not found." });

                return Ok(new { Message = "Invoice and associated data deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //[HttpDelete("delete-zip/{zipFileId}")]
        //public async Task<IActionResult> DeleteZipFile(Guid zipFileId)
        //{
        //    try
        //    {
        //        var isDeleted = await _invoiceService.DeleteZipFileAndRelatedDataAsync(zipFileId);
        //        if (!isDeleted)
        //            return NotFound(new { Message = $"Zip file with ID {zipFileId} not found." });

        //        return Ok(new { Message = "Zip file and related Excel files deleted successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}

        [HttpDelete("delete-all-excel")]
        public async Task<IActionResult> DeleteAllExcelFiles()
        {
            try
            {
                var isDeleted = await _invoiceService.DeleteAllExcelFilesAndRelatedDataAsync();
                if (!isDeleted)
                    return NotFound(new { Message = "No Excel files found to delete." });

                return Ok(new { Message = "All Excel files and related data deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }



        //[HttpGet("test-token")]
        //public async Task<IActionResult> TestOAuthToken()
        //{
        //    try
        //    {
        //        var token = await _oauthService.GetOAuthTokenAsync("m8v88jn7mmnpml3jqni0arb4", "44gjwzvhy4");
        //        return Ok(new { Token = token });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Error = ex.Message });
        //    }
        //}


        //[HttpPost("generate/{Shop_id}/{Listing_id}")]
        //public async Task<IActionResult> GenerateInvoice(int Shop_id, int Listing_id)
        //{
        //    try
        //    {
        //        var invoiceItems = await _receiptTransactionsService.GetInvoiceItemsFromReceiptsAsync(Shop_id, Listing_id);

        //        if (invoiceItems == null || !invoiceItems.Any())
        //        {
        //            return BadRequest(new { message = "No invoice items found for the specified shop and listing." });
        //        }

        //        await _invoiceService.GenerateAndSaveInvoiceAsync(Shop_id, Listing_id);

        //        return Ok(new { message = "Invoice generated and saved successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = "Error generating invoice", error = ex.Message });
        //    }
        //}

        //[HttpGet("download-all-invoices/{Shop_id}")]
        //public async Task<IActionResult> DownloadAllInvoices(int Shop_id)
        //{
        //    try
        //    {

        //        byte[] zipFile = await _invoiceService.GenerateInvoicesZipForAllReceiptsAsync(Shop_id);


        //        return File(zipFile, "application/zip", $"Invoices_Shop_{Shop_id}.zip");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = "Error generating invoices", error = ex.Message });
        //    }
        //}



    }
}