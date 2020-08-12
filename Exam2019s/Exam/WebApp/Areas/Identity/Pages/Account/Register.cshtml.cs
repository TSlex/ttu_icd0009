using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace WebApp.Areas.Identity.Pages.Account
{
    /// <inheritdoc />
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        /// <inheritdoc />
        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, 
            RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// 
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = default!;
            
            /// <summary>
            /// 
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; } = default!;

            /// <summary>
            /// 
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [Display(Name = "LastName")]
            public string LastName { get; set; } = default!;

            /// <summary>
            /// 
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = default!;

            /// <summary>
            /// 
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = default!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task OnGetAsync(string? returnUrl)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            var appUser = await _userManager.FindByEmailAsync(Input.Email);
            if (appUser != null)
            {
                _logger.LogInformation($"WebApi register. User {Input.Email} already registered!");
                ModelState.AddModelError(string.Empty, Resources.Domain.AppUsers.AppUser.ErrorUserAlreadyExists);
            }
            
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = (await GenerateStudentCode(Input.FirstName, Input.LastName, Input.Email)).ToLower(), 
                    Email = Input.Email, EmailConfirmed = true, 
                    FirstName = Input.FirstName, LastName = Input.LastName
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    
                    //add user role
                    var role = await _roleManager.FindByNameAsync("User");

                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        _logger.LogWarning("User role - \"User\" was not found!");
                    }

//                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
//                    var callbackUrl = Url.Page(
//                        "/Account/ConfirmEmail",
//                        pageHandler: null,
//                        values: new {area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl},
//                        protocol: Request.Scheme);
//
//                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
//                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
//
//                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
//                    {
//                        return RedirectToPage("RegisterConfirmation", new {email = Input.Email, returnUrl = returnUrl});
//                    }
//                    else
//                    {
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

        /// <summary>
        /// generates a unique student code
        /// </summary>
        /// <returns></returns>
        private async Task<string> GenerateStudentCode(string firstName, string lastName, string email)
        {
            var fLenght = firstName.Length;
            var fNameLeft = firstName.Substring(0, fLenght / 2);
            
            var lLenght = lastName.Length;
            var lNameRight = lastName.Substring(lLenght / 2);

            var name = fNameLeft + lNameRight;
            var result = await _userManager.FindByNameAsync(name);

            if (result == null)
            {
                return name;
            }

            var counter = 1;
            
            while (true)
            {
                if (counter > 15)
                {
                    return email.Split("@")[0];
                }
                
                result = await _userManager.FindByNameAsync(name + counter);
                
                if (result == null)
                {
                    return name;
                }

                counter++;
            }
        }
    }
}