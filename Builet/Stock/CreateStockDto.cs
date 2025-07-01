using System.ComponentModel.DataAnnotations;

namespace Builet.Stock;

public class CreateStockDto
{
    [Required]
    [StringLength(5, MinimumLength = 1, ErrorMessage = "Ticker symbol must be between 1 and 5 characters.")]
    public string TickerSymbol { get; set; } = null!;
    
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Company name must be between 3 and 100 characters.")]
    public string CompanyName { get; set; } = null!;
    
    public string? Field { get; set; }
}