using AssetManager.API.Domain.DTOs;
using AssetManager.API.Domain.Entities;

namespace AssetManager.API.Applications.Interfaces;

public interface IAssetService
{
    Task<List<Assets>> GetAllAssets(string? status = null);
    Task<List<string>> GetAllCategory();
    Task<List<Assets>> GetAssetsByCategory(string category);
    Task<Assets> GetAssetById(string id);
    Task<Assets> AddAsset(AssetDto asset);
    Task<Assets?> UpdateAsset(string id, AssetDto asset);

    /*
    TODO: Update asset to available or unavailable
    
    */
}
