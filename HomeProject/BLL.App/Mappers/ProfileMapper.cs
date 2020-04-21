using AutoMapper;
using BLL.Base.Mappers;
using BLL.App.DTO;
using Profile = BLL.App.DTO.Profile;

namespace BLL.App.Mappers
{
    public class ProfileMapper : BaseBLLMapper<DAL.App.DTO.Profile, Profile>
    {
        public ProfileMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.Profile, Profile>();
                    config.CreateMap<Profile, DAL.App.DTO.Profile>();
                    config.CreateMap<DAL.App.DTO.Post, Post>();
                    config.CreateMap<Post, DAL.App.DTO.Post>();
                })
                .CreateMapper())
        {
        }
    }
}