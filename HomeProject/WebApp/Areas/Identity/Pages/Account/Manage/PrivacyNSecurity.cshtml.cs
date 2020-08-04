using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

#pragma warning disable 1591
namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public class PrivacyNSecurityModel : PageModel
    {
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}";

        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<PrivacyNSecurityModel> _logger;
        private readonly IAppBLL _bll;

        public PrivacyNSecurityModel(UserManager<Profile> userManager, SignInManager<Profile> signInManager,
            ILogger<PrivacyNSecurityModel> logger, IAppBLL bll)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _bll = bll;
        }

//        public bool HasAuthenticator { get; set; }
//        public bool IsMachineRemembered { get; set; }
//        
//        public int RecoveryCodesLeft { get; set; }
//        
//        [BindProperty]
//        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
//            ErrorMessageResourceName = "ErrorMessage_Required")]
//        public bool Is2faEnabled { get; set; } = default!;

        [BindProperty] public InputModel Input { get; set; } = default!;

        public class InputModel
        {
            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [Display(Name = nameof(Password),
                ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [DataType(DataType.Password)]
            public string Password { get; set; } = default!;
        }

        /// <summary>
        /// Message to notify user
        /// </summary>
        [TempData]
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

//            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
//            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
//            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
//            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

            return Page();
        }

//        public async Task<IActionResult> OnPost()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
//            }
//
//            await _signInManager.ForgetTwoFactorClientAsync();
//            /*StatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";*/
//            return RedirectToPage();
//        }

        public async Task<IActionResult> OnPostDownloadPersonalData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.",
                _userManager.GetUserId(User));

            // Only include personal data for download
            var personalDataProps = typeof(Profile).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            var personalData = personalDataProps.ToDictionary(p => p.Name, p => p.GetValue(user)?.ToString() ?? "null");

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData, Formatting.Indented)), "text/json");
        }

        public async Task<IActionResult> OnPostDeleteProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var requirePassword = await _userManager.HasPasswordAsync(user);
            if (requirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty,
                        Resourses.BLL.App.DTO.Profiles.Profiles.ErrorIncorrectPassword);
                    return Page();
                }
            }

            _bll.Profiles.Remove(user.Id);

            var result = await _bll.Profiles.GetForUpdateAsync(user.Id);

            if (result.DeletedAt == null)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{user.Id}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);

            return Redirect("~/");
        }
    }
}