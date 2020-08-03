using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

#pragma warning disable 1591
namespace WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IAppBLL _bll;

        public LoginModel(SignInManager<Profile> signInManager,
            ILogger<LoginModel> logger,
            UserManager<Profile> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; } = default!;

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        public string? ReturnUrl { get; set; }

        [TempData] public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [Display(Name = nameof(Email),
                ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [EmailAddress(ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
                ErrorMessageResourceName = "InvalidEmail")]
            public string Email { get; set; } = default!;

            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [Display(Name = nameof(Password),
                ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [DataType(DataType.Password)]
            public string Password { get; set; } = default!;

            [Display(Name = nameof(RememberMe),
                ResourceType = typeof(Resourses.Views.Identity.Identity))]
            public bool RememberMe { get; set; } = default!;
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                // var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
//                    return NotFound($"Unable to load user with Email '{Input.Email}'.");
                    ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorUserNotFound);
                    return Page();
                }

                // var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new {ReturnUrl = returnUrl, RememberMe = Input.RememberMe});
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorUserNotFound);
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}