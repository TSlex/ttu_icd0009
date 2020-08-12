using AutoMapper;
using BLL.App.DTO;
using BLL.App.DTO.Identity;
using ee.itcollege.aleksi.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class UniversalBLLMapper<TInObject, TOutObject> : BaseBLLMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        public UniversalBLLMapper() : base(
            new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();

                // own mapping for composite entities
                config.CreateMap<DAL.App.DTO.Identity.AppUser, AppUser>();
                config.CreateMap<AppUser, DAL.App.DTO.Identity.AppUser>();
                
                config.CreateMap<DAL.App.DTO.Identity.AppRole, AppRole>();
                config.CreateMap<AppRole, DAL.App.DTO.Identity.AppRole>();
                
                config.CreateMap<DAL.App.DTO.HomeWork, HomeWork>();
                config.CreateMap<HomeWork, DAL.App.DTO.HomeWork>();
                
                config.CreateMap<DAL.App.DTO.Semester, Semester>();
                config.CreateMap<Semester, DAL.App.DTO.Semester>();
                
                config.CreateMap<DAL.App.DTO.Subject, Subject>();
                config.CreateMap<Subject, DAL.App.DTO.Subject>();
                
                config.CreateMap<DAL.App.DTO.StudentSubject, StudentSubject>();
                config.CreateMap<StudentSubject, DAL.App.DTO.StudentSubject>();
                
                config.CreateMap<DAL.App.DTO.StudentHomeWork, StudentHomeWork>();
                config.CreateMap<StudentHomeWork, DAL.App.DTO.StudentHomeWork>();

                config.AllowNullDestinationValues = true;
            }).CreateMapper())
        {
        }
    }
}