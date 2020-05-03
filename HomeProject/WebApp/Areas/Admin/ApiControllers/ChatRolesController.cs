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
    public class ChatRolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChatRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatRole>>> GetChatRoles()
        {
            return await _context.ChatRoles.ToListAsync();
        }

        // GET: api/ChatRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatRole>> GetChatRole(Guid id)
        {
            var chatRole = await _context.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }

            return chatRole;
        }

        // PUT: api/ChatRoles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatRole(Guid id, ChatRole chatRole)
        {
            if (id != chatRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatRoleExists(id))
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

        // POST: api/ChatRoles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ChatRole>> PostChatRole(ChatRole chatRole)
        {
            _context.ChatRoles.Add(chatRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatRole", new { id = chatRole.Id }, chatRole);
        }

        // DELETE: api/ChatRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChatRole>> DeleteChatRole(Guid id)
        {
            var chatRole = await _context.ChatRoles.FindAsync(id);
            if (chatRole == null)
            {
                return NotFound();
            }

            _context.ChatRoles.Remove(chatRole);
            await _context.SaveChangesAsync();

            return chatRole;
        }

        private bool ChatRoleExists(Guid id)
        {
            return _context.ChatRoles.Any(e => e.Id == id);
        }
    }
}
