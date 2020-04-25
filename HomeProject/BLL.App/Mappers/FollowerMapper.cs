using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class FollowerMapper : BaseBLLMapper<DAL.App.DTO.Follower, Follower>
    {
        public FollowerMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.Follower, Follower>();
                    config.CreateMap<Follower, DAL.App.DTO.Follower>();
                })
                .CreateMapper())
        {
        }
    }
}