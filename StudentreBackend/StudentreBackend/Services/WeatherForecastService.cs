using AutoMapper;
using StudentreBackend.Data;
using StudentreBackend.Data.Extensions;

namespace StudentreBackend.Services
{
    public class WeatherForecastService : BaseService, IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastService(DefaultDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        public bool IsDbContext()
        {
            return Context.Database != null;
        }
    }
}
