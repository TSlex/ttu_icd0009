using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Mappers
{
    public class CommentMapper : BaseBLLMapper<DAL.App.DTO.Comment, Comment>
    {
        public CommentMapper() : base(
            new MapperConfiguration(config =>
                {
                    config.CreateMap<DAL.App.DTO.Comment, Comment>();
                    config.CreateMap<Comment, DAL.App.DTO.Comment>();
                })
                .CreateMapper())
        {
        }
    }
}