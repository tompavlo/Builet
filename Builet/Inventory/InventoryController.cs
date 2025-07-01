using Builet.BaseRepository;
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
    public async Task<IActionResult> GetMyInventory([FromQuery] PaginationQuery query)
    {
    
        if (!Request.Headers.TryGetValue("X-User-Id", out var userIdString) ||
            !Guid.TryParse(userIdString, out var userId))
        {
          
            return BadRequest("For testing without auth, provide a valid 'X-User-Id' header.");
        }

        return await GetInventoryByUserId(userId, query);
    }
    
    [HttpGet("users/{userId:guid}/inventory")] 
    public async Task<IActionResult> GetInventoryByUserId(Guid userId, [FromQuery] PaginationQuery query) // <-- 4. Accept pagination query
    {
        try
        {
            var pagedInventory = await _inventoryService.GetAllStocksOfUserAsync(userId, query);
            return Ok(pagedInventory);
        }
        catch (Exception ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}