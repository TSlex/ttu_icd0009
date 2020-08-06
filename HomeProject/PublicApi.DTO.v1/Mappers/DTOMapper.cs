using AutoMapper;
using BLL.App.DTO;
using ee.itcollege.aleksi.DAL.Base.EF.Mappers;
using Profile = BLL.App.DTO.Profile;

namespace PublicApi.DTO.v1.Mappers
{
    public class DTOMapper<TInObject, TOutObject> : BaseDALMapper<TInObject, TOutObject>
        where TOutObject : class, new()
        where TInObject : class, new()
    {
        public DTOMapper() : base(
            new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();

                config.CreateMap<ChatRoomGetDTO, ChatRoom>();
                config.CreateMap<ChatRoom, ChatRoomGetDTO>();

                config.CreateMap<ProfileDTO, Profile>();
                config.CreateMap<Profile, ProfileDTO>();

                config.CreateMap<ChatMemberDTO, ChatMember>();
                config.CreateMap<ChatMember, ChatMemberDTO>();

                config.CreateMap<ChatRoleDTO, ChatRole>();
                config.CreateMap<ChatRole, ChatRoleDTO>();

                config.CreateMap<GiftDTO, Gift>();
                config.CreateMap<Gift, GiftDTO>();

                config.CreateMap<RankDTO, Rank>();
                config.CreateMap<Rank, RankDTO>();

                config.CreateMap<ProfileGiftDTO, ProfileGift>();
                config.CreateMap<ProfileGift, ProfileGiftDTO>();

                config.CreateMap<BlockedProfileDTO, BlockedProfile>();
                config.CreateMap<BlockedProfile, BlockedProfileDTO>();

                config.CreateMap<ImageDTO, Image>();
                config.CreateMap<Image, ImageDTO>();
                
                config.AllowNullDestinationValues = true;
            }).CreateMapper())
        {
        }
    }
}