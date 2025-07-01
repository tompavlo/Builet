using Microsoft.AspNetCore.Mvc;

namespace Builet.Transaction;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        try
        {
            var createdTransaction = await _transactionService.CreateTransactionAsync(dto);
            return Ok(createdTransaction);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTransactionsByStatus([FromQuery] TransactionStatus status)
    {
        var transactions = await _transactionService.ShowTypeOfTransactionAsync(status);
        return Ok(transactions);
    }
    
    [HttpPost("buy")]
    public async Task<IActionResult> BuyStockFromTransaction([FromBody] TransactionDto dto)
    {
        try
        {
            var updatedTransaction = await _transactionService.BuyTransaction(dto);
            return Ok(updatedTransaction);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpDelete("{transactionId}")]
    public async Task<IActionResult> DeleteTransaction(long transactionId,
        [FromBody] DeleteTransactionRequestDto request)
    {
        try
        {
            await _transactionService.DeleteTransactionAsync(transactionId, request.CurrentUserId,
                request.CurrentUserRole);
            return NoContent();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("permission"))
            {
                return StatusCode(403, new { error = ex.Message });
            }

            if (ex.Message.Contains("not found"))
            {
                return NotFound(new { error = ex.Message }); 
            }

            return BadRequest(new { error = ex.Message });
        }
    }
}