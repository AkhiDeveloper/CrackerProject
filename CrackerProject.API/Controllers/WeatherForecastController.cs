using CrackerProject.API.Models;
using CrackerProject.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrackerProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBookService _bookservice;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IBookService bookservice)
        {
            _logger = logger;
            _bookservice = bookservice;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _bookservice.Create(new Book()
            {
                Description = "hello",
                CreatedDateTime = DateTime.UtcNow
            }) ;

            var books = _bookservice.Get();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}