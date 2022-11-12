namespace CurrentWeatherApi.OpenWeatherApi
{
    public interface IOpenWeatherApi
    {
        Task<string> GetOpenApiWeather(string CityAndCountryName, string ApiKey);
    }
}
