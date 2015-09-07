using System.Data.Entity.ModelConfiguration;
using AutoMapper;
using Implementation.Entities.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Implementation.Entities
{
    #region Entities


    public class GameRosterSql : MySqlBaseEntity
    {
        public string GameId { get; set; }
        public string TeamId { get; set; }
        public string PlayerId { get; set; }
        public long? DepthNumber { get; set; }
    }

    #endregion Entities

    public partial class SqlEntitiesFactory
    {
        public GameRosterSql CreateGameRosterSql(GameMongo game, TeamMongo team, PlayerMongo player)
        {
            GameRosterSql entity = new GameRosterSql
            {
                DepthNumber = player == null ? null : player.Depth,
                TeamId = team == null ? string.Empty : team.EntityId,
                PlayerId = player == null ? string.Empty : player.EntityId,
                GameId = game == null ? string.Empty : game.EntityId
            };



            return entity;
        }

    }

    #region SQL mappings

    public class MapGameRosterSql : EntityTypeConfiguration<GameRosterSql>
    {
        public MapGameRosterSql()
        {
            ToTable("GameRoster");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("GameRoster");

            SetAutoMapperProfile();
        }


        protected void SetAutoMapperProfile()
        {
            //var map = Mapper.CreateMap<PlayerMongo, PlayerSql>();
            //map.ForAllMembers(opt => opt.Ignore());
            //map.ForMember(source => source.Id, dest => dest.MapFrom(x => x.IdInMongo));
            //map.ForMember(source => source.PlayerFullName, dest => dest.MapFrom(x => x.FullName));
            //map.ForMember(source => source.Position, dest => dest.MapFrom(x => x.Position));
            //map.ForMember(source => source.CurrentStatus, dest => dest.MapFrom(x => x.Status));
            //map.ForMember(source => source.CurrentDepthNumber, dest => dest.MapFrom(x => x.Depth));
            //map.ForMember(source => source.CurrentTeamId, dest => dest.MapFrom(x => x.TeamId));

            //Mapper.AssertConfigurationIsValid();

        }
    }


    #endregion MySQL mappings

}
