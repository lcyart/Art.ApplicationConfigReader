using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Art.ConfigurationReader.UI.Models
{
    public class ApplicationConfig
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public int ID { get; set; }
        [BsonElement]
        public string Name { get; set; }
        [BsonElement]
        public string Type { get; set; }
        [BsonElement]
        public string Value { get; set; }
        [BsonElement]
        public bool IsActive { get; set; }
        [BsonElement]
        public string ApplicationName { get; set; }
    }
}
