using GloboClimaAPI.Models;
using Newtonsoft.Json;

namespace GloboClimaAPI.Services
{
    public class CountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CountryData> GetCountryDataAsync(string country)
        {
            try
            {
                var encodedCountry = Uri.EscapeDataString(country);

                var response = await _httpClient.GetStringAsync($"https://restcountries.com/v3.1/name/{encodedCountry}");

                var countryDataArray = JsonConvert.DeserializeObject<CountryData[]>(response);
                var countryData = countryDataArray?.FirstOrDefault();

                if (countryData != null)
                {
                    string countryCode = countryData.Cca2?.ToLower();
                    if (!string.IsNullOrEmpty(countryCode))
                    {
                        countryData.Cca2 = countryCode;
                    }
                }

                return countryData;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro ao acessar a API de países: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao deserializar os dados: {ex.Message}");
                return null;
            }
        }
    }
}
