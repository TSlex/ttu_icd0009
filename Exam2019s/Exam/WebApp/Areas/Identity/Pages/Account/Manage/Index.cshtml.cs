using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    /// <inheritdoc />
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        /// <inheritdoc />
        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// 
        /// </summary>
        [TempData]
        public string? StatusMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// 
            /// </summary>
            [Required]
            [Display(Name = nameof(Username))]
            public string Username { get; set; } = default!;
            
            /// <summary>
            /// 
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string? PhoneNumber { get; set; }
            
            /// <summary>
            /// 
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; } = default!;
            
            /// <summary>
            /// 
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "LastName")]
            public string LastName { get; set; } = default!;
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            
            Input = new InputModel
            {
                Username = userName,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                var userCheck = await _userManager.FindByNameAsync(Input.Username);

                if (userCheck != null && !(userCheck.Equals(user)))
                {
                    StatusMessage = Resources.Views.Identity.Identity.ProfileDataUpdateStatusUsernameExists;
                    return RedirectToPage();
                }

                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.Username);

                if (!setUserNameResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting username for user with ID '{userId}'.");
                }
            }
            
            //setup other
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
