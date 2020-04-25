using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class ChatMemberMapper : BaseDALMapper<Domain.ChatMember, ChatMember>
    {
        public ChatMemberMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.ChatMember, ChatMember>();
                    config.CreateMap<ChatMember, Domain.ChatMember>();
                })
                .CreateMapper())
        {
        }
    }
}