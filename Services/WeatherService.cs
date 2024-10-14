using GloboClimaAPI.Models;
using Newtonsoft.Json;

namespace GloboClimaAPI.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;
        private readonly string _apiKey = "28a5ce958ab82e966e62fe53a99d2a92";

        // Dicionário de traduções
        private readonly Dictionary<string, string> translations = new Dictionary<string, string>
        {
            { "clear sky", "céu limpo" },
            { "few clouds", "poucas nuvens" },
            { "scattered clouds", "nuvens esparsas" },
            { "broken clouds", "nuvens fragmentadas" },
            { "shower rain", "chuva rápida" },
            { "rain", "chuva" },
            { "thunderstorm", "tempestade" },
            { "snow", "neve" },
            { "mist", "neblina" },
            { "fog", "nevoeiro" },
            { "drizzle", "chuvisco" },
            { "light rain", "chuva leve" },
            { "heavy intensity rain", "chuva intensa" },
            { "light snow", "neve leve" },
            { "heavy snow", "neve forte" },
            { "sleet", "chuva congelante" },
            { "dust", "poeira" },
            { "sand", "areia" },
            { "ash", "cinzas" },
            { "tornado", "tornado" },
            { "hurricane", "furacão" },
            { "cold", "frio" },
            { "hot", "quente" },
            { "windy", "ventoso" }
        };

        public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Método para obter o clima atual
        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Falha ao buscar dados de clima para a cidade '{city}'. Status Code: {response.StatusCode}");
                throw new HttpRequestException($"Não foi possível obter os dados de clima para a cidade '{city}'. Status Code: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

            if (apiResponse == null || apiResponse.weather == null || apiResponse.main == null || apiResponse.wind == null)
            {
                throw new Exception($"Não foram encontrados dados climáticos para a cidade '{city}'.");
            }

            var description = (string)apiResponse.weather[0].description;
            var translatedDescription = translations.ContainsKey(description) ? translations[description] : description;

            var iconCode = (string)apiResponse.weather[0].icon;
            var imageUrl = $"http://openweathermap.org/img/wn/{iconCode}@2x.png";

            return new WeatherData
            {
                City = (string)apiResponse.name,
                Description = translatedDescription,
                Temperature = (double)apiResponse.main.temp,
                Humidity = (double)apiResponse.main.humidity,
                WindSpeed = (double)apiResponse.wind.speed,
                Icon = iconCode,
                ImageUrl = imageUrl,
                MinTemperature = (double)apiResponse.main.temp_min,
                MaxTemperature = (double)apiResponse.main.temp_max,
                Pressure = (double)apiResponse.main.pressure,
                Forecast = await GetForecastAsync(city) 
            };
        }

        public async Task<List<ForecastData>> GetForecastAsync(string city)
        {
            var forecastUrl = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={_apiKey}&units=metric";
            var forecastResponse = await _httpClient.GetAsync(forecastUrl);

            if (!forecastResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Falha ao buscar previsão do tempo para a cidade '{city}'. Status Code: {forecastResponse.StatusCode}");
                throw new HttpRequestException($"Não foi possível obter a previsão do tempo para a cidade '{city}'. Status Code: {forecastResponse.StatusCode}");
            }

            var forecastContent = await forecastResponse.Content.ReadAsStringAsync();
            var forecastApiResponse = JsonConvert.DeserializeObject<dynamic>(forecastContent);

            var forecastList = new List<ForecastData>();

            foreach (var forecast in forecastApiResponse.list)
            {
                forecastList.Add(new ForecastData
                {
                    Date = DateTime.Parse((string)forecast.dt_txt),
                    MinTemperature = (double)forecast.main.temp_min,
                    MaxTemperature = (double)forecast.main.temp_max,
                    Description = translations.ContainsKey((string)forecast.weather[0].description)
                        ? translations[(string)forecast.weather[0].description]
                        : (string)forecast.weather[0].description,
                    Icon = (string)forecast.weather[0].icon 

                });
            }

            return forecastList;
        }
    }
}