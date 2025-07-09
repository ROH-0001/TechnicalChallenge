using System.ComponentModel.DataAnnotations;


namespace UserManagement.Shared.Forms;
public class UpdateUserDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Forename is required")]
    [StringLength(50, ErrorMessage = "Forename must be less than 50 characters")]
    public string Forename { get; set; } = string.Empty;

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(50, ErrorMessage = "Surname must be less than 50 characters")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email formatting")]
    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime? DateOfBirth { get; set; }
}

