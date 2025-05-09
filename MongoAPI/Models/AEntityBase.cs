using MongoAPI.Models.Block;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoAPI.Models.KeyModels;

namespace MongoAPI.Models
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(
    typeof(Client),
    typeof(Region),
    typeof(TM),
    typeof(Building),
    typeof(Concrete),
    typeof(Masonry))]
    public abstract class AEntityBase
    {
        [BsonId]
        public ObjectId _ID { get; set; }
    }
}
