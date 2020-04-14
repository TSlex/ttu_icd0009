using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
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
            [Phone]
            [MaxLength(300)]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; } = default!;

            [MaxLength(300)] public string Username { get; set; } = default!;
            
            [MinLength(1)] [MaxLength(100)] public string? ProfileFullName { get; set; }
            [MaxLength(300)] public string? ProfileWorkPlace { get; set; }
            [MaxLength(300)] public string? ProfileAvatarUrl { get; set; }
            [MaxLength(1000)] public string? ProfileAbout { get; set; }

            public ProfileGender ProfileGender { get; set; } = default!;
            public string? ProfileGenderOwn { get; set; }
        }

        private async Task LoadAsync(Profile user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {    
                Username = userName,
                ProfileFullName = user.ProfileFullName,
                ProfileWorkPlace = user.ProfileWorkPlace,
                ProfileAvatarUrl = user.ProfileAvatarUrl,
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
            if (Input.Username != username)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                
                if (!setUserNameResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting username for user with ID '{userId}'.");
                }
            }
            
            //setup other
            user.ProfileAvatarUrl = Input.ProfileAvatarUrl;
            user.ProfileFullName = Input.ProfileFullName;
            user.ProfileWorkPlace = Input.ProfileWorkPlace;
            user.ProfileAbout = Input.ProfileAbout;
            user.ProfileGender = Input.ProfileGender;
            user.ProfileGenderOwn = Input.ProfileGenderOwn;
            
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}