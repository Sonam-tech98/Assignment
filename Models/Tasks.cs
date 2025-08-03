using System.ComponentModel.DataAnnotations;

namespace TestingApi1.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public string AssignedToId { get; set; }
        public TasksStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
    }
    public enum TasksStatus
    { Pending, InProgress, Completed }
}
