using MongoDB.Driver;
using SIGEBI.IOC.Dependencies;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// MongoDB
var connectionString = builder.Configuration.GetValue<string>("MongoDB:ConnectionString")
    ?? throw new InvalidOperationException("MongoDB:ConnectionString no está configurado.");
var databaseName = builder.Configuration.GetValue<string>("MongoDB:DatabaseName")
    ?? throw new InvalidOperationException("MongoDB:DatabaseName no está configurado.");

var mongoSettings = MongoClientSettings.FromConnectionString(connectionString);
mongoSettings.MaxConnectionPoolSize = 100;
mongoSettings.MinConnectionPoolSize = 5;
mongoSettings.ConnectTimeout = TimeSpan.FromSeconds(10);
mongoSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
mongoSettings.SocketTimeout = TimeSpan.FromSeconds(30);

var mongoClient = new MongoClient(mongoSettings);
var mongoDatabase = mongoClient.GetDatabase(databaseName);
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

// Dependencias SIGEBI
builder.Services.AddSIGEBIDependencies();

// Controladores con soporte de enums como string
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SIGEBI API", Version = "v1" });
});

var app = builder.Build();

// Manejo global de errores
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            error = "Ocurrió un error interno en el servidor. Intenta nuevamente."
        });
    });
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIGEBI API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();