using System.Linq;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;

namespace DAL.Mappers
{
    public class ProfileMapper : BaseDALMapper<Domain.Profile, Profile>
    {
        /*public override Profile Map(Domain.Profile inObject)
        {
            return new Profile
            {
                Comments = inObject.Comments.Select(comment => new CommentMapper().Map(comment)).ToList(),
                
            };
        }*/

        public override Domain.Profile MapReverse(Profile outObject)
        {
            return base.MapReverse(outObject);
        }
    }
}