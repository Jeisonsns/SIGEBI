using MongoDB.Driver;

namespace SIGEBI.Persistence.Context;

public class SIGEBIContext
{
    private readonly IMongoDatabase _database;

    public SIGEBIContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoDatabase Database => _database;
}
