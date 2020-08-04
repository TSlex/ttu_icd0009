using ee.itcollege.aleksi.Contracts.DAL.Base;
using ee.itcollege.aleksi.DAL.Base;

namespace BLL.App.DTO.Base
{
    public class DomainEntityBaseMetaSoftDelete: DomainEntityBaseMetadata, ISoftDeleteEntity
    {
    }
}