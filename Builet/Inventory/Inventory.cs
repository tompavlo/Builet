namespace Builet.Inventory;

public class Inventory
{
    public long Id { get; set; }

    public Guid UserId { get; set; }
    public User.User User { get; set; } = null!;

    public long StockId { get; set; }
    public Stock.Stock Stock { get; set; } = null!;

    public int Quantity { get; set; }
}