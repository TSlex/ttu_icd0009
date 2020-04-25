using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Mappers
{
    public class MessageMapper : BaseBLLMapper<DAL.App.DTO.Message, Message>
    {
        public MessageMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.Message, Message>();
                    config.CreateMap<Message, DAL.App.DTO.Message>();
                })
                .CreateMapper())
        {
        }
    }
}