using Models;
using MongoDB.Driver;

namespace MongoDBRepository.Repositories
{
    public class GalleryRepository
    {
        protected IMongoCollection<Gallery> collection;

        public GalleryRepository()
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(Configuration.GetConnectionUri());

            MongoClient client = new MongoClient(Configuration.GetConnectionUri());

            IMongoDatabase database = client.GetDatabase(Configuration.GetDatabaseName());

            collection = database.GetCollection<Gallery>(typeof(Gallery).Name);
        }

        public async Task Create(Gallery gallery)
        {
            await collection.InsertOneAsync(gallery);
        }

        public async Task<Gallery> ReadOne(string name)
        {
            FilterDefinition<Gallery> filter = Builders<Gallery>.Filter.Eq(gallery => gallery.Name, name);

            List<Gallery> resultList = await collection.Find(filter).ToListAsync();

            Gallery result = null;

            if (resultList.Count > 1)
            {
                result = resultList[0];
            }

            return result;
        }

        public async Task<bool> Delete(string name)
        {
            FilterDefinition<Gallery> filter = Builders<Gallery>.Filter.Eq(gallery => gallery.Name, name);
            DeleteResult result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount != 0;
        }
    }
}
