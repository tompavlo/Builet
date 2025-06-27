using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Builet.Stock;

[Table("stock")]
public class Stock
{
    [Key] [Column("id")] public long Id { get; set; }

    [Required]
    [MaxLength(10)]
    [Column("ticker_symbol")]
    public string TickerSymbol { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("company_name")]
    public string CompanyName { get; set; } = null!;

    [MaxLength(50)] [Column("sector")] public string? Sector { get; set; }
}