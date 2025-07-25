using AssetManager.API.Applications.Interfaces;
using AssetManager.API.Data;
using AssetManager.API.Data.Enums;
using AssetManager.API.Data.Identity.Models;
using AssetManager.API.Domain.Entities;
using MongoDB.Driver;

namespace AssetManager.API.Applications.Services
{
    public class RentalService : IRentalService
    {
        private readonly MongoDbService _context;

        public RentalService(MongoDbService context)
        {
            _context = context;
        }

        // This service will handle asset operations such as creating, updating, and deleting assets.
        // It will interact with the database through a repository or direct database access.
        public async Task<List<Rentals>> GetEmployeeRentals(
            string? rentalStatus = null,
            string? employeeId = null
        )
        {
            if (
                !string.IsNullOrEmpty(rentalStatus)
                && !Enum.TryParse<StatusEnum.RentalStatusEnum>(rentalStatus, true, out _)
            )
            {
                throw new ArgumentException(
                    $"Invalid rental status: {rentalStatus}. Valid values are: "
                        + string.Join(", ", Enum.GetNames(typeof(StatusEnum.RentalStatusEnum)))
                );
            }

            var collection = _context.Database.GetCollection<Rentals>("Rentals");
            FilterDefinition<Rentals> filter = Builders<Rentals>.Filter.Empty;

            List<Rentals> allRentals;
            if (!string.IsNullOrEmpty(rentalStatus))
            {
                // Use case-insensitive collation for RentalStatus
                var findOptions = new FindOptions
                {
                    Collation = new Collation("en", strength: CollationStrength.Secondary),
                };
                filter = Builders<Rentals>.Filter.Eq(r => r.RentalStatus, rentalStatus);
                allRentals = await collection.Find(filter, findOptions).ToListAsync();
            }
            else
            {
                allRentals = await collection.Find(filter).ToListAsync();
            }
            if (!string.IsNullOrEmpty(employeeId))
            {
                allRentals = allRentals
                    .Where(r => r.EmployeeId.Equals(employeeId, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return allRentals;
        }

        public async Task<Rentals?> RentAsset(
            string assetId,
            string employeeId,
            DateTime borrowedStart,
            DateTime borrowedEnd
        )
        {
            var assetCollection = _context.Database.GetCollection<Assets>("Assets");
            var asset = await assetCollection.Find(r => r.Id == assetId).FirstOrDefaultAsync();

            if (asset == null || asset.Status == false)
                return null; // Asset not found or already rented
            asset.Status = false; // Mark as rented
            await assetCollection.ReplaceOneAsync(a => a.Id == asset.Id, asset); // Update asset status in the database

            // Create a new Rentals object based on the asset and employeeId
            var rentalsCollection = _context.Database.GetCollection<Rentals>("Rentals");
            var employeeName = _context
                .Database.GetCollection<UserModel>("userModels")
                .Find(e => e.Id == Guid.Parse(employeeId))
                .FirstOrDefault()
                ?.Fullname;

            var newRental = new Rentals
            {
                AssetId = assetId,
                AssetName = asset.Name ?? string.Empty,
                EmployeeId = employeeId,
                EmployeeName = employeeName ?? string.Empty,
                BorrowedStart = borrowedStart,
                BorrowedEnd = borrowedEnd,
                RentalStatus = "Pending", // Initial status
                Amount = asset.Amount,
                ImageUrl = asset.ImageUrl, // Assuming asset has an ImageUrl property
            };

            await rentalsCollection.InsertOneAsync(newRental);

            return newRental;
        }

        public async Task<Rentals?> UpdateRental(string id, string managerStatus)
        {
            var rentalsCollection = _context.Database.GetCollection<Rentals>("Rentals");
            var rental = await rentalsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();

            // No rental found with the given ID
            if (rental == null)
                return null;

            // Check if the rental exists
            if (string.IsNullOrEmpty(managerStatus))
                throw new ArgumentNullException(
                    nameof(managerStatus),
                    "Manager status cannot be null or empty"
                );

            // Accept only managerStatus values that are defined in UpdateStatusEnum
            if (!Enum.TryParse<StatusEnum.UpdateStatusEnum>(managerStatus, true, out _))
                throw new ArgumentException(
                    $"Invalid manager status: {managerStatus}. Valid values are: "
                        + string.Join(", ", Enum.GetNames(typeof(StatusEnum.UpdateStatusEnum)))
                );

            var normalizedStatus = managerStatus.Trim().ToLowerInvariant();

            var assetCollection = _context.Database.GetCollection<Assets>("Assets");
            var asset =
                await assetCollection.Find(a => a.Id == rental.AssetId).FirstOrDefaultAsync()
                ?? throw new Exception("Asset not found for the rental.");

            switch (normalizedStatus)
            {
                case "approved":
                    rental.RentalStatus = "Confirmed";
                    // Update the asset status to rented
                    asset.Status = false; // Mark as rented
                    break;

                case "returned":
                    rental.RentalStatus = "Returned";
                    // Update the asset status to available
                    asset.Status = true; // Mark as available
                    break;

                case "pending":
                    rental.RentalStatus = "Pending";
                    break;

                default:
                    // Should not occur due to earlier validation
                    throw new ArgumentException($"Unhandled manager status: {managerStatus}");
            }

            // Update the asset status in the database
            await assetCollection.ReplaceOneAsync(a => a.Id == asset.Id, asset);
            // Update the rental in the database
            await rentalsCollection.ReplaceOneAsync(r => r.Id == id, rental);

            return rental;
        }
    }
}
