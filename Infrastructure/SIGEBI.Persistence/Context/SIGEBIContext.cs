using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace SIGEBI.Persistence.Context;

public class SIGEBIContext
{
    private readonly IMongoDatabase _database;

    static SIGEBIContext()
    {
        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(BsonType.String)
        };
        ConventionRegistry.Register("SIGEBIConventions", pack, _ => true);

        if (!BsonClassMap.IsClassMapRegistered(typeof(DateTime)))
        {
            BsonSerializer.RegisterSerializer(typeof(DateTime),
                new DateTimeSerializer(DateTimeKind.Utc));
        }
    }

    public SIGEBIContext(string connectionString, string databaseName)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.MaxConnectionPoolSize = 100;
        settings.MinConnectionPoolSize = 5;
        settings.ConnectTimeout = TimeSpan.FromSeconds(10);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
        settings.SocketTimeout = TimeSpan.FromSeconds(30);

        var client = new MongoClient(settings);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoDatabase Database => _database;
}
