using DevLearning.CategoryAPI.DataBase;
using DevLearning.CategoryAPI.Repositories;
using DevLearning.CategoryAPI.Repositories.Interfaces;
using DevLearning.CategoryAPI.Services;
using DevLearning.CategoryAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient("CourseAPI", c =>
{
    c.BaseAddress = new Uri("https://localhost:5007/api/v1/course/");
}).ConfigurePrimaryHttpMessageHandler(() =>
    new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<ConnectionDB>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
//