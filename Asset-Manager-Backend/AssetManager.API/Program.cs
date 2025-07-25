using AssetManager.API.Applications.Interfaces; // Interface for asset service
using AssetManager.API.Applications.Services; // Implementation of asset service
using AssetManager.API.Data; // MongoDbService
using AssetManager.API.Data.Identity.Extensions; // Identity extension methods
using AssetManager.API.Data.Identity.Services; // Role seeder service

var builder = WebApplication.CreateBuilder(args); // Create a new web application builder

//* --- Service Configuration ---

// Retrieve MongoDB connection settings from configuration
var mongoConnectionString = builder.Configuration.GetConnectionString("DbConnection");
var mongoDatabaseName = builder.Configuration.GetConnectionString("DbName");

builder.Services.AddControllers(); // Add controllers for API endpoints
builder.Services.AddScoped<RoleSeederService>(); // Add role seeder for initial role setup
builder.Services.AddScoped<AssetSeederService>();
builder.Services.AddScoped<IAssetService, AssetService>(); // Register asset service for DI
builder.Services.AddScoped<IRentalService, RentalService>(); // Register rental service for DI
builder.Services.AddSingleton<MongoDbService>(); // Register MongoDbService as singleton

// Add Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS configuration
builder.Services.AddCorsService(builder.Configuration);

// Add MongoDB Identity (user management/authentication)
#pragma warning disable CS0612 // Type or member is obsolete
builder.Services.AddMongoIdentity(mongoConnectionString!, mongoDatabaseName!);
#pragma warning restore CS0612 // Type or member is obsolete

// Add JWT authentication and custom authorization policies
builder.Services.AddAuthService(builder.Configuration);
builder.Services.AddCustomPolicies();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();

//* --- Application Pipeline Configuration ---

var app = builder.Build(); // Build the application

// Seed roles and super admin user on startup
using (var scope = app.Services.CreateScope())
{
    var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeederService>();
    roleSeeder.SeedRolesAndSuperAdminAsync().Wait();

    var assetSeeder = scope.ServiceProvider.GetRequiredService<AssetSeederService>();
    assetSeeder.SeedAssetsAsync().Wait();
}

app.UseCors("MyCorsPolicy"); // Use the defined CORS policy

// Custom middleware Http response
app.UseCustomForbiddenResponse();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger in development
    app.UseSwaggerUI(); // Enable Swagger UI in development
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS

app.UseAuthentication();

app.UseAuthorization(); // Enable authorization middleware

app.MapControllers(); // Map controller routes

app.Run(); // Run the application
