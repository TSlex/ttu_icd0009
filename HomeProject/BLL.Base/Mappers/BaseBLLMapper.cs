using AutoMapper;
using BLL.App.DTO;
using Contracts.BLL.Base.Mappers;
using Profile = BLL.App.DTO.Profile;

namespace BLL.Base.Mappers
{
    public class BaseBLLMapper<TInObject, TOutObject> : IBaseBLLMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        private readonly IMapper _mapper;

        public BaseBLLMapper() : this(null)
        {
        }

        public BaseBLLMapper(IMapper? mapper)
        {
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();

                config.CreateMap<DAL.App.DTO.ChatRoom, ChatRoom>();
                config.CreateMap<ChatRoom, DAL.App.DTO.ChatRoom>();

                config.CreateMap<DAL.App.DTO.Profile, Profile>();
                config.CreateMap<Profile, DAL.App.DTO.Profile>();

                config.CreateMap<DAL.App.DTO.ChatMember, ChatMember>();
                config.CreateMap<ChatMember, DAL.App.DTO.ChatMember>();

                config.CreateMap<DAL.App.DTO.ChatRole, ChatRole>();
                config.CreateMap<ChatRole, DAL.App.DTO.ChatRole>();

                config.CreateMap<DAL.App.DTO.Post, Post>();
                config.CreateMap<Post, DAL.App.DTO.Post>();
                
                config.CreateMap<DAL.App.DTO.Comment, Comment>();
                config.CreateMap<Comment, DAL.App.DTO.Comment>();
                
                config.CreateMap<DAL.App.DTO.Favorite, Favorite>();
                config.CreateMap<Favorite, DAL.App.DTO.Favorite>();

                config.AllowNullDestinationValues = true;
            }).CreateMapper();
        }

        public virtual TOutObject Map(TInObject inObject)
        {
            return _mapper.Map<TInObject, TOutObject>(inObject);
        }

        public virtual TInObject MapReverse(TOutObject outObject)
        {
            return _mapper.Map<TOutObject, TInObject>(outObject);
        }
    }
}