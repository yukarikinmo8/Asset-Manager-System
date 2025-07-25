using AspNetCore.Identity.MongoDbCore.Models;
using AssetManager.API.Data.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace AssetManager.API.Data.Identity.Services;

public class RoleSeederService
{
    private readonly RoleManager<MongoIdentityRole<Guid>> _roleManager;
    private readonly UserManager<UserModel> _userManager;

    public RoleSeederService(
        RoleManager<MongoIdentityRole<Guid>> roleManager,
        UserManager<UserModel> userManager
    )
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedRolesAndSuperAdminAsync()
    {
        // Seed roles
        if (!await _roleManager.RoleExistsAsync("AssetManager"))
        {
            await _roleManager.CreateAsync(new MongoIdentityRole<Guid>("AssetManager"));
        }

        if (!await _roleManager.RoleExistsAsync("Employee"))
        {
            await _roleManager.CreateAsync(new MongoIdentityRole<Guid>("Employee"));
        }

        // Seed default superadmin user
        var defaultAdminEmail = "superadmin@gmail.com";
        var existingUser = await _userManager.FindByEmailAsync(defaultAdminEmail);

        if (existingUser == null)
        {
            var superAdminUser = new UserModel
            {
                UserName = defaultAdminEmail,
                Email = defaultAdminEmail,
                Fullname = "asset manager superadmin",
                EmailConfirmed = true,
            };

            var createResult = await _userManager.CreateAsync(superAdminUser, "Pass123$");

            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(superAdminUser, "AssetManager");
            }
            else
            {
                throw new Exception(
                    $"Failed to create superadmin user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}"
                );
            }
        }
    }
}
