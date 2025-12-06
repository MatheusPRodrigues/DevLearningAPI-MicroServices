using DevLearning.CourseAPI.Data;
using DevLearning.CourseAPI.Repositories;
using DevLearning.CourseAPI.Repositories.Interfaces;
using DevLearning.CourseAPI.Services;
using DevLearning.CourseAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<ConnectionDB>();
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSingleton<ICourseService, CourseService>();
builder.Services.AddSingleton<ICourseRepository, CourseRepository>();

builder.Services.AddHttpClient<ICourseService, CourseService>(client => client.BaseAddress = new Uri("https://localhost:5005/api/v1/Category"));
builder.Services.AddHttpClient<ICourseService, CourseService>(client => client.BaseAddress = new Uri("https://localhost:5001/api/v1/Author"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//Felipe
//CopyFelipeBranch