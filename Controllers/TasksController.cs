using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TestingApi1.Models;
using TestingApi1.SaveData;

namespace TestingApi1.Controllers
{
    [Route("api")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }
        #region TaskAdd
        [HttpPost("projects/{projectId}/tasks")]
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        public async Task<IActionResult> TaskAdd(int projectId, Taskss obj)
        {
            if (obj.Title == null || obj.Title == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Title" });
            }
            if (obj.Description == null || obj.Description == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Description" });
            }
            if (obj.AssignedToId == null)
            {
                return BadRequest(new { status = 400, msg = "please enter AssignedToId" });
            }

            if (_context.Tasks.Any(u => u.Title == obj.Title && u.ProjectId == obj.projectId))
                return BadRequest("task already registered.");
            var project = await _context.Projects.FindAsync(projectId);

            if (project != null)
            {
                var claims = User.Claims;
                string userId = claims.Where(c => c.Type == ClaimTypes.PrimarySid).FirstOrDefault().Value;
                string role = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
                if (role == UserRole.Developer.ToString() && !project.DeveloperIds.Contains(userId))
                {
                    return Forbid("You can not create task which is not assigned to you");
                }
                var user = new Tasks
                {
                    Title = obj.Title,
                    Description = obj.Description,
                    ProjectId = projectId.ToString(),
                    AssignedToId = obj.AssignedToId,
                    DueDate = obj.DueDate,
                    Status = obj.Status

                };
                _context.Tasks.Add(user);
                await _context.SaveChangesAsync();
                string msg = "Task Added Successfully";
                int status = 201;
                return Ok(new { msg, status });

            }
            else
            {
                return NotFound("No project are found");
            }
        }
        #endregion
        #region TaskUpdate
        [HttpPut("tasks/{id}")]
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        public async Task<IActionResult> TaskUpdate(int id, [FromBody] Taskss obj)
        {

            if (obj.Title == null || obj.Title == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Title" });
            }
            if (obj.Description == null || obj.Description == "")
            {
                return BadRequest(new { status = 400, msg = "please enter Description" });
            }
            if (obj.projectId == null || obj.projectId == "")
            {
                return BadRequest(new { status = 400, msg = "please enter projectId" });
            }
            if (obj.AssignedToId == null)
            {
                return BadRequest(new { status = 400, msg = "please enter AssignedToId" });
            }

            var user = await _context.Tasks.FindAsync(id);
            var project = await _context.Projects.FindAsync(int.Parse(obj.projectId));
            if (project != null)
            {
                var claims = User.Claims;
                string userId = claims.Where(c => c.Type == ClaimTypes.PrimarySid).FirstOrDefault().Value;
                string role = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
                if (role == UserRole.Developer.ToString() && !project.DeveloperIds.Contains(userId) && obj.AssignedToId != userId)
                {
                    return Forbid("You can not update task which is not assigned to you");
                }

                if (role == UserRole.ProjectManager.ToString() && project.ProjectManagerId != userId)
                {
                    return Forbid("Project Managers can only update own project .");
                }
                if (user != null)
                {

                    user.Title = obj.Title;
                    user.Description = obj.Description;
                    user.ProjectId = obj.projectId;
                    user.AssignedToId = obj.AssignedToId;
                    user.DueDate = obj.DueDate;
                    user.Status = obj.Status;

                    await _context.SaveChangesAsync();
                    string msg = "Task Update Successfully";
                    int status = 200;
                    return Ok(new { msg, status });
                }
                else
                {
                    return NotFound(new { msg = "task not found", status = 404 });
                }
            }
            else
            {
                return NotFound(new { msg = "project not found", status = 404 });
            }
        }
        #endregion
        #region DeleteTask
        [HttpDelete("tasks/{id}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var user = await _context.Tasks.FindAsync(id);
            if (user != null)
            {
                _context.Tasks.Remove(user);
                _context.SaveChanges();
                return Ok(new { msg = "Record Delete Successfully", status = 200 });
            }
            else
            {
                return NoContent();
            }
        }
        #endregion

        #region GetByProjectId
        [HttpGet("projects/{projectId}/tasks")]
        [Authorize]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var user = await _context.Tasks.FindAsync(projectId);
            if (user != null)
            {
                return Ok(new { user.Title, user.Description, user.ProjectId, user.AssignedToId, role = user.Status.ToString() });
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
    }
}
