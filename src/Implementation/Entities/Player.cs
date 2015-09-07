using System.Data.Entity.ModelConfiguration;
using AutoMapper;
using Implementation.Entities.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Implementation.Entities
{
    #region Entities

    [BsonIgnoreExtraElements]
    public class PlayerMongo : MongoBaseEntity
    {

        public string FullName
        {
            get
            {
                return string.IsNullOrEmpty(fullName) ? name : fullName;
            }
        }

        [BsonElement("name_full")]
        public string fullName;

        [BsonElement("name")]
        public string name;

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("position")]
        public string Position { get; set; }

        [BsonElement("depth")]
        public long? Depth { get; set; }

        [BsonElement("team__id")]
        public string teamId;

        [BsonElement("team")]
        public string team;

        public string TeamId
        {
            get
            {
                return string.IsNullOrEmpty(teamId) ? team : teamId;
            }
        }

    [BsonElement("game__id")]
        public string GameId { get; set; }

        [BsonElement("play__id")]
        public string PlayId { get; set; }

    }


    public class PlayerSql : MySqlBaseEntity
    {
        public string PlayerFullName { get; set; }
        public string DraftClub { get; set; }
        public string DraftRd { get; set; }
        public string DraftPk { get; set; }
        public string OverallPk { get; set; }
        public string CurrentStatus { get; set; }
        public string Position { get; set; }
        public string JerseyNumber { get; set; }
        public long? CurrentDepthNumber { get; set; }
        public string CurrentTeamId { get; set; }
    }

    #endregion Entities

    public partial class SqlEntitiesFactory
    {
        public PlayerSql CreatePlayerSql(PlayerMongo player)
        {
            PlayerSql entity = Mapper.Map<PlayerSql>(player);
            


            return entity;
        }

    }

    #region SQL mappings

    public class MapPayerSql : EntityTypeConfiguration<PlayerSql>
    {
        public MapPayerSql()
        {
            ToTable("Player");
            HasKey(x => x.Id);

            SetAutoMapperProfile();
        }


        protected void SetAutoMapperProfile()
        {
            var map = Mapper.CreateMap<PlayerMongo, PlayerSql>();
            map.ForAllMembers(opt => opt.Ignore());
            map.ForMember(source => source.Id, dest => dest.MapFrom(x => x.EntityId));
            map.ForMember(source => source.PlayerFullName, dest => dest.MapFrom(x => x.FullName));
            map.ForMember(source => source.Position, dest => dest.MapFrom(x => x.Position));
            map.ForMember(source => source.CurrentStatus, dest => dest.MapFrom(x => x.Status));
            map.ForMember(source => source.CurrentDepthNumber, dest => dest.MapFrom(x => x.Depth));
            map.ForMember(source => source.CurrentTeamId, dest => dest.MapFrom(x => x.TeamId));

            Mapper.AssertConfigurationIsValid();

        }
    }


    #endregion MySQL mappings

}
