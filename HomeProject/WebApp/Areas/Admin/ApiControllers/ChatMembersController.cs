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
    public class ChatMembersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ChatMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChatMembers
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatMember>>> GetChatMembers()
        {
            return await _context.ChatMembers.ToListAsync();
        }

        // GET: api/ChatMembers/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatMember>> GetChatMember(Guid id)
        {
            var chatMember = await _context.ChatMembers.FindAsync(id);

            if (chatMember == null)
            {
                return NotFound();
            }

            return chatMember;
        }

        // PUT: api/ChatMembers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chatMember"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatMember(Guid id, ChatMember chatMember)
        {
            if (id != chatMember.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMemberExists(id))
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

        // POST: api/ChatMembers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatMember"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ChatMember>> PostChatMember(ChatMember chatMember)
        {
            _context.ChatMembers.Add(chatMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatMember", new { id = chatMember.Id }, chatMember);
        }

        // DELETE: api/ChatMembers/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChatMember>> DeleteChatMember(Guid id)
        {
            var chatMember = await _context.ChatMembers.FindAsync(id);
            if (chatMember == null)
            {
                return NotFound();
            }

            _context.ChatMembers.Remove(chatMember);
            await _context.SaveChangesAsync();

            return chatMember;
        }

        private bool ChatMemberExists(Guid id)
        {
            return _context.ChatMembers.Any(e => e.Id == id);
        }
    }
}
