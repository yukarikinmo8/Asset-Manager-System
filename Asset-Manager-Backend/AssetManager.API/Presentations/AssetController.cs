using AssetManager.API.Applications.Interfaces;
using AssetManager.API.Applications.Services;
using AssetManager.API.Domain.DTOs;
using AssetManager.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Presentations
{
    [Route("api/v1/assets/")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        // Inject AssetService for direct use (if needed)

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet("get-assets")]
        [Authorize] // Ensure that the user is authenticated
        public async Task<IActionResult> GetAllAssets([FromQuery] string? status = null)
        {
            try
            {
                var assets = await _assetService.GetAllAssets(status);
                var categories = await _assetService.GetAllCategory();

                return Ok(
                    new
                    {
                        message = "Assets retrieved successfully.",
                        data = assets,
                        category = categories,
                    }
                );
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpGet("get-by-category")]
        [Authorize] // Ensure that the user is authenticated
        public async Task<IActionResult> GetAssetsByCategory([FromQuery] string category)
        {
            try
            {
                var assets = await _assetService.GetAssetsByCategory(category);
                return Ok(new { message = "Assets retrieved successfully.", data = assets });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpGet("get-by-id")]
        [Authorize] // Ensure that the user is authenticated
        public async Task<IActionResult> GetAssetById([FromQuery] string id)
        {
            try
            {
                var asset = await _assetService.GetAssetById(id);
                return asset == null
                    ? NotFound(new { message = $"Asset with ID '{id}' not found." })
                    : Ok(new { message = "Asset retrieved successfully.", data = asset });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        // this endpoint is hidden will be used for developers only
        [HttpPost("add-asset")]
        [Authorize(Policy = "AssetManagerOnly")] // Only allow AssetManager (Manager) role
        public async Task<IActionResult> AddAsset([FromBody] AssetDto asset)
        {
            try
            {
                var addedAsset = await _assetService!.AddAsset(asset);
                return Ok(new { message = "Asset added successfully.", data = addedAsset });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpPut("update-asset")]
        [Authorize]
        public async Task<IActionResult> UpdateAsset(
            [FromQuery] string id,
            [FromBody] AssetDto asset
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "Asset ID cannot be null or empty." });
            }

            try
            {
                var updatedAsset = await _assetService.UpdateAsset(id, asset);
                return updatedAsset == null
                    ? NotFound(new { message = $"Asset with ID '{id}' not found." })
                    : Ok(new { message = "Asset updated successfully.", data = updatedAsset });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }
    }
}
