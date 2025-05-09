using MongoAPI.Models.Block;
using MongoAPI.Models.KeyModels;
using MongoAPI.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoAPI.Repositories.Core
{
    public class BlockRepository : MongoRepository<ABlock, ObjectId>, IBlockRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Building> _buildingCollection;
        private readonly IMongoCollection<TM> _tmCollection;

        public BlockRepository(IMongoDatabase database) : base(database)
        {
            _database = database;
            _buildingCollection = database.GetCollection<Building>(nameof(Building));
            _tmCollection = database.GetCollection<TM>(nameof(TM));
        }

        public async Task<ABlock> GetByBlockCodeAsync(string blockCode)
        {
            var filter = Builders<ABlock>.Filter.Eq(b => b.ID, blockCode);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ABlock>> GetByModelingTypeAsync(ModelingType modelType)
        {
            var filter = Builders<ABlock>.Filter.Eq("ModelingType", modelType.ToString());
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Concrete>> GetConcreteBlocksAsync()
        {
            var filter = Builders<ABlock>.Filter.OfType<Concrete>();
            return await _collection.Find(filter).As<Concrete>().ToListAsync();
        }

        public async Task<IEnumerable<Masonry>> GetMasonryBlocksAsync()
        {
            var filter = Builders<ABlock>.Filter.OfType<Masonry>();
            return await _collection.Find(filter).As<Masonry>().ToListAsync();
        }

        public async Task<IEnumerable<Concrete>> GetByCompressiveStrengthRangeAsync(double minStrength, double maxStrength)
        {
            var filter = Builders<Concrete>.Filter.And(
                Builders<Concrete>.Filter.Gte(c => c.CompressiveStrengthOfConcrete, minStrength),
                Builders<Concrete>.Filter.Lte(c => c.CompressiveStrengthOfConcrete, maxStrength)
            );
            return await _database.GetCollection<Concrete>(nameof(ABlock))
                .Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Concrete>> GetByReinforcementRatioRangeAsync(double minRatio, double maxRatio)
        {
            var filter = Builders<Concrete>.Filter.And(
                Builders<Concrete>.Filter.Gte(c => c.ReinforcementRatio, minRatio),
                Builders<Concrete>.Filter.Lte(c => c.ReinforcementRatio, maxRatio)
            );
            return await _database.GetCollection<Concrete>(nameof(ABlock))
                .Find(filter).ToListAsync();
        }

        public async Task<Building> GetBuildingForBlockAsync(ObjectId blockId)
        {
            var filter = Builders<Building>.Filter.ElemMatch(b => b.Blocks, block => block._ID == blockId);
            return await _buildingCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TM> GetTMForBlockAsync(ObjectId blockId)
        {
            var building = await GetBuildingForBlockAsync(blockId);
            if (building == null) return null;

            var filter = Builders<TM>.Filter.ElemMatch(tm => tm.Buildings, b => b._ID == building._ID);
            return await _tmCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ABlock>> GetByHeightRangeAsync(double minHeight, double maxHeight)
        {
            var filter = Builders<ABlock>.Filter.And(
                Builders<ABlock>.Filter.Where(b => b.TotalHeight >= minHeight),
                Builders<ABlock>.Filter.Where(b => b.TotalHeight <= maxHeight)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<ABlock>> GetByStoreyCountAsync(int storeyCount)
        {
            var filter = Builders<ABlock>.Filter.Size(b => b.StoreyHeight, storeyCount);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}