using MongoAPI.Models.Common;
using MongoAPI.Models.Hazards;
using MongoAPI.Models.KeyModels;
using MongoAPI.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoAPI.Repositories.Core
{
    public class TMRepository : MongoRepository<TM, ObjectId>, ITMRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Region> _regionCollection;
        private readonly IMongoCollection<Client> _clientCollection;

        public TMRepository(IMongoDatabase database) : base(database)
        {
            _database = database;
            _regionCollection = database.GetCollection<Region>(nameof(Region));
            _clientCollection = database.GetCollection<Client>(nameof(Client));
        }

        public async Task<TM> GetByTMNumberAsync(int tmNumber)
        {
            var filter = Builders<TM>.Filter.Eq(tm => tm.Id, tmNumber);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TM>> GetByNameAsync(string name)
        {
            var filter = Builders<TM>.Filter.Eq(tm => tm.Name, name);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<TM>> GetByTypeAsync(TMType type)
        {
            var filter = Builders<TM>.Filter.Eq(tm => tm.Type, type);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<TM>> GetByVoltageAsync(int voltage)
        {
            var filter = Builders<TM>.Filter.AnyEq(tm => tm.Voltages, voltage);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<TM>> GetByStateAsync(TMState state)
        {
            var filter = Builders<TM>.Filter.Eq(tm => tm.State, state);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<TM>> GetByLocationRadiusAsync(double latitude, double longitude, double radiusKm)
        {
            // MongoDB geospatial query to find TMs within a radius
            // Convert kilometers to meters for the query
            var radiusMeters = radiusKm * 1000;

            var builder = Builders<TM>.Filter;
            var filter = builder.GeoWithinCenterSphere(
                tm => new BsonDocument
                {
                    { "Latitude", tm.Location.Latitude },
                    { "Longitude", tm.Location.Longitude }
                },
                longitude,
                latitude,
                radiusKm / 6371.0 // Convert to radians (Earth's radius is approximately 6371 km)
            );

            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<TM>> GetByCityAsync(string city)
        {
            var filter = Builders<TM>.Filter.Eq(tm => tm.City, city);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<TM>> GetByCountyAsync(string county)
        {
            var filter = Builders<TM>.Filter.Eq(tm => tm.County, county);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<Region> GetRegionForTMAsync(ObjectId tmId)
        {
            var filter = Builders<Region>.Filter.ElemMatch(r => r.TMList, tm => tm._ID == tmId);
            return await _regionCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Client> GetClientForTMAsync(ObjectId tmId)
        {
            var region = await GetRegionForTMAsync(tmId);
            if (region == null) return null;

            var filter = Builders<Client>.Filter.ElemMatch(c => c.Regions, r => r._ID == region._ID);
            return await _clientCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Building>> GetBuildingsForTMAsync(ObjectId tmId)
        {
            var tm = await GetByIdAsync(tmId);
            return tm?.Buildings ?? new List<Building>();
        }

        public async Task<IEnumerable<TM>> GetByFireHazardLevelAsync(Level level)
        {
            var filter = Builders<TM>.Filter.Eq(tm => tm.FireHazard.Level, level);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<TM>> GetBySecurityHazardScoreAsync(double minScore, double maxScore)
        {
            var filter = Builders<TM>.Filter.And(
                Builders<TM>.Filter.Gte(tm => tm.SecurityHazard.Score, minScore),
                Builders<TM>.Filter.Lte(tm => tm.SecurityHazard.Score, maxScore)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<TM>> GetByLiquefactionRiskAsync(bool hasLiquefactionRisk)
        {
            var filter = Builders<TM>.Filter.Eq(tm => tm.Soil.LiquefactionRisk, hasLiquefactionRisk);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task AddBuildingToTMAsync(ObjectId tmId, Building building)
        {
            var update = Builders<TM>.Update.Push(tm => tm.Buildings, building);
            await _collection.UpdateOneAsync(tm => tm._ID == tmId, update);
        }

        public async Task UpdateHazardScoreAsync<THazard>(ObjectId tmId, THazard hazard) where THazard : class
        {
            // This will need specific implementation depending on hazard type
            // Here's a simple example for FireHazard
            if (hazard is FireHazard fireHazard)
            {
                var update = Builders<TM>.Update.Set(tm => tm.FireHazard, fireHazard);
                await _collection.UpdateOneAsync(tm => tm._ID == tmId, update);
            }
            // Add other hazard types as needed
        }

        public async Task AddAlternativeTMAsync(ObjectId tmId, ObjectId alternativeTMId)
        {
            var alternativeTM = await GetByIdAsync(alternativeTMId);
            if (alternativeTM == null) return;

            var update = Builders<TM>.Update.Push(tm => tm.Alternatives, alternativeTM);
            await _collection.UpdateOneAsync(tm => tm._ID == tmId, update);
        }

        public async Task<IEnumerable<TM>> GetAlternativeTMsAsync(ObjectId tmId)
        {
            var tm = await GetByIdAsync(tmId);
            return tm?.Alternatives ?? new List<TM>();
        }
    }
}