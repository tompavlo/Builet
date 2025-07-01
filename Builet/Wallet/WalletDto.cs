using System.ComponentModel.DataAnnotations;

namespace Builet.Wallet;

public class WalletDto
{
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative")]
    public decimal Balance { get; set; }
}