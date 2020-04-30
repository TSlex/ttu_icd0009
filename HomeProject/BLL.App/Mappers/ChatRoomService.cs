using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Mappers
{
    public class ChatRoomMapper : BaseBLLMapper<DAL.App.DTO.ChatRoom, ChatRoom>
    {
        public ChatRoomMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.ChatRoom, ChatRoom>();
                    config.CreateMap<ChatRoom, DAL.App.DTO.ChatRoom>();
                    
                    config.CreateMap<DAL.App.DTO.ChatMember, ChatMember>();
                    config.CreateMap<ChatMember, DAL.App.DTO.ChatMember>();
                })
                .CreateMapper())
        {
        }
    }
}