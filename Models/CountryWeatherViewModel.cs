namespace GloboClimaAPI.Models
{
    public class CountryWeatherViewModel
    {
        public CountryData Country { get; set; }
        public WeatherData Weather { get; set; }
        public List<Favorite> Favorites { get; set; }

    }
}
