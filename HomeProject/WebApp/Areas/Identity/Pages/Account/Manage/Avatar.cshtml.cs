using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Domain;
using Domain.Enums;
using Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Image = BLL.App.DTO.Image;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Model for avater changing
    /// </summary>
    public class AvatarModel : PageModel
    {
        /// <summary>
        /// Status messge
        /// </summary>
        [TempData] public string? StatusMessage { get; set; }
        
//        [BindProperty] public Image ImageModel { get; set; } = default!;
        
        //=====================================================
#pragma warning disable 1591
        public Guid Id { get; set; }

        [BindProperty][MaxLength(300)] public string? ImageUrl { get; set; }
        [BindProperty][MaxLength(300)] public string? OriginalImageUrl { get; set; }
        
        [BindProperty][Range(0, 10000)] public int HeightPx { get; set; }
        [BindProperty][Range(0, 10000)] public int WidthPx { get; set; }
        
        [BindProperty]public int PaddingTop { get; set; }
        [BindProperty]public int PaddingRight { get; set; }
        [BindProperty]public int PaddingBottom { get; set; }
        [BindProperty]public int PaddingLeft { get; set; }

        [BindProperty]public IFormFile? ImageFile { get; set; }
#pragma warning restore 1591
        //=====================================================

        private readonly IAppBLL _bll;
        private readonly UserManager<Profile> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        /// <param name="hostEnvironment"></param>
        /// <param name="userManager"></param>
        public AvatarModel(IAppBLL bll, IWebHostEnvironment hostEnvironment, UserManager<Profile> userManager)
        {
            _bll = bll;
            _userManager = userManager;
            _bll.Images.RootPath = hostEnvironment.WebRootPath;
        }
        
        /// <summary>
        /// Get page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var imageModel = await _bll.Images.FindProfileAsync(User.UserId());

            if (imageModel != null)
            {
                Id = imageModel.Id;
                ImageUrl = imageModel.ImageUrl;
                OriginalImageUrl = imageModel.OriginalImageUrl;

                HeightPx = imageModel.HeightPx;
                WidthPx = imageModel.WidthPx;

                PaddingTop = imageModel.PaddingTop;
                PaddingRight = imageModel.PaddingRight;
                PaddingBottom = imageModel.PaddingBottom;
                PaddingLeft = imageModel.PaddingLeft;
            }

            return Page();
            }
        
        /// <summary>
        /// Update avatar image
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var imageModel = await _bll.Images.FindProfileAsync(User.UserId());

            if (imageModel == null && ImageFile == null)
            {
                ModelState.AddModelError(string.Empty, "Image should be specified");
                return RedirectToPage();
            }
            
            if (ModelState.IsValid)
            {
                if (imageModel == null)
                {
                    imageModel = await _bll.Images.AddProfileAsync(User.UserId(), new Image()
                    {
                        Id = Guid.NewGuid(),
                        ImageType = ImageType.ProfileAvatar,
                        PaddingTop = PaddingTop,
                        PaddingRight = PaddingRight,
                        PaddingBottom = PaddingBottom,
                        PaddingLeft = PaddingLeft,
                        WidthPx = WidthPx,
                        HeightPx = HeightPx,
                        ImageFile = ImageFile,
                        ImageFor = User.UserId()
                    });

                    var profile = await _userManager.FindByIdAsync(User.UserId().ToString());

                    profile.ProfileAvatarId = imageModel.Id;
                    
                    await _bll.SaveChangesAsync();
                    await _userManager.UpdateAsync(profile);
                }
                else
                {
                    imageModel.PaddingTop = PaddingTop;
                    imageModel.PaddingRight = PaddingRight;
                    imageModel.PaddingBottom = PaddingBottom;
                    imageModel.PaddingLeft = PaddingLeft;
                    imageModel.ImageFile = ImageFile;
                    imageModel.WidthPx = WidthPx;
                    imageModel.HeightPx = HeightPx;
                    
                    await _bll.Images.UpdateProfileAsync(User.UserId(), imageModel);
                    await _bll.SaveChangesAsync();
                }

                StatusMessage = Resourses.Views.Identity.Identity.AvatarUpdateStatusSuccess;
            }

            return RedirectToPage();
        }
    }
}