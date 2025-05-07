using MongoAPI.Models.Block.Properties;

namespace MongoAPI.Models.Block
{
    public class Masonry : ABlock
    {
        public List<MasonryUnitType> UnitTypeList = new();
    }
}
