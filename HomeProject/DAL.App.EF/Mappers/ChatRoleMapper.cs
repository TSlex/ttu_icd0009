using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class ChatRoleMapper : BaseDALMapper<Domain.ChatRole, ChatRole>
    {
        public ChatRoleMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.ChatRole, ChatRole>();
                    config.CreateMap<ChatRole, Domain.ChatRole>();
                })
                .CreateMapper())
        {
        }
    }
}