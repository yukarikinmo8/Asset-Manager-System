using AssetManager.API.Applications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Presentations
{
    [Route("api/v1/rentals/")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        private readonly IUserService _userService;

        public RentalController(IRentalService rentalService, IUserService userService)
        {
            _rentalService = rentalService;
            _userService = userService;
        }

        [HttpGet("get-employee-rentals")]
        [Authorize] // Ensure that the user is authenticated
        public async Task<IActionResult> GetEmployeeRentals(
            [FromQuery] string? rentalStatus = null,
            [FromQuery] string? employeeId = null
        )
        {
            try
            {
                var rentals = await _rentalService.GetEmployeeRentals(rentalStatus, employeeId);
                return Ok(
                    new { message = "Employee rentals retrieved successfully.", data = rentals }
                );
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpPost("rent-asset")]
        [Authorize] // Ensure that the user is authenticated
        public async Task<IActionResult> RentAsset(
            [FromQuery] string assetId,
            [FromQuery] DateTime borrowedStart,
            [FromQuery] DateTime borrowedEnd
        )
        {
            if (string.IsNullOrWhiteSpace(assetId))
                return BadRequest("Asset ID is required.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (borrowedStart == default || borrowedEnd == default)
                return BadRequest(new { message = "Borrowed start and end dates are required." });

            if (borrowedStart > borrowedEnd)
                return BadRequest(new { message = "Start date cannot be after end date." });

            if (
                DateOnly.FromDateTime(borrowedStart) < today
                || DateOnly.FromDateTime(borrowedEnd) < today
            )
                return BadRequest(new { message = "Dates cannot be in the past." });

            try
            {
                var employeeId = _userService.UserId;
                var rental = await _rentalService.RentAsset(
                    assetId,
                    employeeId,
                    borrowedStart,
                    borrowedEnd
                );
                if (rental == null)
                {
                    return NotFound(new { message = "Asset not found or already rented." });
                }

                return Ok(new { message = "Asset rented successfully.", data = rental });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpPut("update-rental")]
        [Authorize(Policy = "AssetManagerOnly")] // Ensure that the user is authenticated
        public async Task<IActionResult> UpdateRental(
            [FromQuery] string id,
            [FromQuery] string status
        )
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(status))
            {
                return BadRequest("Rental ID and status are required.");
            }

            try
            {
                var updatedRental = await _rentalService.UpdateRental(id, status);
                if (updatedRental == null)
                {
                    return NotFound(new { message = "Rental not found or no update made." });
                }

                return Ok(new { message = "Rental updated successfully.", data = updatedRental });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }
    }
}
