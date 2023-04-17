using AutoMapper.Extensions.ExpressionMapping;
using BookManager.API.Data;
using BookManager.API.Mapper;
using BookManager.API.ServiceProvider;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddHttpContextAccessor(); // Register the IHttpContextAccessor interface

builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddExpressionMapping();
}, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IBookManager, DefaultBookManager>();
builder.Services.AddScoped<IChapterManager, DefaultChapterManager>();

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

app.UseStaticFiles();

//app.UseStaticFiles(new StaticFileOptions
//{
//    ContentTypeProvider = new FileExtensionContentTypeProvider
//    {
//        Mappings = { [".jpg"] = "image/jpeg", [".png"] = "image/png" }
//    }
//});

app.MapControllers();

app.Run();
