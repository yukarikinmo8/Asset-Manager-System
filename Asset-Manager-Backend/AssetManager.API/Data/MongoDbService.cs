using MongoDB.Driver;

namespace AssetManager.API.Data;

public class MongoDbService
{
    private readonly IConfiguration _configuration;

    private readonly IMongoDatabase? _database;

    public MongoDbService(IConfiguration configuration)
    {
        _configuration = configuration;

        var connectionString = _configuration.GetConnectionString("DbConnection");
        var dbName = _configuration.GetConnectionString("DbName");
        var mongoUrl = MongoUrl.Create(connectionString);
        var mongoClient = new MongoClient(mongoUrl);
        _database = mongoClient.GetDatabase(dbName);
    }

    public IMongoDatabase Database =>
        _database ?? throw new InvalidOperationException("Database is not initialized.");
}
