﻿using System.ComponentModel.DataAnnotations;

namespace AssetManager.API.Data.Identity.Models.DTOs;

public class LoginModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(
        100,
        MinimumLength = 6,
        ErrorMessage = "Password must be at least 6 characters long"
    )]
    public required string Password { get; set; }
}
