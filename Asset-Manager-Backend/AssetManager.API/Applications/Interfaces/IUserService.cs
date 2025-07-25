namespace AssetManager.API.Applications.Interfaces
{
    public interface IUserService
    {
        string UserId { get; }
        bool IsAuthenticated { get; }
    }
}
