using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Mappers
{
    public class ChatRoleMapper : BaseBLLMapper<DAL.App.DTO.ChatRole, ChatRole>
    {
        public ChatRoleMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.ChatRole, ChatRole>();
                    config.CreateMap<ChatRole, DAL.App.DTO.ChatRole>();
                })
                .CreateMapper())
        {
        }
    }
}