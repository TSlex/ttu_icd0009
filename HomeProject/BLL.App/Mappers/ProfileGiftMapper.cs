using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Mappers
{
    public class ProfileGiftMapper : BaseBLLMapper<DAL.App.DTO.ProfileGift, ProfileGift>
    {
        public ProfileGiftMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.ProfileGift, ProfileGift>();
                    config.CreateMap<ProfileGift, DAL.App.DTO.ProfileGift>();
                })
                .CreateMapper())
        {
        }
    }
}