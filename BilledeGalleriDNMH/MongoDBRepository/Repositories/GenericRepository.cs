using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;

namespace MongoDBRepository.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : Entity
    {
        protected IMongoCollection<T> collection;

        public GenericRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(mongoDBSettings.Value.ConnectionURI);

            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);

            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            collection = database.GetCollection<T>(typeof(T).Name);
        }
     

        public async Task<T> Create(T t)
        {
            await collection.InsertOneAsync(t);
            return t;
        }

        public async Task<bool> Delete(Guid id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(T => T.Id == id);
            DeleteResult result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount != 0;
        }

        public Task<T> Read(T t)
        {
            throw new NotImplementedException();
        }

        protected virtual FilterDefinition<T> CreateFilterDefinition(BaseQueryParameters? filterObject)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Empty;

            if (filterObject is not null)
            {
                Console.Error.WriteLine("Filtering is not supported on generic repositories");
            }

            return filter;
        }
    }
}
