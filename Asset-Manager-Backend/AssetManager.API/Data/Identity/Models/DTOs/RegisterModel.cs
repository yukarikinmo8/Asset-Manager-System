using System.ComponentModel.DataAnnotations;

namespace AssetManager.API.Data.Identity.Models.DTOs;

public class RegisterModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    [RegularExpression(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email format without spaces."
    )]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(
        100,
        MinimumLength = 6,
        ErrorMessage = "Password must be at least 6 characters long"
    )]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(
        100,
        MinimumLength = 5,
        ErrorMessage = "Full name must be between 5 and 100 characters."
    )]
    public required string FullName { get; set; }
}
