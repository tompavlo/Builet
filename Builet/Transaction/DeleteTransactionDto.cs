using System.ComponentModel.DataAnnotations;
using Builet.User;

namespace Builet.Transaction;

public class DeleteTransactionRequestDto
{
    [Required(ErrorMessage = "You must provide the ID of the user performing this action.")]
    public Guid CurrentUserId { get; set; }

    [Required(ErrorMessage = "You must provide the role of the user performing this action.")]
    public Role CurrentUserRole { get; set; }
}