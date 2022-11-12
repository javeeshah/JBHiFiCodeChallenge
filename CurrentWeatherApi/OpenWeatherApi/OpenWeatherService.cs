using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text.Json;

namespace CurrentWeatherApi.OpenWeatherApi
{
    public class OpenWeatherService: IOpenWeatherApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public OpenWeatherService(IConfiguration configuration, IHttpClientFactory HttpClientFactory)
        {
            this._configuration = configuration;
            this._httpClientFactory = HttpClientFactory;
        }

        public async Task<string> GetOpenApiWeather(string CityName, string CountryName)
        {
            if (string.IsNullOrEmpty(CityName) || string.IsNullOrEmpty(CountryName))
            {
                return "City & Country names are mandatory";
            }

            var ApiURL = _configuration["ApiUrl"];

            var query = new Dictionary<string, string>()
            {
                ["q"] = CityName + "," + CountryName,
                ["appid"] = "8b7535b42fe1c551f18028f64e8688f7"
            };

            var uri = QueryHelpers.AddQueryString(ApiURL, query);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            
            requestMessage.Headers.Add("Accept", "application/json");

            var client = _httpClientFactory.CreateClient();

            try
            {
                var responseMessage = await client.SendAsync(requestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var contents =
                       await responseMessage.Content.ReadAsStringAsync();

                    using JsonDocument doc = JsonDocument.Parse(contents);
                    JsonElement root = doc.RootElement;
                    JsonElement weatherArray = root.GetProperty("weather");

                    if (weatherArray[0].TryGetProperty("description", out JsonElement value))
                    {
                        return value.ToString();
                    }
                    else
                    {
                        return "No weather description found for this city and country";
                    }                    
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var contents =
                       await responseMessage.Content.ReadAsStringAsync();

                    using JsonDocument doc = JsonDocument.Parse(contents);
                    JsonElement root = doc.RootElement;

                    return string.Format( "Error Response Message: {0}", root.GetProperty("message").ToString());
                }
                else
                {
                    return responseMessage.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                return string.Format("Error calling API!! Please check if API or it's endpoint is up and listening to requests. Error message: {0}", ex.Message);
            }
        }        
    }
}
