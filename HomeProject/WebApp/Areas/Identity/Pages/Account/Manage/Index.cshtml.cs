using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#pragma warning disable 1591
namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;

        public IndexModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

//        public string Username { get; set; }

        [TempData] public string? StatusMessage { get; set; }

        [BindProperty] public InputModel Input { get; set; } = default!;

        public class InputModel
        {
            [Phone(ErrorMessageResourceType = typeof(Resourses.Views.Identity.Identity),
                ErrorMessageResourceName = "InvalidPhone")]
            [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")]
            [Display(Name = nameof(PhoneNumber), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            public string? PhoneNumber { get; set; }

            [Display(Name = nameof(UserName), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")]
            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            public string UserName { get; set; } = default!;

            [Display(Name = nameof(ProfileFullName), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [MinLength(1, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MinLength")]
            [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")]
            public string? ProfileFullName { get; set; }

            [Display(Name = nameof(ProfileWorkPlace), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")]
            public string? ProfileWorkPlace { get; set; }

            [Display(Name = nameof(ProfileAbout), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [MaxLength(1000, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")]
            public string? ProfileAbout { get; set; }

            [Display(Name = nameof(ProfileGender), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            public ProfileGender ProfileGender { get; set; } = default!;

            [Display(Name = nameof(ProfileGenderOwn), ResourceType = typeof(Resourses.BLL.App.DTO.Profiles.Profiles))]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")]
            public string? ProfileGenderOwn { get; set; }
        }

        private async Task LoadAsync(Profile user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                UserName = userName,
                ProfileFullName = user.ProfileFullName,
                ProfileWorkPlace = user.ProfileWorkPlace,
                ProfileAbout = user.ProfileAbout,
                PhoneNumber = phoneNumber,
                ProfileGender = user.ProfileGender,
                ProfileGenderOwn = user.ProfileGenderOwn
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //setup new phone
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            //setup new username
            var username = await _userManager.GetUserNameAsync(user);
            if (Input.UserName != username)
            {
                var userCheck = await _userManager.FindByNameAsync(Input.UserName);

                if (userCheck != null && !(userCheck.Equals(user)))
                {
                    StatusMessage = Resourses.Views.Identity.Identity.ProfileDataUpdateStatusUsernameExists;
                    return RedirectToPage();
                }

                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.UserName);

                if (!setUserNameResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting username for user with ID '{userId}'.");
                }
            }
            
            user = await _userManager.GetUserAsync(User);

            //setup other
            user.ProfileFullName = Input.ProfileFullName;
            user.ProfileWorkPlace = Input.ProfileWorkPlace;
            user.ProfileAbout = Input.ProfileAbout;
            user.ProfileGender = Input.ProfileGender;
            user.ProfileGenderOwn = Input.ProfileGenderOwn;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = Resourses.Views.Identity.Identity.ProfileDataUpdateStatusSuccess;

            return RedirectToPage();
        }
    }
}