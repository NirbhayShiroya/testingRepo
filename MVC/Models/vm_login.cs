using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Repositories;

public class vm_login
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [DisplayName("Employee Email")]
    public string Email { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Password is required.")]
    [DisplayName("Employee Password")]
    public string Password { get; set; }
}
