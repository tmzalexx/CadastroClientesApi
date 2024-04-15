using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CadastroClienteApi.mailer.Infrastructure;

    public class RegistrationRepository
    {
        private readonly IMongoDatabase _database;

        public RegistrationRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("MongoDBSettings:ConnectionString")?.Value ?? throw new ArgumentNullException(nameof(connectionString));
            string databaseName = configuration.GetSection("MongoDBSettings:DatabaseName")?.Value ?? throw new ArgumentNullException(nameof(databaseName));
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public async Task<T> GetDocumentById<T>(string collectionName, ObjectId id)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateSendEmailFlag<T>(string collectionName, ObjectId id)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var update = Builders<T>.Update.Set("SendEmail", true);
            var result = await collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
        
    }

