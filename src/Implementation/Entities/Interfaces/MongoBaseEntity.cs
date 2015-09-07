using MongoDB.Bson.Serialization.Attributes;

namespace Implementation.Entities.Interfaces
{
    /// <summary>
    /// interface for Mongo entities
    /// </summary>
    public interface IMongoEntity
    {
        string IdInMongo { get; set; }
        string EntityId { get; set; }
        string ParentApiId { get; set; }
    }


    /// <summary>
    /// implementation of IMongoEntity in base class
    /// </summary>
    public abstract class MongoBaseEntity : IMongoEntity
    {
        //[BsonId]
        //[BsonElement("_id")]
        //public ObjectId Id { get; set; }
        
        /// <summary>
        /// Id in mongo database. Possible Index
        /// </summary>
        [BsonElement("_id")]
        public string IdInMongo { get; set; }

        /// <summary>
        /// non unique identificator of the mongo entity
        /// </summary>
        [BsonElement("id")]
        public string EntityId { get; set; }


        /// <summary>
        /// Id which links all the related entities. It's the same for all entities which has relations
        /// </summary>
        [BsonElement("dd_updated__id")]
        public long? DdUpdatedId { get; set; }


        /// <summary>
        /// Id wich with together with with EntityId makes it unique (complex private key)
        /// </summary>
        [BsonElement("parent_api__id")]
        public string ParentApiId { get; set; }
    }
}
