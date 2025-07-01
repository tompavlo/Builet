using Builet.BaseRepository;
using Microsoft.AspNetCore.Mvc;

namespace Builet.Stock;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly StockService _stockService;

    public StockController(StockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetStockById(long id)
    {
        try
        {
            var stock = await _stockService.FindById(id);
            return Ok(stock);
        }
        catch (Exception ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockDto dto)
    {
        try
        {
            var newStock = await _stockService.CreateStockAsync(dto);

            return CreatedAtAction(nameof(GetStockById), new { id = newStock.Id }, newStock);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpDelete("{stockId:long}")]
    public async Task<IActionResult> DeleteStock(long stockId)
    {
        try
        {
            await _stockService.DeleteStockAsync(stockId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllStocks([FromQuery] SortingQuery query)
    {
        try
        {
            var stocks = await _stockService.GetAllStocksAsync(query);
            return Ok(stocks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An error occurred while fetching stocks." });
        }
    }
}