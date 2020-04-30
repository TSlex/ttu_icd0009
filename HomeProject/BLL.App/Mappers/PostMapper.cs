using AutoMapper;
using BLL.Base.Mappers;
using BLL.App.DTO;

namespace BLL.App.Mappers
{
    public class PostMapper : BaseBLLMapper<DAL.App.DTO.Post, Post>
    {
        public PostMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.Post, Post>();
                    config.CreateMap<Post, DAL.App.DTO.Post>();
                    config.CreateMap<DAL.App.DTO.Profile, ProfileFull>();
                    config.CreateMap<ProfileFull, DAL.App.DTO.Profile>();
                })
                .CreateMapper())
        {
        }
    }
}