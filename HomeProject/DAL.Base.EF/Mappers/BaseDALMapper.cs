using AutoMapper;
using Contracts.DAL.Base.Mappers;
using DAL.App.DTO;
using Profile = DAL.App.DTO.Profile;

namespace DAL.Base.EF.Mappers
{
    public class BaseDALMapper<TInObject, TOutObject> : IBaseDALMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        private readonly IMapper _mapper;

        public BaseDALMapper(): this(null)
        {
            
        }
        
        public BaseDALMapper(IMapper? mapper)
        {
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();
                
                config.CreateMap<Domain.ChatRoom, ChatRoom>();
                config.CreateMap<ChatRoom, Domain.ChatRoom>();
                
                config.CreateMap<Domain.Profile, Profile>();
                config.CreateMap<Profile, Domain.Profile>();
                
                config.CreateMap<Domain.ChatMember, ChatMember>();
                config.CreateMap<ChatMember, Domain.ChatMember>();
                
                config.CreateMap<Domain.ChatRole, ChatRole>();
                config.CreateMap<ChatRole, Domain.ChatRole>();
                
                config.CreateMap<Domain.Gift, Gift>();
                config.CreateMap<Gift, Domain.Gift>();
                
                config.CreateMap<Domain.Rank, Rank>();
                config.CreateMap<Rank, Domain.Rank>();
                
                config.CreateMap<Domain.ProfileRank, ProfileRank>();
                config.CreateMap<ProfileRank, Domain.ProfileRank>();
                
                config.CreateMap<Domain.ProfileGift, ProfileGift>();
                config.CreateMap<ProfileGift, Domain.ProfileGift>();
                
                config.CreateMap<Domain.Post, Post>();
                config.CreateMap<Post, Domain.Post>();
                
                config.CreateMap<Domain.Comment, Comment>();
                config.CreateMap<Comment, Domain.Comment>();
                
                config.CreateMap<Domain.Favorite, Favorite>();
                config.CreateMap<Favorite, Domain.Favorite>();
                
                config.CreateMap<Domain.Follower, Follower>();
                config.CreateMap<Follower, Domain.Follower>();
                
                config.CreateMap<Domain.BlockedProfile, BlockedProfile>();
                config.CreateMap<BlockedProfile, Domain.BlockedProfile>();
                
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