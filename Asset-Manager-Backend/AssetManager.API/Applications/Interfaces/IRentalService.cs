using System;
using AssetManager.API.Domain.DTOs;
using AssetManager.API.Domain.Entities;

namespace AssetManager.API.Applications.Interfaces;

public interface IRentalService
{
    Task<List<Rentals>> GetEmployeeRentals(string? rentalStatus = null, string? employeeId = null);

    // Task<List<Rentals>> GetManagerRentals(string? managerStatus = null);
    Task<Rentals?> RentAsset(
        string assetId,
        string employeeId,
        DateTime borrowedStart,
        DateTime borrowedEnd
    );
    Task<Rentals?> UpdateRental(string id, string status);

    /*
    TODO: Add rental
    TODO: Update rental
    TODO: Delete rental
    */
}
