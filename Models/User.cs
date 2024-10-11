using Amazon.DynamoDBv2.DataModel;

namespace GloboClimaAPI.Models
{
    [DynamoDBTable("Users")]
    public class User
    {
        [DynamoDBHashKey] 
        public string Username { get; set; }

        public string Id { get; set; } 

        [DynamoDBProperty]
        public string PasswordHash { get; set; }

        [DynamoDBProperty]
        public string Email { get; set; }

        [DynamoDBProperty]
        public string FullName { get; set; }
    }
}
