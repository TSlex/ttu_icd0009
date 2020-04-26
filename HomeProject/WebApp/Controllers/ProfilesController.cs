using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    [Route("/{username}")]
    public class ProfilesController : Controller
    {
        private readonly UserManager<Profile> _userManager;
        private readonly IAppBLL _bll;

        public ProfilesController(UserManager<Profile> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }
        
        public async Task<IActionResult> Index(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            
            if (user == null)
            {
                return NotFound();
            }

            var userModel = await _bll.Profiles.GetProfileFull(user.Id);
            
            if (userModel == null)
            {
                return NotFound();
            }
            
            return View(userModel);
        }
    }
}
