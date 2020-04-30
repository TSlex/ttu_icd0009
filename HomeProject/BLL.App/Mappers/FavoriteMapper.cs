using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Mappers
{
    public class FavoriteMapper : BaseBLLMapper<DAL.App.DTO.Favorite, Favorite>
    {
        public FavoriteMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.Favorite, Favorite>();
                    config.CreateMap<Favorite, DAL.App.DTO.Favorite>();
                })
                .CreateMapper())
        {
        }
    }
}