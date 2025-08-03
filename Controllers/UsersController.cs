using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Linq;
using System.Security.Claims;
using TestingApi1.Models;
using TestingApi1.SaveData;

namespace TestingApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
                _context = context;
        }

        #region GetAllUser
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _context.Users.ToListAsync();
            if (user != null)
            {
                var userList = user.Select(u => new
                {
                   id= u.Id,
                   userName= u.Username,
                   email= u.Email,
                   password =u.PasswordHash,
                   roleId= u.Role,
                   roleName= u.Role.ToString(),
                   createdDate=u.CreatedAt.ToString()


                });
                return Ok(new { msg = "Record Received Successfully",status=200, data = userList });
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
        #region UpdateRole
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserRoleUpdate(int id, [FromBody] int role)
        {
            if (id == null )
            {
                return BadRequest(new { status = 400, msg = "please enter id" });
            }
            if (role == null )
            {
                return BadRequest(new { status = 400, msg = "please enter role" });
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            { 
                user.Role = (Models.UserRole)role;
                
                await _context.SaveChangesAsync();
                string msg = "Role Update Successfully";
                int status = 200;
                return Ok(new { msg, status });
            }
            else
            {
                return Ok(new { msg = "No User Found", status=204  });
            }
            
        }
        #endregion
        #region GetUserprofile
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserprofile()
        {
            var claims = User.Claims;
            string userId = claims.Where(c => c.Type == ClaimTypes.PrimarySid).FirstOrDefault().Value;
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id.ToString()==userId);
            if (user != null)
            {
                return Ok(new { msg = "Record Received Successfully", status = 200, user.Username, user.Email, Role = user.Role.ToString() });
                
            }
            else
            {
                return NotFound();
            }
        }
        #endregion
    }
}
