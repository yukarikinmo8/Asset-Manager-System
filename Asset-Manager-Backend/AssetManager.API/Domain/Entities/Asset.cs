using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AssetManager.API.Domain.Entities;

public class Assets
{
    [BsonId]
    [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string? Name { get; set; }

    [BsonElement("description"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string? Description { get; set; }

    [BsonElement("amount"), BsonRepresentation(MongoDB.Bson.BsonType.Double)]
    public double Amount { get; set; }

    [BsonElement("status"), BsonRepresentation(MongoDB.Bson.BsonType.Boolean)]
    public bool Status { get; set; } = true; // Default status

    [BsonElement("category"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string? Category { get; set; } = string.Empty;

    [BsonElement("imageUrl"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string? ImageUrl { get; set; } = string.Empty;
}
