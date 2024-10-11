namespace GloboClimaAPI.Models
{
    public class CountryData
    {
        public Name Name { get; set; }
        public long Population { get; set; }
        public Dictionary<string, string> Languages { get; set; }
        public Dictionary<string, Currency> Currencies { get; set; }
        public string Cca2 { get; set; }
        public string Region { get; set; }
        public double Area { get; set; }
        public string[] Capital { get; set; }
        public string[] RegionalBlocs { get; set; }
    }

    public class Name
    {
        public string Common { get; set; }
        public string Official { get; set; }
    }

    public class Currency
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
