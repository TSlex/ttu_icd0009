using AutoMapper;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class CommentMapper : BaseDALMapper<Domain.Comment, Comment>
    {
        public CommentMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<Domain.Comment, Comment>();
                    config.CreateMap<Comment, Domain.Comment>();
                })
                .CreateMapper())
        {
        }
    }
}