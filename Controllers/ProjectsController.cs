using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Security.Claims;
using TestingApi1.Models;
using TestingApi1.SaveData;

namespace TestingApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProjectsController(ApplicationDbContext context)
        {
                _context = context; 
        }
        #region ProjectAdd
        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> ProjectAdd(Projects obj)
        {
            if (obj.Name == null || obj.Name == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Name" });
            }
            if (obj.Description == null || obj.Description == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Description" });
            }
            if (obj.ProjectManagerId == null || obj.ProjectManagerId == "")
            {
                return BadRequest(new { status = 400, msg = "please enter ProjectManagerId" });
            }
            if (obj.DeveloperIds == null)
            {
                return BadRequest(new { status = 400, msg = "please enter DeveloperIds" });
            }
            if (_context.Projects.Any(u => u.Name == obj.Name))
                return BadRequest("project already registered.");

            var user = new Project
            {
                Name = obj.Name,
                Description = obj.Description,
                ProjectManagerId = obj.ProjectManagerId,
                DeveloperIds = obj.DeveloperIds,
                Status=obj.Status

            };
            _context.Projects.Add(user);
            await _context.SaveChangesAsync();
            string msg = "Project Add Successfully";
            int status = 201;
            return Ok(new { msg, status });


        }
        #endregion
        #region ProjectUpdate
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> ProjectUpdate(int id,[FromBody]Projects obj)
        {

            if (obj.Name == null || obj.Name == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Name" });
            }
            if (obj.Description == null || obj.Description == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Description" });
            }
            if (obj.ProjectManagerId == null || obj.ProjectManagerId == "")
            {
                return BadRequest(new { status = 400, msg = "please enter ProjectManagerId" });
            }
            if (obj.DeveloperIds == null)
            {
                return BadRequest(new { status = 400, msg = "please enter DeveloperIds" });
            }
        
              var claims = User.Claims;
            string userId = claims.Where(c => c.Type == ClaimTypes.PrimarySid).FirstOrDefault().Value;
            string role = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
            var user = await _context.Projects.FindAsync(id);
            if (user != null)
            {

                if(role ==UserRole.ProjectManager.ToString() && obj.ProjectManagerId!=userId)
                {
                    return Forbid("You can update only your project not others");
                }
                user.Name = obj.Name;
                user.Description = obj.Description;               
                user.DeveloperIds = obj.DeveloperIds;
                user.Status = obj.Status;
                if (role == UserRole.Admin.ToString())
                {
                    user.ProjectManagerId = obj.ProjectManagerId;
                }
                else
                {
                    return Forbid("You can update only your project not others");
                }
                await _context.SaveChangesAsync();
                string msg = "Project Updated Successfully";
                int status = 200;
                return Ok(new { msg, status });
            }
            else
            {
                return NotFound("Project not available");
            }

            
          
        }
        #endregion
        #region DeleteProject
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var user = await _context.Projects.FindAsync(id);
            if (user != null)
            {
                _context.Projects.Remove(user);
                _context.SaveChanges();
                return Ok(new { msg = "Record Delete Successfully", status = 200 });
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
        #region GetAllUser
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _context.Projects.ToListAsync();
            if (user != null)
            {
                var projectList = user.Select(u => new
                {
                    id = u.Id,
                    name = u.Name,
                    description = u.Description,
                    projectmanagerId = u.ProjectManagerId,
                    developerId = u.DeveloperIds,
                    statusId=u.Status,
                    status = u.Status.ToString(),
                    createdDate = u.CreatedAt.ToString()


                });
                return Ok(new { msg = "Record Received Successfully", status = 200, data = projectList });
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
        #region GetById
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Projects.FindAsync(id);
            if (user != null)
            {
                return Ok(new { user.Name, user.Description, user.DeveloperIds,user.ProjectManagerId,role=user.Status.ToString() });
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
    }
}
