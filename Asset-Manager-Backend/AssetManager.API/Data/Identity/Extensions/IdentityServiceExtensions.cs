using AspNetCore.Identity.MongoDbCore.Models;
using AssetManager.API.Data.Identity.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace AssetManager.API.Data.Identity.Extensions
{
    public static class IdentityServiceExtensions
    {
        [Obsolete]
        public static IServiceCollection AddMongoIdentity(
            this IServiceCollection services,
            string mongoConnectionString,
            string mongoDatabaseName
        )
        {
            var settings = MongoClientSettings.FromConnectionString(mongoConnectionString);
            settings.GuidRepresentation = GuidRepresentation.Standard;
            var mongoClient = new MongoClient(settings);

            services.AddSingleton<IMongoClient>(mongoClient);

            var mongoIdentityContext = new MongoDbContext(mongoConnectionString, mongoDatabaseName);
            services.AddSingleton<IMongoDbContext>(mongoIdentityContext);

            services
                .AddIdentityCore<UserModel>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredLength = 6;
                })
                .AddRoles<MongoIdentityRole<Guid>>()
                .AddMongoDbStores<UserModel, MongoIdentityRole<Guid>, Guid>(mongoIdentityContext)
                .AddSignInManager()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
