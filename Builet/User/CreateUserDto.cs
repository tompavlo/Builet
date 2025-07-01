using System.ComponentModel.DataAnnotations;

namespace Builet.User;

public class CreateUserDto
{
    [Required]
    [StringLength(40, MinimumLength = 3)]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}