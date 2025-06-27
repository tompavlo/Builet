using System.ComponentModel.DataAnnotations.Schema;

namespace Builet.Wallet;

[Table("wallet")]
public class Wallet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User.User User { get; set; } = null!;

    public decimal Balance { get; set; }
}