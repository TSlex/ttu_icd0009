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
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ProfileGiftsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfileGiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProfileGifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileGift>>> GetProfileGifts()
        {
            return await _context.ProfileGifts.ToListAsync();
        }

        // GET: api/ProfileGifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileGift>> GetProfileGift(Guid id)
        {
            var profileGift = await _context.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }

            return profileGift;
        }

        // PUT: api/ProfileGifts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfileGift(Guid id, ProfileGift profileGift)
        {
            if (id != profileGift.Id)
            {
                return BadRequest();
            }

            _context.Entry(profileGift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileGiftExists(id))
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

        // POST: api/ProfileGifts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ProfileGift>> PostProfileGift(ProfileGift profileGift)
        {
            _context.ProfileGifts.Add(profileGift);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfileGift", new { id = profileGift.Id }, profileGift);
        }

        // DELETE: api/ProfileGifts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProfileGift>> DeleteProfileGift(Guid id)
        {
            var profileGift = await _context.ProfileGifts.FindAsync(id);
            if (profileGift == null)
            {
                return NotFound();
            }

            _context.ProfileGifts.Remove(profileGift);
            await _context.SaveChangesAsync();

            return profileGift;
        }

        private bool ProfileGiftExists(Guid id)
        {
            return _context.ProfileGifts.Any(e => e.Id == id);
        }
    }
}
