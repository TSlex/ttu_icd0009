using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    /// <summary>
    /// URL ROUTER (kinda :P)
    /// </summary>
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