using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class FavoriteMapper : BaseDALMapper<Domain.Favorite, Favorite>
    {
        public FavoriteMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.Favorite, Favorite>();
                    config.CreateMap<Favorite, Domain.Favorite>();
                })
                .CreateMapper())
        {
        }
    }
}