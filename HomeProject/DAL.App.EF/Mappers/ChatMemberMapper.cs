using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using Profile = DAL.App.DTO.Profile;

namespace DAL.Mappers
{
    public class ChatMemberMapper : BaseDALMapper<Domain.ChatMember, ChatMember>
    {
        public ChatMemberMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.ChatMember, ChatMember>();
                    config.CreateMap<ChatMember, Domain.ChatMember>();
                    
                    config.CreateMap<Domain.Profile, Profile>();
                    config.CreateMap<Profile, Domain.Profile>();
                    
                    config.CreateMap<Domain.ChatRole, ChatRole>();
                    config.CreateMap<ChatRole, Domain.ChatRole>();
                    
                    config.CreateMap<Domain.ChatRoom, ChatRoom>();
                    config.CreateMap<ChatRoom, Domain.ChatRoom>();
                })
                .CreateMapper())
        {
        }
    }
}