using AutoMapper;
using DAL.App.DTO;
using ee.itcollege.aleksi.DAL.Base.EF.Mappers;

namespace DAL.App.Mappers
{
    public class UniversalDALMapper<TInObject, TOutObject> : BaseDALMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        public UniversalDALMapper() : base(
            new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();
                
                // own mapping for composite entities
                config.CreateMap<Domain.HomeWork, HomeWork>();
                config.CreateMap<HomeWork, Domain.HomeWork>();
                
                config.CreateMap<Domain.Semester, Semester>();
                config.CreateMap<Semester, Domain.Semester>();
                
                config.CreateMap<Domain.Subject, Subject>();
                config.CreateMap<Subject, Domain.Subject>();
                
                config.CreateMap<Domain.StudentSubject, StudentSubject>();
                config.CreateMap<StudentSubject, Domain.StudentSubject>();
                
                config.CreateMap<Domain.StudentHomeWork, StudentHomeWork>();
                config.CreateMap<StudentHomeWork, Domain.StudentHomeWork>();

                config.AllowNullDestinationValues = true;
            }).CreateMapper())
        {
        }
    }
}