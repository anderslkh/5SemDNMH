using Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBRepository.Repositories
{
    public class UserRepository
    {
        protected IMongoCollection<User> collection;

        public UserRepository()
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(Configuration.GetConnectionUri());

            MongoClient client = new MongoClient(Configuration.GetConnectionUri());

            IMongoDatabase database = client.GetDatabase(Configuration.GetDatabaseName());

            collection = database.GetCollection<User>(typeof(User).Name);
        }

        public async Task Create(User user)
        {
            await collection.InsertOneAsync(user);
        }

        public async Task<bool> Delete(string email)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(T => T.Email, email);
            DeleteResult result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount != 0;
        }

        public async Task<User> ReadOne(string email)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(T => T.Email, email);

            List<User> resultList = await collection.Find(filter).ToListAsync();

            User result = null;

            if (resultList.Count > 0)
            {
                result = resultList[0];
            }

            return result;
        }
    }
}
