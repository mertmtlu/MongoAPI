using MongoAPI.Models.KeyModels;
using MongoAPI.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoAPI.Repositories.Core
{
    public class RegionRepository : MongoRepository<Region, ObjectId>, IRegionRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Client> _clientCollection;

        public RegionRepository(IMongoDatabase database) : base(database)
        {
            _database = database;
            _clientCollection = database.GetCollection<Client>(nameof(Client));
        }

        public async Task<Region> GetByRegionNumberAsync(int regionNumber)
        {
            var filter = Builders<Region>.Filter.Eq(r => r.Id, regionNumber);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Region>> GetByHeadquartersAsync(string headquarters)
        {
            var filter = Builders<Region>.Filter.Eq(r => r.Headquarters, headquarters);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Region>> GetByCityAsync(string city)
        {
            var filter = Builders<Region>.Filter.AnyEq(r => r.Cities, city);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<Client> GetClientForRegionAsync(ObjectId regionId)
        {
            var filter = Builders<Client>.Filter.ElemMatch(c => c.Regions, r => r._ID == regionId);
            return await _clientCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TM>> GetTMsForRegionAsync(ObjectId regionId)
        {
            var region = await GetByIdAsync(regionId);
            return region?.TMList ?? new List<TM>();
        }

        public async Task AddTMToRegionAsync(ObjectId regionId, TM tm)
        {
            var update = Builders<Region>.Update.Push(r => r.TMList, tm);
            await _collection.UpdateOneAsync(r => r._ID == regionId, update);
        }

        public async Task RemoveTMFromRegionAsync(ObjectId regionId, ObjectId tmId)
        {
            var update = Builders<Region>.Update.PullFilter(
                r => r.TMList,
                Builders<TM>.Filter.Eq(tm => tm._ID, tmId)
            );
            await _collection.UpdateOneAsync(r => r._ID == regionId, update);
        }

        public async Task<Dictionary<string, int>> GetTMCountByCityAsync(ObjectId regionId)
        {
            var region = await GetByIdAsync(regionId);
            if (region == null) return new Dictionary<string, int>();

            return region.TMList
                .GroupBy(tm => tm.City)
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                );
        }
    }
}