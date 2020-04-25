using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class FollowerMapper : BaseDALMapper<Domain.Follower, Follower>
    {
        public FollowerMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.Follower, Follower>();
                    config.CreateMap<Follower, Domain.Follower>();
                })
                .CreateMapper())
        {
        }
    }
}