using DevLearning.CareerAPI.Data;
using DevLearning.CareerAPI.Repository;
using DevLearning.CareerAPI.Repository.Interface;
using DevLearning.CareerAPI.Service;
using DevLearning.CareerAPI.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddSingleton<ICareerRepository, CareerRepository>();
builder.Services.AddSingleton<ICareerItemRepository, CareerItemRepository>();


builder.Services.AddHttpClient<ICareerItemService, CareerItemService>(client => client.BaseAddress = new Uri("https://localhost:5007/api/v1/Course/"));
builder.Services.AddHttpClient<ICareerService, CareerService>(client => client.BaseAddress = new Uri("https://localhost:5007/api/v1/Course/"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

