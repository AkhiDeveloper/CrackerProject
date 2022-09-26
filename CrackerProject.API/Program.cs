using CrackerProject.API.Services;
using CrackerProject.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongodbSettings = builder.Configuration
    .GetSection(nameof(MongoDbSettings))
    .Get<MongoDbSettings>();

builder.Services.AddSingleton<MongoDbSettings>(mongodbSettings);

builder.Services.AddSingleton<IMongoClient>(
    s => new MongoClient(mongodbSettings.ConnectionString));

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
