using System.ComponentModel.DataAnnotations;
using UserManagement.Shared.ValidationAttributes;


namespace UserManagement.Shared.Forms;
public class UpdateUserDto
{

    [Required(ErrorMessage = "Forename is required")]
    [StringLength(50, ErrorMessage = "Forename must be less than 50 characters")]
    public string Forename { get; set; } = string.Empty;

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(50, ErrorMessage = "Surname must be less than 50 characters")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    [EmailAddress(ErrorMessage = "Invalid email formatting")]
    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Date Of Birth is required")]
    [DateOfBirth]
    public DateTime? DateOfBirth { get; set; }
}

