using System.ComponentModel.DataAnnotations.Schema;

namespace Builet.User;

[Table("user")]
public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } 
    public string PasswordHash { get; set; } 
    public string Email { get; set; } 
    public Role Role { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }
    
    public ICollection<Inventory.Inventory> Inventory { get; set; } = new List<Inventory.Inventory>();
    
    public ICollection<Transaction.Transaction> TransactionsAsSeller { get; set; } = new List<Transaction.Transaction>();
    
    public ICollection<Transaction.Transaction> TransactionsAsBuyer { get; set; } = new List<Transaction.Transaction>();
}