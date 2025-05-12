using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoAPI.Models.KeyModels
{
    public class Region : AEntityBase
    {
        public required ObjectId ClientID { get; set; }
        public required int Id { get; set; }  // Replaced with No (genel_bilgi: bolge_no)
        public required List<string> Cities { get; set; }
        public required string Headquarters { get; set; }
    }
}
