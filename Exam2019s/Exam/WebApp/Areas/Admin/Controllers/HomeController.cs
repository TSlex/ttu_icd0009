using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Home
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Get admin panel
        /// </summary>
        /// <returns></returns>
        [Route("admin/panel")]
        public IActionResult Index()
        {
            return View();
        }
    }
}