using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class ChatRoomMapper : BaseDALMapper<Domain.ChatRoom, ChatRoom>
    {
        public ChatRoomMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.ChatRoom, ChatRoom>();
                    config.CreateMap<ChatRoom, Domain.ChatRoom>();
                    
                    config.CreateMap<Domain.ChatMember, ChatMember>();
                    config.CreateMap<ChatMember, Domain.ChatMember>();
                })
                .CreateMapper())
        {
        }
    }
}