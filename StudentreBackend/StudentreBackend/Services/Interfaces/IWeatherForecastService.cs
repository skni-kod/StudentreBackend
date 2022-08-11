
using StudentreBackend.Data;

namespace StudentreBackend.Services
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get();
        public bool IsDbContext();
    }
}