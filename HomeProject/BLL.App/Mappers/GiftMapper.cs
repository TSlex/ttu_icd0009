using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class GiftMapper : BaseBLLMapper<DAL.App.DTO.Gift, Gift>
    {
        public GiftMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.Gift, Gift>();
                    config.CreateMap<Gift, DAL.App.DTO.Gift>();
                })
                .CreateMapper())
        {
        }
    }
}