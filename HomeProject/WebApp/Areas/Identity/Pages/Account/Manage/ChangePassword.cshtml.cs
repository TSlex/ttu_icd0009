using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

#pragma warning disable 1591
namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; } = default!;

        /// <summary>
        /// Message to notify user
        /// </summary>
        [TempData]
        public string? StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [DataType(DataType.Password)]
            [Display(Name = "CurrentPassword", ResourceType = typeof(Resourses.Views.Identity.Identity))]
            public string OldPassword { get; set; } = default!;

            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [StringLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "NewPassword", ResourceType = typeof(Resourses.Views.Identity.Identity))]
            public string NewPassword { get; set; } = default!;

            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [DataType(DataType.Password)]
            [Display(Name = "NewPasswordConfirm", ResourceType = typeof(Resourses.Views.Identity.Identity))]
            [Compare("NewPassword", ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
                ErrorMessageResourceName = "PasswordMatchError")]
            public string ConfirmPassword { get; set; } = default!;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult =
                await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Profiles.Profiles.ErrorIncorrectPassword);

                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = Resourses.Views.Identity.Identity.PasswordDataUpdateStatusSuccess;

            return RedirectToPage();
        }
    }
}