using BLL.App.DTO;
using BLL.App.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using ee.itcollege.aleksi.BLL.Base.Services;

 namespace BLL.App.Services
{
    public class TemplateService : BaseEntityService<ITemplateRepo, DAL.App.DTO.Template, Template>, ITemplateService
    {
        public TemplateService(IAppUnitOfWork uow) :
            base(uow.Templates, new UniversalBLLMapper<DAL.App.DTO.Template, Template>())
        {
        }
    }
}