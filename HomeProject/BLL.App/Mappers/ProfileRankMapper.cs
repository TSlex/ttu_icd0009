using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Mappers
{
    public class ProfileRankMapper : BaseBLLMapper<DAL.App.DTO.ProfileRank, ProfileRank>
    {
        public ProfileRankMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.ProfileRank, ProfileRank>();
                    config.CreateMap<ProfileRank, DAL.App.DTO.ProfileRank>();
                })
                .CreateMapper())
        {
        }
    }
}