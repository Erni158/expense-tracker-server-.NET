using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Core;
using WebApplication2.DTOs;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptController : BaseController
    {
        private readonly ReceiptService _receiptService;

        public ReceiptController(ReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ReceiptResponse>>> GetReceipts([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var receipts = await _receiptService.GetReceiptsAsync(UserId, start, end);
            return Ok(receipts);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReceiptResponse>> PostReceipt([FromBody] ReceiptRequest receiptRequest)
        {
            var newReceipt = new Receipt
            {
                Name = receiptRequest.Name,
                Category = receiptRequest.Category,
                Amount = receiptRequest.Amount,
                List = receiptRequest.List,
            };
            var receipt = await _receiptService.AddReceiptAsync(newReceipt);
            return Ok(receipt);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteReceipt([FromBody] DeleteReceiptRequest request)
        {
            if (request?.Selected == null || !request.Selected.Any())
            {
                return BadRequest("No receipts selected for deletion.");
            }

            await _receiptService.DeleteReceiptsAsync(request.Selected);
            return Ok(new { status = "Ok" });
        }
    }
}
