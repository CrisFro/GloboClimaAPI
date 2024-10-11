namespace GloboClimaAPI.Models
{
    public class WeatherData
    {
        public string City { get; set; }              
        public string Description { get; set; }        
        public double Temperature { get; set; }       
        public double Humidity { get; set; }            
        public double WindSpeed { get; set; }           
        public double Pressure { get; set; }           
        public string Icon { get; set; }                
        public string ImageUrl { get; set; }           
        public double MinTemperature { get; set; }     
        public double MaxTemperature { get; set; }     
        public DateTime LastUpdated { get; set; }    

    }
}
