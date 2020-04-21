using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using Profile = DAL.App.DTO.Profile;

namespace DAL.Mappers
{
    public class PostMapper : BaseDALMapper<Domain.Post, Post>
    {
        public PostMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.Post, Post>();
                    config.CreateMap<Post, Domain.Post>();
                    config.CreateMap<Domain.Profile, Profile>();
                    config.CreateMap<Profile, Domain.Profile>();
                })
                .CreateMapper())
        {
        }

        public override Post Map(Domain.Post inObject)
        {
            return base.Map(inObject);
        }

        public override Domain.Post MapReverse(Post outObject)
        {
            return base.MapReverse(outObject);
        }
    }
}