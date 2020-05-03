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
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class BlockedProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public BlockedProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BlockedProfiles
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlockedProfile>>> GetBlockedProfiles()
        {
            return await _context.BlockedProfiles.ToListAsync();
        }

        // GET: api/BlockedProfiles/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BlockedProfile>> GetBlockedProfile(Guid id)
        {
            var blockedProfile = await _context.BlockedProfiles.FindAsync(id);

            if (blockedProfile == null)
            {
                return NotFound();
            }

            return blockedProfile;
        }

        // PUT: api/BlockedProfiles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blockedProfile"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlockedProfile(Guid id, BlockedProfile blockedProfile)
        {
            if (id != blockedProfile.Id)
            {
                return BadRequest();
            }

            _context.Entry(blockedProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockedProfileExists(id))
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

        // POST: api/BlockedProfiles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockedProfile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<BlockedProfile>> PostBlockedProfile(BlockedProfile blockedProfile)
        {
            _context.BlockedProfiles.Add(blockedProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlockedProfile", new { id = blockedProfile.Id }, blockedProfile);
        }

        // DELETE: api/BlockedProfiles/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<BlockedProfile>> DeleteBlockedProfile(Guid id)
        {
            var blockedProfile = await _context.BlockedProfiles.FindAsync(id);
            if (blockedProfile == null)
            {
                return NotFound();
            }

            _context.BlockedProfiles.Remove(blockedProfile);
            await _context.SaveChangesAsync();

            return blockedProfile;
        }

        private bool BlockedProfileExists(Guid id)
        {
            return _context.BlockedProfiles.Any(e => e.Id == id);
        }
    }
}
