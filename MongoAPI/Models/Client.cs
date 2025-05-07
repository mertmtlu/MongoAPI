using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoAPI.Models
{
    public class Client
    {
        [BsonId] public ObjectId Id { get; set; }
        public required string Name { get; set; }
        public ClientType Type { get; set; }
        public required List<Region> Regions { get; set; } = new();
    }

    public enum ClientType
    {
        Private,
        State,
    }
}
