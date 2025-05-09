using MongoAPI.Models.Block.Properties;

namespace MongoAPI.Models.Block
{
    public class Masonry : ABlock
    {
        public override ModelingType ModelingType => ModelingType.Masonry;
        public List<MasonryUnitType> UnitTypeList = new();
    }
}