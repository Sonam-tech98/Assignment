using System.ComponentModel.DataAnnotations;

namespace TestingApi1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
    public enum UserRole
    {
        Admin, ProjectManager, Developer, Viewer
    }
}