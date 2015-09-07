using System;

namespace Implementation.Entities.Interfaces
{
    /// <summary>
    /// interface for mysql tables
    /// </summary>
    public interface IMySqlEntity
    {
        string Id { get; set; }
    }


    /// <summary>
    ///  implementation of IMySqlEntity interface in base class
    /// </summary>
    public abstract class MySqlBaseEntity : IMySqlEntity
    {
        /// <summary>
        /// Id of entity in Mysql, Equal to "id" (not _id) in Mongo
        /// </summary>
        public string Id { get; set; }

    }

}
