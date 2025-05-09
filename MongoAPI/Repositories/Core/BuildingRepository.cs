using MongoAPI.Models.Block;
using MongoAPI.Models.KeyModels;
using MongoAPI.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoAPI.Repositories.Core
{
    public class BuildingRepository : MongoRepository<Building, ObjectId>, IBuildingRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TM> _tmCollection;
        private readonly IMongoCollection<Region> _regionCollection;

        public BuildingRepository(IMongoDatabase database) : base(database)
        {
            _database = database;
            _tmCollection = database.GetCollection<TM>(nameof(TM));
            _regionCollection = database.GetCollection<Region>(nameof(Region));
        }

        public async Task<Building> GetByBuildingNumberAsync(int buildingNumber)
        {
            var filter = Builders<Building>.Filter.Eq(b => b.BuildingTMID, buildingNumber);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Building>> GetByTypeAsync(BuildingType type)
        {
            var filter = Builders<Building>.Filter.Eq(b => b.Type, type);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Building>> GetByMETUScopeAsync(bool inScope)
        {
            var filter = Builders<Building>.Filter.Eq(b => b.InScopeOfMETU, inScope);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<TM> GetTMForBuildingAsync(ObjectId buildingId)
        {
            var filter = Builders<TM>.Filter.ElemMatch(tm => tm.Buildings, b => b._ID == buildingId);
            return await _tmCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Region> GetRegionForBuildingAsync(ObjectId buildingId)
        {
            var tm = await GetTMForBuildingAsync(buildingId);
            if (tm == null) return null;

            var filter = Builders<Region>.Filter.ElemMatch(r => r.TMList, t => t._ID == tm._ID);
            return await _regionCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ABlock>> GetBlocksForBuildingAsync(ObjectId buildingId)
        {
            var building = await GetByIdAsync(buildingId);
            return building?.Blocks ?? new List<ABlock>();
        }

        public async Task AddBlockToBuildingAsync(ObjectId buildingId, ABlock block)
        {
            var update = Builders<Building>.Update.Push(b => b.Blocks, block);
            await _collection.UpdateOneAsync(b => b._ID == buildingId, update);
        }

        public async Task RemoveBlockFromBuildingAsync(ObjectId buildingId, ObjectId blockId)
        {
            var update = Builders<Building>.Update.PullFilter(
                b => b.Blocks,
                Builders<ABlock>.Filter.Eq(block => block._ID, blockId)
            );
            await _collection.UpdateOneAsync(b => b._ID == buildingId, update);
        }

        public async Task<Dictionary<BuildingType, int>> GetBuildingCountByTypeForTMAsync(ObjectId tmId)
        {
            var tm = await _tmCollection.Find(t => t._ID == tmId).FirstOrDefaultAsync();
            if (tm == null || tm.Buildings == null) return new Dictionary<BuildingType, int>();

            return tm.Buildings
                .GroupBy(b => b.Type)
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                );
        }
    }
}