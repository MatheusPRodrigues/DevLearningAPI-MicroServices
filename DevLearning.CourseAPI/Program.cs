using DevLearning.CourseAPI.Data;
using DevLearning.CourseAPI.Repositories;
using DevLearning.CourseAPI.Repositories.Interfaces;
using DevLearning.CourseAPI.Services;
using DevLearning.CourseAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// Register MongoDB client and database so IMongoDatabase can be resolved
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionURI);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Keep ConnectionDB if other code depends on it
builder.Services.AddSingleton<ConnectionDB>();

// Use scoped lifetimes for repository/service (safer for per-request operations)
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddHttpClient<ICourseService, CourseService>(client => client.BaseAddress = new Uri("https://localhost:5005/api/v1/Category"));
builder.Services.AddHttpClient<ICourseService, CourseService>(client => client.BaseAddress = new Uri("https://localhost:5001/api/v1/Author"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();