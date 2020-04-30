using AutoMapper;
using BLL.Base.Mappers;
using BLL.App.DTO;

namespace BLL.App.Mappers
{
    public class ProfileMapper : BaseBLLMapper<DAL.App.DTO.Profile, ProfileFull>
    {
        public ProfileMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.Profile, ProfileFull>();
                    config.CreateMap<ProfileFull, DAL.App.DTO.Profile>();
                    
                    config.CreateMap<DAL.App.DTO.Post, Post>();
                    config.CreateMap<Post, DAL.App.DTO.Post>();
                    
                    config.CreateMap<DAL.App.DTO.ChatMember, ChatMember>();
                    config.CreateMap<ChatMember, DAL.App.DTO.ChatMember>();
                })
                .CreateMapper())
        {
        }
    }
}