using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class GiftMapper : BaseDALMapper<Domain.Gift, Gift>
    {
        public GiftMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.Gift, Gift>();
                    config.CreateMap<Gift, Domain.Gift>();
                })
                .CreateMapper())
        {
        }
    }
}