using MongoAPI.Models.Common;

namespace MongoAPI.Models.TMRelatedProperties
{
    public class Pollution
    {
        public required Location PollutantLocation { get; set; }
        public required int PollutantNo { get; set; }
        public string PollutantSource { get; set; } = string.Empty;
        public double PollutantDistance { get; set; }
        public Level PollutantLevel { get; set; }
        //public PolutantCost Cost { get; set; }
    }
}
