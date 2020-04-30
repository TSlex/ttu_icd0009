using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Mappers
{
    public class BlockedProfileMapper : BaseBLLMapper<DAL.App.DTO.BlockedProfile, BlockedProfile>
    {
        public BlockedProfileMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.BlockedProfile, BlockedProfile>();
                    config.CreateMap<BlockedProfile, DAL.App.DTO.BlockedProfile>();
                })
                .CreateMapper())
        {
        }
    }
}