using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class ProfileGiftMapper : BaseDALMapper<Domain.ProfileGift, ProfileGift>
    {
        public ProfileGiftMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.ProfileGift, ProfileGift>();
                    config.CreateMap<ProfileGift, Domain.ProfileGift>();
                })
                .CreateMapper())
        {
        }
    }
}