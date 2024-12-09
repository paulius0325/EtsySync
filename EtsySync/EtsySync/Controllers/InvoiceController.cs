using EtsySync.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace EtsySync.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class InvoiceController : ControllerBase
    //{
    //    private readonly IInvoiceService _invoiceService;
       
       

    //    public InvoiceController(IInvoiceService invoiceService)
    //    {
    //        _invoiceService = invoiceService;
          
    //    }


        
    //    [HttpPost("upload")]
    //    public async Task<IActionResult> UploadInvoicesCsv(IFormFile file)
    //    {
    //        if (file == null || file.Length == 0)
    //        {
    //            return BadRequest("No file uploaded.");
    //        }

    //        try
    //        {
    //            var zipFileId = await _invoiceService.GenerateInvoicesZipForCsvAsync(file);
    //            return Ok(new { ZipFileId = zipFileId });
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(500, new { Message = "An error occurred while processing the file.", Details = ex.Message });
    //        }
    //    }

       
    //    [HttpGet("by-serial/{serialNumber}")]
    //    public async Task<IActionResult> GetInvoiceBySerialNumber(long serialNumber)
    //    {
    //        try
    //        {
    //            var fileData = await _invoiceService.GetInvoiceBySerialNumberAsync(serialNumber);
    //            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Invoice_{serialNumber}.xlsx");
    //        }
    //        catch (Exception ex)
    //        {
    //            return NotFound(new { message = ex.Message });
    //        }
    //    }

        
    //    [HttpDelete("delete-invoice/{serialNumber}")]
    //    public async Task<IActionResult> DeleteInvoice(long serialNumber)
    //    {
    //        try
    //        {
    //            var isDeleted = await _invoiceService.DeleteInvoiceBySerialNumberAsync(serialNumber);
    //            if (!isDeleted)
    //                return NotFound(new { Message = $"Invoice with Serial Number {serialNumber} not found." });

    //            return Ok(new { Message = "Invoice and associated data deleted successfully." });
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(new { Message = ex.Message });
    //        }
    //    }

        
    //    [HttpGet]
    //    public async Task<IActionResult> GetInvoices()
    //    {
    //        try
    //        {
    //            var invoices = await _invoiceService.GetInvoicesMetadataAsync();
    //            return Ok(invoices);
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(500, $"Internal server error: {ex.Message}");
    //        }
    //    }
       
    //    [HttpGet("{id}/download")]
    //    public async Task<IActionResult> DownloadInvoicesAsync(Guid id)
    //    {
    //        try
    //        {
                
    //            var zipData = await _invoiceService.GetInvoicesZipByIdAsync(id);

             
    //            return File(zipData, "application/zip", $"Invoices_{id}.zip");
    //        }
    //        catch (Exception ex)
    //        {
             
    //            return NotFound(new { message = ex.Message });
    //        }
    //    }


        //[HttpDelete("delete-all-excel")]
        //public async Task<IActionResult> DeleteAllExcelFiles()
        //{
        //    try
        //    {
        //        var isDeleted = await _invoiceService.DeleteAllExcelFilesAndRelatedDataAsync();
        //        if (!isDeleted)
        //            return NotFound(new { Message = "No Excel files found to delete." });

        //        return Ok(new { Message = "All Excel files and related data deleted successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}




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


    //}
}