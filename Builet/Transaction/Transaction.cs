using System.ComponentModel.DataAnnotations.Schema;

namespace Builet.Transaction;

[Table("transaction")]
public class Transaction
{
    public long Id { get; set; }

    public Guid SellerId { get; set; }
    public User.User Seller { get; set; } = null!;

    public Guid? BuyerId { get; set; }
    public User.User? Buyer { get; set; }

    public long StockId { get; set; }
    public Stock.Stock Stock { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }

    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}