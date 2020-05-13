using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.ApiControllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin/{controller}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ProfileRanksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ProfileRanksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProfileRanks
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileRank>>> GetProfileRanks()
        {
            return await _context.ProfileRanks.ToListAsync();
        }

        // GET: api/ProfileRanks/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileRank>> GetProfileRank(Guid id)
        {
            var profileRank = await _context.ProfileRanks.FindAsync(id);

            if (profileRank == null)
            {
                return NotFound();
            }

            return profileRank;
        }

        // PUT: api/ProfileRanks/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profileRank"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfileRank(Guid id, ProfileRank profileRank)
        {
            if (id != profileRank.Id)
            {
                return BadRequest();
            }

            _context.Entry(profileRank).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileRankExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProfileRanks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileRank"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProfileRank>> PostProfileRank(ProfileRank profileRank)
        {
            _context.ProfileRanks.Add(profileRank);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfileRank", new { id = profileRank.Id }, profileRank);
        }

        // DELETE: api/ProfileRanks/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProfileRank>> DeleteProfileRank(Guid id)
        {
            var profileRank = await _context.ProfileRanks.FindAsync(id);
            if (profileRank == null)
            {
                return NotFound();
            }

            _context.ProfileRanks.Remove(profileRank);
            await _context.SaveChangesAsync();

            return profileRank;
        }

        private bool ProfileRankExists(Guid id)
        {
            return _context.ProfileRanks.Any(e => e.Id == id);
        }
    }
}
