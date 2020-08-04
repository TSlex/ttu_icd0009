using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using Profile = DAL.App.DTO.Profile;

namespace DAL.Mappers
{
    public class UniversalDALMapper<TInObject, TOutObject> : BaseDALMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        public UniversalDALMapper() : base(
            new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();

                config.CreateMap<Domain.ChatRoom, ChatRoom>();
                config.CreateMap<ChatRoom, Domain.ChatRoom>();

                config.CreateMap<Domain.Profile, Profile>();
                config.CreateMap<Profile, Domain.Profile>();

                config.CreateMap<Domain.ChatMember, ChatMember>();
                config.CreateMap<ChatMember, Domain.ChatMember>();

                config.CreateMap<Domain.ChatRole, ChatRole>();
                config.CreateMap<ChatRole, Domain.ChatRole>();

                config.CreateMap<Domain.Gift, Gift>();
                config.CreateMap<Gift, Domain.Gift>();

                config.CreateMap<Domain.Rank, Rank>();
                config.CreateMap<Rank, Domain.Rank>();

                config.CreateMap<Domain.ProfileRank, ProfileRank>();
                config.CreateMap<ProfileRank, Domain.ProfileRank>();

                config.CreateMap<Domain.ProfileGift, ProfileGift>();
                config.CreateMap<ProfileGift, Domain.ProfileGift>();

                config.CreateMap<Domain.Post, Post>();
                config.CreateMap<Post, Domain.Post>();

                config.CreateMap<Domain.Comment, Comment>();
                config.CreateMap<Comment, Domain.Comment>();

                config.CreateMap<Domain.Favorite, Favorite>();
                config.CreateMap<Favorite, Domain.Favorite>();

                config.CreateMap<Domain.Follower, Follower>();
                config.CreateMap<Follower, Domain.Follower>();

                config.CreateMap<Domain.BlockedProfile, BlockedProfile>();
                config.CreateMap<BlockedProfile, Domain.BlockedProfile>();

                config.CreateMap<Domain.Message, Message>();
                config.CreateMap<Message, Domain.Message>();

                config.CreateMap<Domain.Image, Image>();
                config.CreateMap<Image, Domain.Image>();

                config.AllowNullDestinationValues = true;
            }).CreateMapper())
        {
        }
    }
}