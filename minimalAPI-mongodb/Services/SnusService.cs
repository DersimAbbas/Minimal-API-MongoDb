using minimalAPI_mongodb.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace minimalAPI_mongodb.Services
{
    public class SnusService
    {
        private readonly IMongoCollection<Snus> _snusCollection;

        public SnusService(IOptions<SnuslagerDatabaseSettings> snuslagerDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                snuslagerDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
                snuslagerDatabaseSettings.Value.DatabaseName);
            _snusCollection = mongoDatabase.GetCollection<Snus>(
                snuslagerDatabaseSettings.Value.CollectionName);

        }
        //creating CRUD functions for fetching with minimal API.
        //GET
        public async Task<List<Snus>> GetAsync() =>
            await _snusCollection.Find(_ => true).ToListAsync();
        // GET BY ID.
        public async Task<Snus?> GetByIdAsync(string id) =>
            await _snusCollection.Find(m => m.Id == id).FirstOrDefaultAsync();
        //CREATE
        public async Task CreateAsync(Snus newSnus) =>
            await _snusCollection.InsertOneAsync(newSnus);

        public async Task UpdateAsync(string id, Snus updatedSnus) =>
            await _snusCollection.ReplaceOneAsync(m => m.Id== id, updatedSnus);

        public async Task RemoveAsync(string id) =>
            await _snusCollection.DeleteOneAsync(m => m.Id== id);

    }
}
