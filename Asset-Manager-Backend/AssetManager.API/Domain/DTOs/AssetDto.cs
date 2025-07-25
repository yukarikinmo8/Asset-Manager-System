using System;
using MongoDB.Bson;

namespace AssetManager.API.Domain.DTOs;

public class AssetDto
{
    // public ObjectId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool Status { get; set; } = true; // Default status
    public string ImageUrl { get; set; } = string.Empty;
}
