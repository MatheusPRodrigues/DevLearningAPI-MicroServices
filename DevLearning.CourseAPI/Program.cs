using DevLearning.CourseAPI.Data;
using DevLearning.CourseAPI.Repositories;
using DevLearning.CourseAPI.Repositories.Interfaces;
using DevLearning.CourseAPI.Services;
using DevLearning.CourseAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

BsonSerializer.RegisterSerializer(
    new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard)
);

builder.Services.AddControllers();

builder.Services.AddSingleton<ConnectionDB>();
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    var client = new MongoClient(settings.ConnectionURI);
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddSingleton<ICourseService, CourseService>();
builder.Services.AddSingleton<ICourseRepository, CourseRepository>();

builder.Services.AddHttpClient("AuthorAPI", c =>
{
    c.BaseAddress = new Uri("https://localhost:5001/api/v1/author/");
}).ConfigurePrimaryHttpMessageHandler(() =>
    new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

builder.Services.AddHttpClient("CategoryAPI", c =>
{
    c.BaseAddress = new Uri("https://localhost:5005/api/v1/category/");
}).ConfigurePrimaryHttpMessageHandler(() =>
    new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

builder.Services.AddHttpClient("StudentAPI", c =>
{
    c.BaseAddress = new Uri("https://localhost:5009/api/v1/student/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//Felipe