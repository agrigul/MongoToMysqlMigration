using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using AutoMapper;
using Implementation.Entities.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Implementation.Entities
{
    #region Entities

    [BsonIgnoreExtraElements]
    public class PlayMongo : MongoBaseEntity
    {
        [BsonElement("game__id")]
        public string GameId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("direction")]
        public string Direction { get; set; }

        [BsonElement("sequence")]
        public int Sequence { get; set; }

        [BsonElement("clock")]
        public string Clock { get; set; }

        [BsonElement("down")]
        public int Down { get; set; }

        [BsonElement("side")]
        public string Side { get; set; }

        [BsonElement("yard_line")]
        public int YardLine { get; set; }

        [BsonElement("summary")]
        public string Summary { get; set; }


        [BsonElement("participants__list")]
        public IList<PlayPartisipant> PlayerList { get; set; }


        public class PlayPartisipant
        {
            [BsonElement("player")]
            public string playerId { get; set; }
        }

    }



    public class PlaySummarySql : MySqlBaseEntity
    {
        public string GameId { get; set; }
        public string PossesionTeamId { get; set; }
        public string OffenseFormation { get; set; }
        public string DefenseFormation { get; set; }
        public string PlayType { get; set; }
        public string PlayDirection { get; set; }
        public int Sequence { get; set; }
        public string ClockTime { get; set; }
        public int Quarter { get; set; }
        public int Down { get; set; }
        public int YardsToGo { get; set; }
        public string SideOfBall { get; set; }
        public string YardLine { get; set; }
        public int AbsoluteYardLine { get; set; }
        public int PossesionTeamScoreDifferential { get; set; }
        public int PossesionTeamScore { get; set; }
        public int OppTeamScore { get; set; }
        public string Summary { get; set; }
        public int YardsGained { get; set; }
    }

    #endregion Entities

    public partial class SqlEntitiesFactory
    {
        public PlaySummarySql CreatePlaySummarySql(PlayMongo playMongo)
        {
            PlaySummarySql entity = Mapper.Map<PlaySummarySql>(playMongo);
           


            return entity;
        }

    }

    #region SQL mappings

    public class MapPlaySummarySql : EntityTypeConfiguration<PlaySummarySql>
    {
        public MapPlaySummarySql()
        {
            ToTable("PlaySummary");
            HasKey(x => x.Id);

            SetAutoMapperProfile();
        }


        protected void SetAutoMapperProfile()
        {
            var map = Mapper.CreateMap<PlayMongo, PlaySummarySql>();
            map.ForAllMembers(opt => opt.Ignore());
            map.ForMember(source => source.Id, dest => dest.MapFrom(x => x.EntityId));
            map.ForMember(source => source.GameId, dest => dest.MapFrom(x => x.GameId));
            map.ForMember(source => source.PlayType, dest => dest.MapFrom(x => x.Type));
            map.ForMember(source => source.PlayDirection, dest => dest.MapFrom(x => x.Direction));
            map.ForMember(source => source.Sequence, dest => dest.MapFrom(x => x.Sequence));
            map.ForMember(source => source.ClockTime, dest => dest.MapFrom(x => x.Clock));
            map.ForMember(source => source.Down, dest => dest.MapFrom(x => x.Down));
            map.ForMember(source => source.SideOfBall, dest => dest.MapFrom(x => x.Side));
            map.ForMember(source => source.YardLine, dest => dest.MapFrom(x => x.YardLine));
            map.ForMember(source => source.Summary, dest => dest.MapFrom(x => x.Summary));

            Mapper.AssertConfigurationIsValid();

        }
    }


    #endregion MySQL mappings

}
