using CurrentWeatherApi.OpenWeatherApi;
using Microsoft.AspNetCore.Mvc;

namespace CurrentWeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrentWeatherController : ControllerBase
    {
        private readonly ILogger<CurrentWeatherController> _logger;
        private IOpenWeatherApi _openWeatherApi;

        public CurrentWeatherController(IOpenWeatherApi openWeatherApi, ILogger<CurrentWeatherController> logger)
        {
            _openWeatherApi = openWeatherApi;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetWeather/{CityName}/{CountryName}")]
        public async Task<string> Get([FromRoute] string CityName, [FromRoute] string CountryName)
        {
            string result = await _openWeatherApi.GetOpenApiWeather(CityName, CountryName);

            return result;
        }
    }
}