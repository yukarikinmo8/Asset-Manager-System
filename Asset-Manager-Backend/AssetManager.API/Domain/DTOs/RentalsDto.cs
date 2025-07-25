using System;

namespace AssetManager.API.Domain.DTOs;

public class RentalsDto
{
    public string AssetId { get; set; } = string.Empty;
    public string AssetName { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime BorrowedStart { get; set; }
    public DateTime BorrowedEnd { get; set; }
    public string RentalStatus { get; set; } = "Pending";
    public double Amount { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
}
