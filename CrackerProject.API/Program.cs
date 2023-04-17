using CrackerProject.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.GenericRepository.UoW;
using AutoMapper.Extensions.ExpressionMapping;
using AutoMapper;
using CrackerProject.API.Data.MongoDb.SchemaOne.Repository;
using CrackerProject.API.Data.MongoDb;
using CrackerProject.API.Data.MongoDb.SchemaOne.Persistence;
using CrackerProject.API.Data.Firebase.Storage;
using CrackerProject.API.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongodbSettings = builder.Configuration
    .GetSection(nameof(MongoDbSettings))
    .Get<MongoDbSettings>();

builder.Services.AddSingleton<MongoDbSettings>(mongodbSettings);

var firebaseconfig=builder.Configuration
    .GetSection(nameof(FirebaseConfig).ToLower())
    .Get<FirebaseConfig>();
builder.Services.AddSingleton<FirebaseConfig>(firebaseconfig);

builder.Services.AddSingleton<IMongoClient>(
    s => new MongoClient(mongodbSettings.ConnectionString));

builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<IQuestionSetRepository, QuestionSetRepository>();
builder.Services.AddScoped<IStorageManager, FirebaseStorageManager>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

//Adding Automapper Service
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddExpressionMapping();
},
AppDomain.CurrentDomain.GetAssemblies());


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
