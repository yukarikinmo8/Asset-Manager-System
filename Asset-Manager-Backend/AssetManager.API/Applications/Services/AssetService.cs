using AssetManager.API.Applications.Interfaces;
using AssetManager.API.Data;
using AssetManager.API.Data.Enums;
using AssetManager.API.Domain.DTOs;
using AssetManager.API.Domain.Entities;
using MongoDB.Driver;

namespace AssetManager.API.Applications.Services
{
    public class AssetService : IAssetService
    {
        private readonly MongoDbService _context;

        public AssetService(MongoDbService context)
        {
            _context = context;
        }

        // This service will handle asset operations such as creating, updating, and deleting assets.
        // It will interact with the database through a repository or direct database access.

        public async Task<List<Assets>> GetAllAssets(string? status = null)
        {
            var collection = _context.Database.GetCollection<Assets>("Assets");
            FilterDefinition<Assets> filter = Builders<Assets>.Filter.Empty;

            if (
                !string.IsNullOrEmpty(status)
                && !status.Equals("all", StringComparison.OrdinalIgnoreCase)
            )
            {
                if (status.Equals("available", StringComparison.OrdinalIgnoreCase))
                    filter = Builders<Assets>.Filter.Eq(a => a.Status, true);
                else if (status.Equals("borrowed", StringComparison.OrdinalIgnoreCase))
                    filter = Builders<Assets>.Filter.Eq(a => a.Status, false);
            }

            return await collection.Find(filter).ToListAsync();
        }

        public async Task<List<string>> GetAllCategory()
        {
            // This method should return a list of assets grouped by category.
            // For now, we will return an empty list as a placeholder.

            var allCategory = await Task.Run(() =>
                Enum.GetValues(typeof(AssetCategory.CategoryEnum))
                    .Cast<AssetCategory.CategoryEnum>()
                    .ToList()
            );
            List<string> allCategoryStrings = allCategory.Select(c => c.ToString()).ToList();

            return allCategoryStrings;
        }

        public async Task<List<Assets>> GetAssetsByCategory(string category)
        {
            // This method should return a list of assets filtered by the specified category.
            // For now, we will return an empty list as a placeholder.

            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException(
                    nameof(category),
                    "Category cannot be null or empty"
                );
            }

            var collection = _context.Database.GetCollection<Assets>("Assets");
            var filter = Builders<Assets>.Filter.Eq(a => a.Category, category);
            var assetsByCategory = await collection.Find(filter).ToListAsync();

            return assetsByCategory;
        }

        public async Task<Assets> GetAssetById(string id)
        {
            // This method should return an asset by its ID.
            // For now, we will return null as a placeholder if the asset is not found.

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Asset ID cannot be null or empty");
            }

            var collection = _context.Database.GetCollection<Assets>("Assets");
            var filter = Builders<Assets>.Filter.Eq(a => a.Id, id);
            var asset = await collection.Find(filter).FirstOrDefaultAsync();

            return asset; // Return the found asset or null if not found
        }

        public async Task<Assets?> UpdateAsset(string id, AssetDto asset)
        {
            // This method should update an existing asset in the database.
            // For now, we will return a new Asset object as a placeholder.

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Asset ID cannot be null or empty");
            }

            // Fetch the existing asset from the database
            var collection = _context.Database.GetCollection<Assets>("Assets");
            var filter = Builders<Assets>.Filter.Eq(a => a.Id, id);
            var existingAsset = await collection.Find(filter).FirstOrDefaultAsync();
            if (existingAsset == null)
            {
                return null;
            }

            // Update only mutable fields (do NOT update _id/Id)
            existingAsset.Status = asset.Status;
            // Add other property mappings as needed

            await collection.ReplaceOneAsync(filter, existingAsset);

            return existingAsset; // Return the updated asset entity
        }

        public async Task<Assets> AddAsset(AssetDto asset)
        {
            // This method should add a new asset to the database.

            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset), "Asset cannot be null");
            }

            // Map AssetDto to Asset entity
            var assetEntity = new Assets
            {
                // Map properties from asset (AssetDto) to assetEntity (Asset)
                // Example:
                // Id = asset.Id,
                Name = asset.Name,
                Description = asset.Description,
                Amount = asset.Amount,
                Status = asset.Status,
                ImageUrl = asset.ImageUrl,
                Category = asset.Category,
                // Add other property mappings as needed
            };

            await _context.Database.GetCollection<Assets>("Assets").InsertOneAsync(assetEntity);

            return assetEntity; // Return the added asset entity
        }
    }
}
