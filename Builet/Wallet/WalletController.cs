using Microsoft.AspNetCore.Mvc;

namespace Builet.Wallet;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly WalletService _walletService;

    public WalletController(WalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<WalletDto>> GetWallet(Guid userId)
    {
        try
        {
            var wallet = await _walletService.GetWalletByUserIdAsync(userId);
            return Ok(wallet);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{userId}/add")]
    public async Task<IActionResult> AddFunds(Guid userId, [FromQuery] decimal amount)
    {
        try
        {
            await _walletService.AddFundsSync(userId, amount);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{userId}/withdraw")]
    public async Task<IActionResult> WithdrawFunds(Guid userId, [FromQuery] decimal amount)
    {
        try
        {
            await _walletService.WithdrawFundsAsync(userId, amount);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}