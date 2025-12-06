using DevLearning.AuthorAPI.DataBase;
using DevLearning.AuthorAPI.Repositories;
using DevLearning.AuthorAPI.Repositories.Interfaces;
using DevLearning.AuthorAPI.Services;
using DevLearning.AuthorAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient("CourseAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5006");
});

builder.Services.AddSingleton<IAuthorService, AuthorService>();
builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();
builder.Services.AddSingleton<ConnectionDB>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
