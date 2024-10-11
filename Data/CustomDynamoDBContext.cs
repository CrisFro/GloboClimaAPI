using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;

namespace GloboClimaAPI.Data
{
    public class CustomDynamoDBContext
    {
        private readonly IDynamoDBContext _context;

        public CustomDynamoDBContext(IAmazonDynamoDB dynamoDBClient)
        {
            _context = new DynamoDBContext(dynamoDBClient);
        }

        public IDynamoDBContext Context => _context;
    }
}
