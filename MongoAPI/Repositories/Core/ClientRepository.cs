using MongoAPI.Models.KeyModels;
using MongoAPI.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoAPI.Repositories.Core
{
    public class ClientRepository : MongoRepository<Client, ObjectId>, IClientRepository
    {
        private readonly IMongoDatabase _database;

        public ClientRepository(IMongoDatabase database) : base(database)
        {
            _database = database;
        }

        public async Task<Client> GetByNameAsync(string clientName)
        {
            var filter = Builders<Client>.Filter.Eq(c => c.Name, clientName);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Client>> GetByTypeAsync(ClientType type)
        {
            var filter = Builders<Client>.Filter.Eq(c => c.Type, type);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Region>> GetRegionsForClientAsync(ObjectId clientId)
        {
            var client = await GetByIdAsync(clientId);
            return client?.Regions ?? new List<Region>();
        }

        public async Task<IEnumerable<TM>> GetAllTMsForClientAsync(ObjectId clientId)
        {
            var regions = await GetRegionsForClientAsync(clientId);
            var allTMs = new List<TM>();

            foreach (var region in regions)
            {
                allTMs.AddRange(region.TMList);
            }

            return allTMs;
        }

        public async Task<int> GetTotalTMCountForClientAsync(ObjectId clientId)
        {
            var regions = await GetRegionsForClientAsync(clientId);
            return regions.Sum(r => r.TMList.Count);
        }

        public async Task<Dictionary<string, int>> GetTMCountByRegionAsync(ObjectId clientId)
        {
            var regions = await GetRegionsForClientAsync(clientId);
            return regions.ToDictionary(
                r => r.Headquarters,
                r => r.TMList.Count
            );
        }

        public async Task AddRegionToClientAsync(ObjectId clientId, Region region)
        {
            var update = Builders<Client>.Update.Push(c => c.Regions, region);
            await _collection.UpdateOneAsync(c => c._ID == clientId, update);
        }

        public async Task AddRegionsToClientAsync(ObjectId clientId, IEnumerable<Region> regions)
        {
            foreach (var region in regions)
            {
                await AddRegionToClientAsync(clientId, region);
            }
        }
    }
}