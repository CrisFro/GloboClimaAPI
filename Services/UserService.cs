using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using GloboClimaAPI.Models;

namespace GloboClimaAPI.Services
{
    public class UserService
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public UserService(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task SaveUserAsync(User user)
        {
            var context = new DynamoDBContext(_dynamoDb);
            await context.SaveAsync(user);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var context = new DynamoDBContext(_dynamoDb);
            return await context.LoadAsync<User>(username);
        }
    }
}
