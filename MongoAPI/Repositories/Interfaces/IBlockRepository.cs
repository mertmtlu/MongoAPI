using MongoAPI.Models.Block;
using MongoAPI.Models.KeyModels;
using MongoDB.Bson;

namespace MongoAPI.Repositories.Interfaces
{
    /// <summary>
    /// Repository for Block entities and operations
    /// </summary>
    public interface IBlockRepository : IBaseRepository<ABlock, ObjectId>
    {
        // Block-specific queries
        Task<ABlock> GetByBlockCodeAsync(string blockCode);
        Task<IEnumerable<ABlock>> GetByModelingTypeAsync(ModelingType modelType);

        // Type-specific queries
        Task<IEnumerable<Concrete>> GetConcreteBlocksAsync();
        Task<IEnumerable<Masonry>> GetMasonryBlocksAsync();

        // Advanced querying for concrete properties
        Task<IEnumerable<Concrete>> GetByCompressiveStrengthRangeAsync(double minStrength, double maxStrength);
        Task<IEnumerable<Concrete>> GetByReinforcementRatioRangeAsync(double minRatio, double maxRatio);

        // Hierarchy navigation
        Task<Building> GetBuildingForBlockAsync(ObjectId blockId);
        Task<TM> GetTMForBlockAsync(ObjectId blockId);

        // Dimensions queries
        Task<IEnumerable<ABlock>> GetByHeightRangeAsync(double minHeight, double maxHeight);
        Task<IEnumerable<ABlock>> GetByStoreyCountAsync(int storeyCount);
    }
}
