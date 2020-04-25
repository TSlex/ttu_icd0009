using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class RankMapper : BaseBLLMapper<DAL.App.DTO.Rank, Rank>
    {
        public RankMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.Rank, Rank>();
                    config.CreateMap<Rank, DAL.App.DTO.Rank>();
                })
                .CreateMapper())
        {
        }
    }
}