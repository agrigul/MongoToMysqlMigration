using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using AutoMapper;
using Implementation.Entities.Interfaces;

namespace Implementation.Entities
{
    #region Entities


    public class PlayPlayerPartisipantsSql : MySqlBaseEntity
    {
        public string GameId { get; set; }
        public string PlayId { get; set; }
        public string TeamId { get; set; }
        public string PlayerId { get; set; }

        public PlayPlayerPartisipantsSql()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    #endregion Entities

    public partial class SqlEntitiesFactory
    {
        public IList<PlayPlayerPartisipantsSql> CreatePlayPlayerPartisipantsSql(PlayMongo playMongo, IList<PlayerMongo> players)
        {
            IList<PlayPlayerPartisipantsSql> items = new List<PlayPlayerPartisipantsSql>();

            foreach (var player in players)
            {
                PlayPlayerPartisipantsSql entity = CreatePlayPlayerPartisipantsSql(playMongo, player);
                items.Add(entity);
            }

            return items;
        }

        protected PlayPlayerPartisipantsSql CreatePlayPlayerPartisipantsSql(PlayMongo playMongo, PlayerMongo player)
        {
            if (playMongo == null)
                throw new ArgumentNullException("playMongo");

            if (player == null)
                throw new ArgumentNullException("player");

            PlayPlayerPartisipantsSql entity = Mapper.Map<PlayPlayerPartisipantsSql>(playMongo);
            entity.PlayerId = player.EntityId;
            entity.TeamId = player.TeamId;
            return entity;
        }


    }

    #region SQL mappings

    public class MapPlayPlayerPartisipantsSql : EntityTypeConfiguration<PlayPlayerPartisipantsSql>
    {
        public MapPlayPlayerPartisipantsSql()
        {
            ToTable("playplayerparticipants");
            HasKey(x => x.Id);

            SetAutoMapperProfile();
        }


        protected void SetAutoMapperProfile()
        {
            var map = Mapper.CreateMap<PlayMongo, PlayPlayerPartisipantsSql>();
            map.ForAllMembers(opt => opt.Ignore());
            map.ForMember(source => source.PlayId, dest => dest.MapFrom(x => x.EntityId));
            map.ForMember(source => source.GameId, dest => dest.MapFrom(x => x.GameId));

            Mapper.AssertConfigurationIsValid();

        }
    }


    #endregion MySQL mappings

}
