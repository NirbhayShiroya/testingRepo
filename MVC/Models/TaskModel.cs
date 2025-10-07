using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories;

public class TaskModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DisplayName("Task Id")]
    public int Tid { get; set; }

    [Required(ErrorMessage = "Employee Id is required")]
    [DisplayName("Employee Id")]
    public int Eid { get; set; }

    [Required(ErrorMessage = "Task title is required")]
    [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
    [DisplayName("Task Title")]
    public string Title { get; set; }

    [DisplayName("Task Description")]
    public string Description { get; set; }

    [DisplayName("Estimated Days")]
    public int? EstimatedDays { get; set; }

    [DisplayName("Start Date")]
    public DateTime? StartDate { get; set; }

    [DisplayName("End Date")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
    [DisplayName("Task Status")]
    public string Status { get; set; }

    // Navigation property for the relationship with EmployeeModel
    [ForeignKey("Eid")]
    public EmployeeModel Employee { get; set; }
}
