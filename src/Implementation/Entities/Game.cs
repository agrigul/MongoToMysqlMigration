using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using AutoMapper;
using Implementation.Entities.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Implementation.Entities
{
    #region Entities

    [BsonIgnoreExtraElements]
    public class GameMongo : MongoBaseEntity
    {
        [BsonElement("away")]
        public string AwayId { get; set; }

        [BsonElement("home")]
        public string HomeId { get; set; }

        [BsonElement("season__id")]
        public string SeasonId { get; set; }


        //"home" : "SEA",
        //"home_rotation" : "",
        //"id" : "3c42f4ea-e4b3-449d-82d5-36850144add9",
        //"scheduled" : "2014-09-05T00:30:00+00:00",
        //"status" : "closed",
        //"parent_api__id" : "schedule",
        //"season__id" : "http://api.sportsdatallc.org/nfl-t1/2014/REG/schedule.xml",
    }

    [BsonIgnoreExtraElements]
    public class SeasonMongo : MongoBaseEntity
    {

        [BsonElement("season")]
        public int Season { get; set; }

        [BsonElement("season_type")]
        public string SeasonType { get; set; }

        [BsonElement("team")]
        public string Team { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("weeks")]
        public IList<WeekItem> Weeks { get; set; }

        [BsonIgnoreExtraElements]
        public class WeekItem
        {
            [BsonElement("week")]
            public Week Week { get; set; }
        }

        [BsonIgnoreExtraElements]
        public class Week
        {
            [BsonElement("week")]
            public double WeekValue { get; set; }
        }

    }


    public class GameSql : MySqlBaseEntity
    {

        public DateTime? GameDate { get; set; }
        public int? Season { get; set; }
        public string SeasonType { get; set; }
        public int? Week { get; set; }
        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }
        public string WeatherCondition { get; set; }

        public int? HomeTeamQ1Score { get; set; }
        public int? HomeTeamQ2Score { get; set; }
        public int? HomeTeamQ3Score { get; set; }
        public int? HomeTeamQ4Score { get; set; }

        public int? AwayTeamQ1Score { get; set; }
        public int? AwayTeamQ2Score { get; set; }
        public int? AwayTeamQ3Score { get; set; }
        public int? AwayTeamQ4Score { get; set; }

        public int? HomeTeamFinalScore { get; set; }
        public int? AwayTeamFinalScore { get; set; }

    }

    #endregion Entities

    public partial class SqlEntitiesFactory
    {
        public GameSql CreateGameSql(GameMongo game, SeasonMongo season)
        {
            if(game == null)
                throw new ArgumentNullException("game");

            GameSql entity = Mapper.Map<GameSql>(game);
            entity.Season = season == null ? 0 : season.Season;
            entity.SeasonType = season == null ? string.Empty : season.SeasonType;
            
            if (season != null && season.Weeks.Any())
                entity.Week = (int)season.Weeks[0].Week.WeekValue;

            return entity;
        }

    }

    #region SQL mappings

    public class MapGameSQL : EntityTypeConfiguration<GameSql>
    {
        public MapGameSQL()
        {
            ToTable("Game");
            HasKey(x => x.Id);

            SetAutoMapperProfile();
        }


        protected void SetAutoMapperProfile()
        {
            var map = Mapper.CreateMap<GameMongo, GameSql>();
            map.ForAllMembers(opt => opt.Ignore());
            map.ForMember(source => source.Id, dest => dest.MapFrom(x => x.EntityId));

            Mapper.AssertConfigurationIsValid();

        }
    }


    #endregion MySQL mappings
}
