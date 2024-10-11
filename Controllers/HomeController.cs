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
                Favorites = new List<Favorite>() 
            };

            if (!string.IsNullOrWhiteSpace(country))
            {
                model.Country = await _countryService.GetCountryDataAsync(country);
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                model.Weather = await _weatherService.GetWeatherAsync(city);
            }

            if (User.Identity.IsAuthenticated)
            {
                model.Favorites = await _favoriteService.GetFavoritesAsync(User.Identity.Name) ?? new List<Favorite>();
            }

            return View(model);
        }
    }
}

