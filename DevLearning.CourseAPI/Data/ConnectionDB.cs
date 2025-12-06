using DevLearning.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevLearning.CourseAPI.Data
{
    public class ConnectionDB
    {
        public readonly IMongoCollection<Course> mongoCollection;

        public ConnectionDB(IOptions<MongoDBSettings> mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            mongoCollection = database.GetCollection<Course>(mongoDbSettings.Value.CollectionName);
        }
        public IMongoCollection<Course> GetMongoCollection()
        {
            return mongoCollection;
        }
    }
}
