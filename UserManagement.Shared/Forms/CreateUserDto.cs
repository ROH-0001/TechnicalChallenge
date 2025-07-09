using System.ComponentModel.DataAnnotations;
using UserManagement.Shared.ValidationAttributes;


namespace UserManagement.Shared.Forms;
public class CreateUserDto
{
    [Required(ErrorMessage = "Forename is required")]
    [StringLength(50, ErrorMessage = "Forename cannot exceed 50 characters")]
    public string Forename { get; set; } = string.Empty;

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Date Of Birth is required")]
    [DateOfBirth]
    public DateTime? DateOfBirth { get; set; }
}
