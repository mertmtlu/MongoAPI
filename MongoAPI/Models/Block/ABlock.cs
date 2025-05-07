using MongoDB.Bson.Serialization.Attributes;

namespace MongoAPI.Models.Block
{
    public abstract class ABlock
    {
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public StructureType StructureType { get; set; }
        public double XAxisLength { get; set; }
        public double YAxisLength { get; set; }
        public required Dictionary<int, double> StoreyHeight { get; set; } = new();
        //public AnalysisData AnalysisData { get; set; } = new();
        //public AnalysisResult AnalysisResult { get; set; }
        //public PostProcessResult PostProcessResult { get; set; }
        [BsonIgnore] public double LongLength { get 
            {
                if (XAxisLength > YAxisLength) return XAxisLength;
                else return YAxisLength;
            } 
        }
        [BsonIgnore] public double ShortLength {
            get
            {
                if (XAxisLength < YAxisLength) return XAxisLength;
                else return YAxisLength;
            }
        }
        [BsonIgnore] public double TotalHeight
        {
            get
            {
                double height = 0;
                foreach (var key in StoreyHeight.Keys) { if (!(key < 0)) height += StoreyHeight[key]; }
                return height;
            }
        }


        // TODO: Talk about areas
    }

    public enum StructureType
    {
        Masonry,
        Concrete
    }
}
