using GloboClimaAPI.Data;
using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GloboClimaAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly CountryService _countryService;
        private readonly WeatherService _weatherService;
        private readonly FavoritesService _favoriteService;

        public HomeController(CountryService countryService, WeatherService weatherService, FavoritesService favoriteService, CustomDynamoDBContext dbContext)
        {
            _countryService = countryService;
            _weatherService = weatherService;
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index(string? country = null, string? city = null)
        {
            var model = new CountryWeatherViewModel
            {
                Favorites = new List<Favorite>(),
                Country = null, 
                Weather = null  
            };

            if (!string.IsNullOrWhiteSpace(country))
            {
                var countryData = await _countryService.GetCountryDataAsync(country);
                if (countryData != null)
                {
                    model.Country = countryData;
                }
                else
                {
                    ViewBag.ErrorMessage = $"País '{country}' não encontrado.";
                }
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                try
                {
                    var weatherData = await _weatherService.GetWeatherAsync(city);
                    if (weatherData != null)
                    {
                        model.Weather = weatherData;                                                      
                        model.Weather.Forecast = model.Weather.Forecast ?? new List<ForecastData>();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Dados de clima para a cidade '{city}' não foram encontrados.";
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = $"Erro ao buscar clima para a cidade '{city}': {ex.Message}";
                }
            }

            if (User.Identity.IsAuthenticated)
            {
                model.Favorites = await _favoriteService.GetFavoritesAsync(User.Identity.Name) ?? new List<Favorite>();
            }

            return View(model); 
        }

        // Apenas para documentação Swagger
        [HttpGet("country/{countryName}")]
        public async Task<IActionResult> GetCountry(string countryName)
        {
            var countryData = await _countryService.GetCountryDataAsync(countryName);
            if (countryData != null)
            {
                return Ok(countryData);
            }
            return NotFound(new { Message = $"País '{countryName}' não encontrado." });
        }

        [HttpGet("weather/{cityName}")]
        public async Task<IActionResult> GetWeather(string cityName)
        {
            try
            {
                var weatherData = await _weatherService.GetWeatherAsync(cityName);
                if (weatherData != null)
                {
                    return Ok(weatherData);
                }
                return NotFound(new { Message = $"Dados de clima para a cidade '{cityName}' não foram encontrados." });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { Message = $"Erro ao buscar clima para a cidade '{cityName}': {ex.Message}" });
            }
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            if (User.Identity.IsAuthenticated)
            {
                var favorites = await _favoriteService.GetFavoritesAsync(User.Identity.Name);
                return Ok(favorites ?? new List<Favorite>());
            }
            return Unauthorized(new { Message = "Usuário não autenticado." });
        }
    }
}

