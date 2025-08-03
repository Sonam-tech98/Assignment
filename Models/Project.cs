using System.ComponentModel.DataAnnotations;

namespace TestingApi1.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectManagerId { get; set; }
        public List<string> DeveloperIds { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public ProjectStatus Status { get; set; }

    }

    public enum ProjectStatus
    {
        NotStarted, InProgress, Completed
    }
}