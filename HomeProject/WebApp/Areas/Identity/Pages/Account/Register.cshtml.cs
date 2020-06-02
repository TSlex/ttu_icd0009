using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Contracts.BLL.App;
using BLL.App.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Profile = Domain.Profile;

#pragma warning disable 1591
namespace WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Profile> _signInManager;
        private readonly UserManager<Profile> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IAppBLL _bll;

        public RegisterModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IAppBLL bll)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _bll = bll;
        }

        [BindProperty] public InputModel Input { get; set; } = default!;

        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; } = default!;

        public class InputModel
        {
            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [EmailAddress(ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
                ErrorMessageResourceName = "InvalidEmail")]
            [Display(Name = nameof(Email),
                ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            public string Email { get; set; } = default!;

            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [Display(Name = "UserName",
                ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")]
            public string Username { get; set; } = default!;

            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")]
            [MinLength(6, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MinLength")]
            [DataType(DataType.Password)]
            [Display(Name = nameof(Password),
                ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            public string Password { get; set; } = default!;

            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [DataType(DataType.Password)]
            [Display(Name = "PasswordConfirm",
                ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [Compare("Password", ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
                ErrorMessageResourceName = "PasswordMatchError")]
            public string ConfirmPassword { get; set; } = default!;
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new Profile() {UserName = Input.Username, Email = Input.Email, EmailConfirmed = true};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //add default rank
                    _bll.ProfileRanks.Add(new ProfileRank()
                    {
                        ProfileId = user.Id,
                        RankId = (await _bll.Ranks.FindByCodeAsync("X_00")).Id
                    });

                    await _bll.SaveChangesAsync();

                    /*var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new {area = "Identity", userId = user.Id, code = code},
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
*/
                    /*if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new {email = Input.Email});
                    }
                    else
                    {*/
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
//                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}