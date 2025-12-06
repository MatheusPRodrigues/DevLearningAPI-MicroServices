using DevLearning.StudentAPI.Data;
using DevLearning.StudentAPI.Repository;
using DevLearning.StudentAPI.Repository.Interfaces;
using DevLearning.StudentAPI.Services;
using DevLearning.StudentAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
builder.Services.AddSingleton<IStudentService, StudentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Matheus