using CrackerProject.API.Context;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Persistence;
using CrackerProject.API.Repository;
using CrackerProject.API.Services;
using CrackerProject.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.GenericRepository.UoW;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongodbSettings = builder.Configuration
    .GetSection(nameof(MongoDbSettings))
    .Get<MongoDbSettings>();

builder.Services.AddSingleton<MongoDbSettings>(mongodbSettings);

builder.Services.AddSingleton<IMongoClient>(
    s => new MongoClient(mongodbSettings.ConnectionString));

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<ISubSectionRepository, SubSectionRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<IBookSectionRepository, BookSectionRepository>();
builder.Services.AddScoped<IQuestionSetRepository, QuestionSetRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

MongoDbPersistence.Configure();
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
