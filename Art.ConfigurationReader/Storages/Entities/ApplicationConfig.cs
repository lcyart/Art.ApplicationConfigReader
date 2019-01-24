using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Art.ConfigurationReader.Storages.Entities
{
    class ApplicationConfig
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }
    }
}
