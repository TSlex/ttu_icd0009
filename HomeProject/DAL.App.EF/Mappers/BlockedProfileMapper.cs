using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class BlockedProfileMapper : BaseDALMapper<Domain.BlockedProfile, BlockedProfile>
    {
        public BlockedProfileMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.BlockedProfile, BlockedProfile>();
                    config.CreateMap<BlockedProfile, Domain.BlockedProfile>();
                })
                .CreateMapper())
        {
        }
    }
}