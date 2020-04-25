using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class RankMapper : BaseDALMapper<Domain.Rank, Rank>
    {
        public RankMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.Rank, Rank>();
                    config.CreateMap<Rank, Domain.Rank>();
                })
                .CreateMapper())
        {
        }
    }
}