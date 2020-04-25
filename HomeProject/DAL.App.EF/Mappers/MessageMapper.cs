using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class MessageMapper : BaseDALMapper<Domain.Message, Message>
    {
        public MessageMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.Message, Message>();
                    config.CreateMap<Message, Domain.Message>();
                })
                .CreateMapper())
        {
        }
    }
}