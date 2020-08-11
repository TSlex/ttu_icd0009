using BLL.App.Services;
using ee.itcollege.aleksi.BLL.Base;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;


namespace BLL.App
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        public AppBLL(IAppUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ITemplateService Templates => GetService<ITemplateService>(() => new TemplateService(UnitOfWork));
    }
}