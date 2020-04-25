using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using Profile = BLL.App.DTO.Profile;

namespace BLL.App.Mappers
{
    public class ChatMemberMapper : BaseBLLMapper<DAL.App.DTO.ChatMember, ChatMember>
    {
        public ChatMemberMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.ChatMember, ChatMember>();
                    config.CreateMap<ChatMember, DAL.App.DTO.ChatMember>();
                })
                .CreateMapper())
        {
        }
    }
}