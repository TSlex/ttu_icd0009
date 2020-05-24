using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// Profile ranks
    /// </summary>
    [Authorize]
    public class ProfileRanksController : Controller
    {
        private readonly IAppBLL _bll;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public ProfileRanksController(IAppBLL bll)
        {
            _bll = bll;
        }
        
        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var profileRank = await _bll.ProfileRanks.FindAsync(id);

            if (profileRank == null)
            {
                return NotFound();
            }

            return View(profileRank);
        }
    }
}
