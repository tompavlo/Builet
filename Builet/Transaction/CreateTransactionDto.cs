using System.ComponentModel.DataAnnotations;

namespace Builet.Transaction;

public class CreateTransactionDto
{
    [Required(ErrorMessage = "SellerId is required.")]
    public Guid SellerId { get; set; }
    [Required(ErrorMessage = "StockId is required.")]
    public long StockId { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
    [Required]
    [Range(0.01, 1000000000.00)]
    public decimal PricePerUnit { get; set; }
}