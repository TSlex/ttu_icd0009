using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProfileGiftsController : Controller
    {
        private readonly IAppBLL _bll;

        public ProfileGiftsController(IAppBLL bll)
        {
            _bll = bll;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ProfileGifts.AllAsync());
        }
        
        public async Task<IActionResult> Details(Guid id)
        {
            var profileGift = await _bll.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }

            return View(profileGift);
        }
        
        public async Task<IActionResult> Create(string username, string? returnUrl)
        {
            var gifts = await _bll.Gifts.AllAsync();
            return View(new ProfileGiftCreate()
            {
                Profile = new ProfileFull{UserName = username},
                GiftGallery = gifts,
                ReturnUrl = returnUrl
            });
        }
        
        public async Task<IActionResult> CreateConfirm(string username, Guid giftId, string? returnUrl)
        {
            var gift = await _bll.Gifts.FindAsync(giftId);

            return View(new ProfileGift
            {
                Profile = new ProfileFull{UserName = username},
                Gift = gift,
                GiftId = giftId,
                Price = gift.Price,
                ReturnUrl = returnUrl
            });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm(ProfileGift profileGift)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(profileGift.Profile!.UserName);

            if (user == null)
            {
                if (profileGift.ReturnUrl != null)
                {
                    return Redirect(profileGift.ReturnUrl);
                }
                
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.Clear();
            profileGift.ProfileId = user.Id;

            if (TryValidateModel(profileGift))
            {
                await _bll.Ranks.IncreaseUserExperience(User.UserId(), 10);
                
                profileGift.Id = Guid.NewGuid();
                profileGift.Profile = null;
                profileGift.GiftDateTime = DateTime.Now;
                
                _bll.ProfileGifts.Add(profileGift);
                await _bll.SaveChangesAsync();
                
                if (profileGift.ReturnUrl != null)
                {
                    return Redirect(profileGift.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            
            foreach (var modelState in ViewData.ModelState.Values) {
                foreach (ModelError error in modelState.Errors) {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(profileGift);
        }
        
        public async Task<IActionResult> Edit(Guid id)
        {


            var profileGift = await _bll.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }


            return View(profileGift);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProfileGift profileGift)
        {
            if (id != profileGift.Id || User.UserId() != profileGift.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.ProfileGifts.UpdateAsync(profileGift);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(profileGift);
        }
        
        public async Task<IActionResult> Delete(Guid id)
        {

            var profileGift = await _bll.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }

            return View(profileGift);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _bll.ProfileGifts.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}