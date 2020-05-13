using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// URL ROUTER (kinda :P)
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UrlController : Controller
    {
        /// <summary>
        /// Redirect to an url (if specified)
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IActionResult Return(string? returnUrl)
        {
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}