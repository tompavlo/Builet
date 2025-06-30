namespace Builet.Inventory;

public class InventoryDto
{
    public long Id { get; set; }
    public Guid UserId { get; set; }

    public long StockId { get; set; }
    public string TickerSymbol { get; set; } = string.Empty;

    public int Quantity { get; set; }
}