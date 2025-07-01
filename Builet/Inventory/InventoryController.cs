using Microsoft.AspNetCore.Mvc;

namespace Builet.Inventory;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public InventoryController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyInventory()
    {
    
        if (!Request.Headers.TryGetValue("X-User-Id", out var userIdString) ||
            !Guid.TryParse(userIdString, out var userId))
        {
          
            return BadRequest("For testing without auth, provide a valid 'X-User-Id' header.");
        }

        return await GetInventoryByUserId(userId);
    }
    
    [HttpGet("/api/users/{userId:guid}/inventory")]
    public async Task<IActionResult> GetInventoryByUserId(Guid userId)
    {
        try
        {
            var inventory = await _inventoryService.GetAllStocksOfUserAsync(userId);
            return Ok(inventory);
        }
        catch (Exception ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}