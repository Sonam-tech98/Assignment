using TestingApi1.Models;

namespace TestingApi1.SaveData
{
    public class Taskss
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? projectId { get; set; }
        public string AssignedToId { get; set; }
        public DateTime? DueDate { get; set; }
        public TasksStatus Status { get; set; }
    }
}
