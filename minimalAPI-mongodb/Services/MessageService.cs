using minimalAPI_mongodb.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace minimalAPI_mongodb.Services
{
    public class MessageService
    {
        private readonly IMongoCollection<Messages> _messagelogsCollection;

        public MessageService(IOptions<MessageLogDatabaseSettings> MessagelogDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                MessagelogDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
                MessagelogDatabaseSettings.Value.DatabaseName);
            _messagelogsCollection = mongoDatabase.GetCollection<Messages>(
                MessagelogDatabaseSettings.Value.CollectionName);

        }
        //creating CRUD functions for fetching with minimal API.
        //GET
        public async Task<List<Messages>> GetAsync() =>
            await _messagelogsCollection.Find(_ => true).ToListAsync();
        // GET BY ID.
        public async Task<Messages?> GetByIdAsync(string id) =>
            await _messagelogsCollection.Find(m => m.Id == id).FirstOrDefaultAsync();
        //CREATE
        public async Task CreateAsync(Messages newMessage) =>
            await _messagelogsCollection.InsertOneAsync(newMessage);

        public async Task UpdateAsync(string id, Messages updatedMessage) =>
            await _messagelogsCollection.ReplaceOneAsync(m => m.Id== id, updatedMessage);

        public async Task RemoveAsync(string id) =>
            await _messagelogsCollection.DeleteOneAsync(m => m.Id== id);

    }
}
