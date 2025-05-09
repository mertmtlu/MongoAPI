using MongoAPI.Models.Block;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoAPI.Models
{
    public class Building
    {
        public int BuildingTMID { get; set; }
        public string Name { get; set; } = string.Empty;
        public BuildingType Type { get; set; }
        public bool InScopeOfMETU { get; set; }
        public List<ABlock> Blocks { get; set; } = new();
        public string ReportName { get; set; } = string.Empty;
        //public ReportInput ReportInput { get; set; }

        [BsonIgnore] public int Code
        {
            get
            {
                switch (Type)
                {
                    case BuildingType.Control: return 1;
                    case BuildingType.Switchyard: return 2;
                    case BuildingType.Security: return 10;
                    default: return 0;
                }
            }
        }
        [BsonIgnore] public int BKS 
        { 
            get
            {
                switch (Type)
                {
                    case BuildingType.Control: return 3;
                    default: return 1;
                }
            } 
        }
    }

    public enum BuildingType
    {
        Control,
        Security,
        Switchyard
    }
}
