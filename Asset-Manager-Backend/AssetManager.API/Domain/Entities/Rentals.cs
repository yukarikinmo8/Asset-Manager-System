using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AssetManager.API.Domain.Entities;

public class Rentals
{
    [BsonId]
    [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("assetId"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string AssetId { get; set; } = string.Empty;

    [BsonElement("assetName"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string AssetName { get; set; } = string.Empty;

    [BsonElement("employeeId"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string EmployeeId { get; set; } = string.Empty;

    [BsonElement("employeeName"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string EmployeeName { get; set; } = string.Empty;

    [BsonElement("borrowedStart"), BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
    public DateTime BorrowedStart { get; set; }

    [BsonElement("borrowedEnd"), BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
    public DateTime BorrowedEnd { get; set; }

    [BsonElement("rentalStatus"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string RentalStatus { get; set; } = "Pending";

    [BsonElement("amount"), BsonRepresentation(MongoDB.Bson.BsonType.Double)]
    public double Amount { get; set; }

    [BsonElement("imageUrl"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string? ImageUrl { get; set; } = string.Empty;
}
