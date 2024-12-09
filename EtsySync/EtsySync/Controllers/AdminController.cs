using EtsySync.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EtsySync.Controllers
{

    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;



        public AdminController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;

        }



        [Authorize(Policy = "AdminOnly")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadInvoicesCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                var zipFileId = await _invoiceService.GenerateInvoicesZipForCsvAsync(file);
                return Ok(new { ZipFileId = zipFileId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the file.", Details = ex.Message });
            }
        }

        [Authorize(Policy = "AdminOnly")]
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

        [Authorize(Policy = "AdminOnly")]
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

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            try
            {
                var invoices = await _invoiceService.GetInvoicesMetadataAsync();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadInvoicesAsync(Guid id)
        {
            try
            {

                var zipData = await _invoiceService.GetInvoicesZipByIdAsync(id);


                return File(zipData, "application/zip", $"Invoices_{id}.zip");
            }
            catch (Exception ex)
            {

                return NotFound(new { message = ex.Message });
            }
        }
    }
}
