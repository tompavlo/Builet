namespace Builet.Transaction;

public class CreateTransactionDto
{
    public Guid SellerId { get; set; }

    public long StockId { get; set; }

    public int Quantity { get; set; }

    public decimal PricePerUnit { get; set; }
}