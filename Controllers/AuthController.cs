using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestingApi1.Helpers;
using TestingApi1.Models;
using TestingApi1.SaveData;

namespace TestingApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Token _token;
   
        public AuthController(ApplicationDbContext context, Token token)
        {
            _context = context;
            _token = token;
        }

        #region Register
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(Register register)
        {
            if (register.Username == null || register.Username == "")
            {
                return BadRequest(new { status = 400, msg = "please enter UserName" });
            }
            if (register.Email == null || register.Email == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Email" });
            }
            if (register.Password == null || register.Password == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Password" });
            }
            if (register.Role == null)
            {
                return BadRequest(new { status = 400, msg = "please enter Role" });
            }

            if (_context.Users.Any(u => u.Email == register.Email))
                return BadRequest("Email Already Registered.");

            var user = new User
            {

                Username = register.Username,
                Email = register.Email,
                //PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PasswordHash = register.Password,
                Role = (Models.UserRole)register.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            string msg = "Register Successfully";
            int status = 201;
            return Ok(new { msg, status });
        }
        #endregion
        #region Login

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (login.Email == null || login.Email == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Email" });
            }
            if (login.Password == null || login.Password == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Password" });
            }
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.PasswordHash == login.Password);

            if (user == null)
                return Unauthorized("Invalid username or password");

            var token = _token.GenerateJwtToken(user);
            var msg = "Login Successfull";
            int status = 200;
            return Ok(new { msg, status, token });
        } 
        #endregion


    }
}
