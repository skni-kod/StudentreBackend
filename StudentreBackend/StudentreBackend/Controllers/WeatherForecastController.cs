using Microsoft.AspNetCore.Mvc;
using StudentreBackend.Services;

namespace StudentreBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        // przyklad: servis dodajemy jako interfejs a nastepnie wstrzykujemy go i rejestrujemy w klasie program.cs
        private readonly IWeatherForecastService Service;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            _logger = logger;
            Service = service;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Service.Get();
        }
        [Route("connection")]
        [HttpGet]
        public bool IsConnection()
        {
            //return true;
            return Service.IsDbContext();
        }
    }
}