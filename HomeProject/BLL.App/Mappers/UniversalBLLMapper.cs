using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;
using Profile = AutoMapper.Profile;

namespace BLL.App.Mappers
{
    public class UniversalBLLMapper<TInObject, TOutObject>: BaseBLLMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        public UniversalBLLMapper(): base(
            new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();

                config.CreateMap<DAL.App.DTO.ChatRoom, ChatRoom>();
                config.CreateMap<ChatRoom, DAL.App.DTO.ChatRoom>();

                config.CreateMap<DAL.App.DTO.Profile, Profile>();
                config.CreateMap<Profile, DAL.App.DTO.Profile>();

                config.CreateMap<DAL.App.DTO.ChatMember, ChatMember>();
                config.CreateMap<ChatMember, DAL.App.DTO.ChatMember>();

                config.CreateMap<DAL.App.DTO.ChatRole, ChatRole>();
                config.CreateMap<ChatRole, DAL.App.DTO.ChatRole>();
                
                config.CreateMap<DAL.App.DTO.Gift, Gift>();
                config.CreateMap<Gift, DAL.App.DTO.Gift>();
                
                config.CreateMap<DAL.App.DTO.Rank, Rank>();
                config.CreateMap<Rank, DAL.App.DTO.Rank>();
                
                config.CreateMap<DAL.App.DTO.ProfileRank, ProfileRank>();
                config.CreateMap<ProfileRank, DAL.App.DTO.ProfileRank>();
                
                config.CreateMap<DAL.App.DTO.ProfileGift, ProfileGift>();
                config.CreateMap<ProfileGift, DAL.App.DTO.ProfileGift>();

                config.CreateMap<DAL.App.DTO.Post, Post>();
                config.CreateMap<Post, DAL.App.DTO.Post>();
                
                config.CreateMap<DAL.App.DTO.Comment, Comment>();
                config.CreateMap<Comment, DAL.App.DTO.Comment>();
                
                config.CreateMap<DAL.App.DTO.Favorite, Favorite>();
                config.CreateMap<Favorite, DAL.App.DTO.Favorite>();
                
                config.CreateMap<DAL.App.DTO.Follower, Follower>();
                config.CreateMap<Follower, DAL.App.DTO.Follower>();
                
                config.CreateMap<DAL.App.DTO.BlockedProfile, BlockedProfile>();
                config.CreateMap<BlockedProfile, DAL.App.DTO.BlockedProfile>();

                config.CreateMap<DAL.App.DTO.Message, Message>();
                config.CreateMap<Message, DAL.App.DTO.Message>();
                
                config.CreateMap<DAL.App.DTO.Image, Image>();
                config.CreateMap<Image, DAL.App.DTO.Image>();
                
                config.CreateMap<DAL.App.DTO.Image?, Image?>();
                config.CreateMap<Image?, DAL.App.DTO.Image?>();

                config.AllowNullDestinationValues = true;
            }).CreateMapper())
        {
        }
    }
}