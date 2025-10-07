using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Repositories;

public class EmployeeModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DisplayName("Employee Id")]
    public int Eid { get; set; }

    [Required(ErrorMessage = "Employee name is required")]
    [StringLength(50, ErrorMessage = "Employee Name not Exceed to 50 character")]
    [DisplayName("Employee Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Employee email is required")]
    [StringLength(50, ErrorMessage = "Employee email not Exceed to 50 character")]
    [DisplayName("Employee Email")]
    public string Email { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Employee password is required")]
    [MinLength(6, ErrorMessage = "Employee password must be at least 6 character long")]
    [DisplayName("Employee Password")]
    public string Password { get; set; }


    [StringLength(50)]
    [Required(ErrorMessage = "Employee confirm password is required")]
    [MinLength(6, ErrorMessage = "Employee confirm password must be at least 6 character long")]
    [Compare("Password", ErrorMessage = "Password do not match")]
    [DisplayName("Employee confirm Password")]
    public string ConfirmPassword { get; set; }

    [StringLength(255, ErrorMessage = "Employee profile image size not Exceed to 255")]
    [DisplayName("Employee Profile Image")]
    public string? ProfileImage { get; set; }

    public IFormFile? ProfileImageFile { get; set; }

}
