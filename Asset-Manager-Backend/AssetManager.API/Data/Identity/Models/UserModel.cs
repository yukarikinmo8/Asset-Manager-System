using AspNetCore.Identity.MongoDbCore.Models;

namespace AssetManager.API.Data.Identity.Models;

public class UserModel : MongoIdentityUser<Guid>
{
    public string? Fullname { get; set; }
}
