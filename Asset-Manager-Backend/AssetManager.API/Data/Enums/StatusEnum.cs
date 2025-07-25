using System;

namespace AssetManager.API.Data.Enums;

public class StatusEnum
{
    public enum RentalStatusEnum
    {
        Returned,
        Pending,
        Confirmed,
    }

    public enum UpdateStatusEnum
    {
        Approved,
        Returned,
        Pending,
    }
}
