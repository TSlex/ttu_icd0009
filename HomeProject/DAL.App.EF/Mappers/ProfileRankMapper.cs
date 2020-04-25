using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class ProfileRankMapper : BaseDALMapper<Domain.ProfileRank, ProfileRank>
    {
        public ProfileRankMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.ProfileRank, ProfileRank>();
                    config.CreateMap<ProfileRank, Domain.ProfileRank>();
                })
                .CreateMapper())
        {
        }
    }
}