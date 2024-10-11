using Amazon.DynamoDBv2.DataModel;

namespace GloboClimaAPI.Models
{
    [DynamoDBTable("Favorites")]
    public class Favorite
    {
        [DynamoDBHashKey]
        public string Id { get; set; } 

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string Country { get; set; }

        [DynamoDBProperty]
        public string City { get; set; } 
    }
}
