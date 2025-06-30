namespace Builet.Stock;

public class CreateStockDto
{
    public string TickerSymbol { get; set; } = null!;
    
    public string CompanyName { get; set; } = null!;
    
    public string? Field { get; set; }
}