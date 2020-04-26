using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class UrlController : Controller
    {
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