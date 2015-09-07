using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using AutoMapper;
using Implementation.Entities.Interfaces;
using Implementation.Exceptions;
using MongoDB.Bson.Serialization.Attributes;

namespace Implementation.Entities
{
    #region Entities

    [BsonIgnoreExtraElements]
    public class TeamMongo : MongoBaseEntity
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("market")]
        public string Market { get; set; }

        [BsonElement("conference__id")]
        public string ConvferenceId { get; set; }

        [BsonElement("division__id")]
        public string DivisionId { get; set; }
    }
    

    [BsonIgnoreExtraElements]
    public class ConferenceMongo : MongoBaseEntity
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("divisions")]
        public IList<Division> Divisions { get; set; }

        public class Division
        {
            [BsonElement("division")]
            public string divisionId { get; set; }
        }
    }

    [BsonIgnoreExtraElements]
    public class DivisionMongo : MongoBaseEntity
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("conference__id")]
        public string ConferenceId { get; set; }

        [BsonElement("teams")]
        public IList<Team> Teams { get; set; }


        public class Team
        {
            [BsonElement("team")]
            public string teamId { get; set; }
        }
    }




    public class TeamSql : MySqlBaseEntity
    {
        public string TeamName { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
    }


    #endregion Entities


    public partial class SqlEntitiesFactory
    {
        public TeamSql CreateTeamSql(TeamMongo team, ConferenceMongo conference, DivisionMongo division)
        {
            if (conference.Divisions.FirstOrDefault(x => x.divisionId == division.EntityId) == null)
                throw new DataBaseConsitancyIsBrokenException("conference", "division__id");

            if (team.DivisionId != division.EntityId)
                throw new DataBaseConsitancyIsBrokenException("team", "division__id");

            TeamSql entity = Mapper.Map<TeamSql>(team);
            entity.Conference = conference.Name;
            entity.Division = division.Name;

            return entity;
        }
    }
    
    #region SQL mappings

    public class MapTeamSQL : EntityTypeConfiguration<TeamSql>
    {
        public MapTeamSQL()
        {
            ToTable("Team");
            HasKey(x => x.Id);

            SetAutoMapperProfile();
        }
        
        protected void SetAutoMapperProfile()
        {
            var map = Mapper.CreateMap<TeamMongo, TeamSql>();
            map.ForAllMembers(opt => opt.Ignore());
            map.ForMember(source => source.Id, dest => dest.MapFrom(x => x.EntityId));
            map.ForMember(source => source.TeamName, dest => dest.MapFrom(x => x.Name));

            Mapper.AssertConfigurationIsValid();
            
        }
    }


    #endregion MySQL mappings


    
}
